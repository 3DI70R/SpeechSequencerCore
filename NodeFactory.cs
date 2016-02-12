using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class NodeFactory
    {
        private struct DecoratorInfo
        {
            public Type decoratorType;
            public Func<IDecoratorNode> ctor;
            public HashSet<string> triggeredProperties;

            public override int GetHashCode()
            {
                return decoratorType.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if(obj is Type)
                {
                    Type otherType = (Type) obj;
                    return decoratorType == otherType;
                }
                else if(obj is DecoratorInfo)
                {
                    DecoratorInfo otherDecorator = (DecoratorInfo) obj;
                    return decoratorType == otherDecorator.decoratorType;
                }

                return base.Equals(obj);
            }
        }

        private static readonly NodeFactory s_instance = new NodeFactory();
        public static NodeFactory Instance { get { return s_instance; } }

        private NodeFactory()
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                RegisterTypesFromAssembly(assembly);
            }
        }

        private Dictionary<string, Func<IValueNode>> m_valueNodeCtors = new Dictionary<string, Func<IValueNode>>();
        private Dictionary<string, Func<IAudioNode>> m_audioNodeCtors = new Dictionary<string, Func<IAudioNode>>();
        private Dictionary<string, Func<IAlias>> m_aliasCtors = new Dictionary<string, Func<IAlias>>();
        private List<DecoratorInfo> m_audioDecoratorInfos = new List<DecoratorInfo>();

        private void RegisterTypesFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes)
            {
                if (!type.IsAbstract)
                {
                    RegisterAudioDecoratorNode(type);
                    RegisterValueNode(type);
                    RegisterAudioNode(type);
                    RegisterAliasNode(type);
                }
            }
        }
        private void RegisterTypeWithEmptyCtor<T>(Type type, Type interfaceType, Dictionary<string, Func<T>> dictionary)
        {
            if (interfaceType.IsAssignableFrom(type))
            {
                XmlElementBinding binding = type.GetCustomAttribute<XmlElementBinding>();

                if(binding != null)
                {
                    Func<T> creator = (Func<T>)type.CreateConstructorDelegate(typeof(Func<>).MakeGenericType(type));
                    dictionary[binding.Name] = creator;
                }
                
            }
        }
        public void RegisterValueNode(Type type)
        {
            RegisterTypeWithEmptyCtor(type, typeof(IValueNode), m_valueNodeCtors);
        }
        public void RegisterAudioNode(Type type)
        {
            RegisterTypeWithEmptyCtor(type, typeof(IAudioNode), m_audioNodeCtors);
        }
        public void RegisterAudioDecoratorNode(Type type)
        {
            Type baseClass = typeof(IDecoratorNode);

            if (baseClass.IsAssignableFrom(type) && !m_audioDecoratorInfos.Contains(new DecoratorInfo() { decoratorType = type }))
            {
                DecoratorInfo decorator = new DecoratorInfo();

                decorator.ctor = (Func<IDecoratorNode>)type.CreateConstructorDelegate(typeof(Func<>).MakeGenericType(type));
                decorator.triggeredProperties = new HashSet<string>();

                foreach (AttributeProperty property in type.GetAttributeProperties())
                {
                    decorator.triggeredProperties.Add(property.Name);
                }

                m_audioDecoratorInfos.Add(decorator);
            }
        }
        public void RegisterAliasNode(Type type)
        {
            RegisterTypeWithEmptyCtor(type, typeof(IAlias), m_aliasCtors);
        }

        #region Alias
        public IAlias[] CreateChildrenAlias(XmlElement element)
        {
            List<IAlias> aliases = new List<IAlias>();

            foreach(XmlNode node in element)
            {
                if(node is XmlElement)
                {
                    aliases.Add(CreateAlias((XmlElement)node));
                }
            }

            return aliases.ToArray();
        }

        public IAlias CreateAlias(XmlElement element)
        {
            IAlias alias = CreateAlias(element.LocalName);
            alias.InitFromXml(element);
            return alias;
        }
        public IAlias CreateAlias(string aliasName)
        {
            return m_aliasCtors[aliasName]();
        }
        #endregion

        #region Audio
        public IAudioNode CreateChildrenAsSingleAudio(XmlElement element)
        {
            SequentialAudioNode nodeList = new SequentialAudioNode();

            EnumerateChildAudio(element, (e) =>
            {
                nodeList.AddNode(CreateAudioNode(e));
            });

            return nodeList;
        }
        public void EnumerateChildAudio(XmlElement element, Action<XmlElement> onElementFound)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    if (node.LocalName.IndexOf('.') == -1) // if not attribute ext node
                    {
                        onElementFound((XmlElement)node);
                    }
                }
            }
        }

        public IAudioNode DecorateAudioNode(IAudioNode node, XmlElement element, IPlaybackContext context)
        {
            HashSet<string> existingAttrs = new HashSet<string>();

            for (int i = 0; i < element.Attributes.Count; i++)
            {
                existingAttrs.Add(element.Attributes[i].LocalName);
            }

            foreach (XmlNode child in element.ChildNodes)
            {
                int attrIndex = child.LocalName.IndexOf('.');

                if (attrIndex != -1)
                {
                    existingAttrs.Add(child.LocalName.Substring(attrIndex + 1));
                }
            }

            foreach (DecoratorInfo decorator in m_audioDecoratorInfos)
            {
                if (decorator.triggeredProperties.Overlaps(existingAttrs))
                {
                    IDecoratorNode decoratorNode = decorator.ctor();
                    decoratorNode.XmlData = element;
                    decoratorNode.InitDecorator(context);

                    if (!decoratorNode.IsRedundant)
                    {
                        decoratorNode.DecoratedNode = node;
                        node = decoratorNode;
                    }
                }
            }

            return node;
        }
        public IAudioNode CreateAudioNode(XmlElement element, IPlaybackContext context)
        {
            return DecorateAudioNode(CreateAudioNode(element), element, context);
        }
        private IAudioNode CreateAudioNode(XmlElement element)
        {
            IAudioNode audio = CreateAudioNode(element.LocalName);
            audio.XmlData = element;
            return audio;
        }
        private IAudioNode CreateAudioNode(string nodeName)
        {
            return m_audioNodeCtors[nodeName]();
        }
        #endregion

        #region Value
        public IValueNode CreateChildrenAsSingleValue(XmlElement element)
        {
            JoinValueNode join = new JoinValueNode();

            EnumerateChildValues(element, (e) =>
            {
                join.AddNode(CreateValueNode(e));
            });

            return join;
        }
        public void EnumerateChildValues(XmlElement element, Action<XmlElement> onElementFound)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    string value = node.Value.Trim();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        XmlElement valueElement = element.OwnerDocument.CreateElement("Value");
                        valueElement.InnerText = value;

                        onElementFound(valueElement);
                    }
                }
                else if (node.NodeType == XmlNodeType.Element)
                {
                    if (node.LocalName.IndexOf('.') == -1) // if not attribute ext node
                    {
                        onElementFound((XmlElement)node);
                    }
                }
            }
        }

        public IValueNode CreateValueNode(XmlElement element)
        {
            IValueNode value = CreateValueNode(element.LocalName);
            value.XmlData = element;
            return value;
        }
        public IValueNode CreateValueNode(string nodeName)
        {
            return m_valueNodeCtors[nodeName]();
        }
        #endregion
    }
}
