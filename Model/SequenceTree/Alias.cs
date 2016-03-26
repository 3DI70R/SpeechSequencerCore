using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class Alias
    {
        private struct VariableInfo
        {
            public string name;
            public Func<Context, ISequenceNode> defaultCreator;
        }

        private string m_name;
        private List<VariableInfo> m_variableInfos;
        private Func<Context, ISequenceNode> m_sequenceFactory;

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public ISequenceNode CreateNode(Context context, params Func<ISequenceNode>[] arguments)
        {
            ISequenceNode sequence = m_sequenceFactory(context);

            if(m_variableInfos.Count != 0)
            {
                for(int i = 0; i < m_variableInfos.Count; i++)
                {
                    VariableInfo info = m_variableInfos[i];
                    Func<ISequenceNode> variable = (i < arguments.Length) ? arguments[i] : () => info.defaultCreator(context);
                    sequence.OverrideVariable(info.name, variable);
                }
            }

            return sequence;
        }

        public void InitFromXml(XmlElement element)
        {
            m_name = element.GetAttribute("Name");

            XmlElement sequenceNode = element["Sequence"];
            XmlElement argumentsNode = element["Arguments"];

            m_sequenceFactory = (c) => SequenceFactory.Instance.CreateChildrenAsSequence(sequenceNode, c);
            m_variableInfos = new List<VariableInfo>();

            foreach (XmlNode childNode in argumentsNode.ChildNodes)
            {
                if(childNode.NodeType == XmlNodeType.Element && childNode.Name == "Argument")
                {
                    XmlElement childElement = (XmlElement) childNode;
                    VariableInfo info = new VariableInfo();

                    info.name = childElement.GetAttribute("Name");

                    if(string.IsNullOrWhiteSpace(childElement.InnerText))
                    {
                        info.defaultCreator = (c) => new EmptyValueNode();
                    }
                    else
                    {
                        info.defaultCreator = (c) => SequenceFactory.Instance.CreateChildrenAsSequence(childElement, c);
                    }

                    m_variableInfos.Add(info);
                }
            }
        }
    }
}
