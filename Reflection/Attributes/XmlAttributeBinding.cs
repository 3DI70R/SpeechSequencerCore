using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class XmlAttributeBinding : Attribute
    {
        public string Name;

        public XmlAttributeBinding() { }
        public XmlAttributeBinding(string name)
        {
            this.Name = name;
        }
    }
}
