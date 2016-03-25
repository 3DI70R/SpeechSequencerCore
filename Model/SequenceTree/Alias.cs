using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{

    /*
        <Alias name="Test">

            <Arguments>
                <Argument name="arg1" />
                <Argument name="arg2" />
                <Argument name="arg3" >
                    // default sequence for argument, optional
                </Argument>
            </Arguments>
            
            <Sequence>
                // sequence
            </Sequence>

        </Alias>
    */

    [XmlElementBinding("Alias")]
    public abstract class Alias
    {
        private string m_name;
        private string[] m_variables;
        private XmlElement m_xmlData;

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

        public ISequenceNode CreateNode(Context context, params Func<ISequenceNode>[] arguments)
        {
            return null;
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
