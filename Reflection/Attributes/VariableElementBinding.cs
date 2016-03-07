using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class VariableElementBinding : Attribute
    {
        public string Name;

        public VariableElementBinding() { }
        public VariableElementBinding(string name)
        {
            this.Name = name;
        }

        public abstract AbstractAttributeField CreateInstance(PropertyInfo property, string attributeName, Func<string, object> converter);
    }
}
