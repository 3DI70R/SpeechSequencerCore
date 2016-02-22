using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class Alias : IAlias
    {
        private string m_name;
        private string[] m_variables;
        private XmlElement m_xmlData;

        public int ArgumentCount
        {
            get
            {
                return m_variables.Length;
            }
        }

        public bool IsAudioAlias
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public XmlElement XmlData
        {
            get
            {
                return m_xmlData;
            }
        }

        public abstract IAliasEntryNode CreateNode();

        public string GetAliasArgumentName(int index)
        {
            return m_variables[index];
        }

        public Func<ISequenceNode> GetDefaultArgumentValue(int index)
        {
            return () => new TextValueNode("");
        }

        public void InitFromXml(XmlElement element)
        {
            string arguments = element.GetAttribute("Arguments");
            m_name = element.GetAttribute("Name");
            m_variables = !string.IsNullOrWhiteSpace(arguments) ? arguments.Split(',') : new string[0];
            m_xmlData = element;
        }
    }
}
