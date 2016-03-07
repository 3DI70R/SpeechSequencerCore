using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ConstantBinding : VariableElementBinding
    {
        public ConstantBinding(string name) : base(name) { }

        public override AbstractAttributeField CreateInstance(PropertyInfo property, string attributeName, Func<string, object> converter)
        {
            return new ConstantAttributeField(property, attributeName, converter);
        }
    }
}
