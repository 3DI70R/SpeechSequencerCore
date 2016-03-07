using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class XmlElementBinding : Attribute
    {
        public string Name;

        public XmlElementBinding() { }
        public XmlElementBinding(string name)
        {
            this.Name = name;
        }
    }
}
