using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class SequenceFactory
    {
        private struct DecoratorInfo
        {
            public Type decoratorType;
            public Func<IAudioDecoratorNode> ctor;
            public HashSet<string> triggeredProperties;

            public override int GetHashCode()
            {
                return decoratorType.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is Type)
                {
                    Type otherType = (Type)obj;
                    return decoratorType == otherType;
                }
                else if (obj is DecoratorInfo)
                {
                    DecoratorInfo otherDecorator = (DecoratorInfo)obj;
                    return decoratorType == otherDecorator.decoratorType;
                }

                return base.Equals(obj);
            }
        }

        private static readonly SequenceFactory s_instance = new SequenceFactory();
        public static SequenceFactory Instance { get { return s_instance; } }
        private SequenceFactory() { }

        private Dictionary<string, Func<ISequenceNode>> m_constructors = new Dictionary<string, Func<ISequenceNode>>();
        private List<DecoratorInfo> m_audioDecorators = new List<DecoratorInfo>();

        public void TryRegisterSequenceNode(Type type)
        {
            if(typeof(IAudioDecoratorNode).IsAssignableFrom(type))
            {
                TryRegisterAudioDecoratorNode(type);
            }
            else if (type.IsAssignableFrom(typeof(ISequenceNode)))
            {
                XmlElementBinding binding = type.GetCustomAttribute<XmlElementBinding>();

                if (binding != null)
                {
                    m_constructors[binding.Name] = (Func<ISequenceNode>)type.CreateConstructorDelegate(typeof(Func<>).MakeGenericType(type));
                }
            }
        }
        public void TryRegisterAudioDecoratorNode(Type type)
        {
            if (typeof(IAudioDecoratorNode).IsAssignableFrom(type) && !m_audioDecorators.Contains(new DecoratorInfo() { decoratorType = type }))
            {
                DecoratorInfo decorator = new DecoratorInfo();

                decorator.ctor = (Func<IAudioDecoratorNode>)type.CreateConstructorDelegate(typeof(Func<>).MakeGenericType(type));
                decorator.triggeredProperties = new HashSet<string>();

                foreach (AbstractAttributeField property in type.GetAttributeProperties())
                {
                    decorator.triggeredProperties.Add(property.Name);
                }

                m_audioDecorators.Add(decorator);
            }
        }

        public ISequenceNode CreateSequence(string elementName)
        {
            return m_constructors[elementName]();
        }
        public ISequenceNode CreateSequence(XmlElement element, Context context)
        {
            ISequenceNode node = CreateSequence(element.LocalName);
            node.XmlData = element;

            return DecorateNode(node, element, context);
        }
        public ISequenceNode CreateChildrenAsSequence(XmlElement element, Context context)
        {
            List<ISequenceNode> nodes = new List<ISequenceNode>();

            EnumerateChildren(element, (e) =>
            {
                nodes.Add(CreateSequence(e, context));
            });

            return AssembleResultNode(nodes);
        }

        public ISequenceNode DecorateNode(ISequenceNode node, XmlElement element, Context context)
        {
            if(node is IAudioNode)
            {
                return DecorateAudioNode((IAudioNode) node, element, context);
            }

            return node;
        }
        public IAudioNode DecorateAudioNode(IAudioNode node, XmlElement element, Context context)
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

            foreach (DecoratorInfo decorator in m_audioDecorators)
            {
                if (decorator.triggeredProperties.Overlaps(existingAttrs))
                {
                    IAudioDecoratorNode decoratorNode = decorator.ctor();
                    decoratorNode.XmlData = element;
                    decoratorNode.DecoratedNode = node;
                    node = decoratorNode;
                }
            }

            return node;
        }

        public void EnumerateChildren(XmlElement element, Action<XmlElement> onElementFound)
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

        public static ISequenceNode AssembleResultNode(List<ISequenceNode> nodes)
        {
            if (nodes.Count == 0)
            {
                return new TextValueNode();
            }
            else if (nodes.Count == 1)
            {
                return nodes[0];
            }
            else
            {
                bool hasAudio = false;

                foreach (ISequenceNode node in nodes)
                {
                    if (node is IAudioNode)
                    {
                        hasAudio = true;
                        break;
                    }
                }

                if (hasAudio)
                {
                    SequentialAudioNode sequential = new SequentialAudioNode();

                    foreach (ISequenceNode node in nodes)
                    {
                        sequential.AddNode(node.ToAudio());
                    }

                    return sequential;
                }
                else
                {
                    JoinValueNode join = new JoinValueNode();

                    foreach (ISequenceNode node in nodes)
                    {
                        join.AddNode((IValueNode) node);
                    }

                    return join;
                }
            }
        }
    }
}
