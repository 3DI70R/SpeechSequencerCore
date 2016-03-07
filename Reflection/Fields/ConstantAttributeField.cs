using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ConstantAttributeField : AbstractAttributeField
    {
        public ConstantAttributeField(PropertyInfo property, string attributeName, Func<string, object> converter) : base(property, attributeName, converter) { }

        public override string GetValueFromElement(XmlElement element, IPlaybackContext context)
        {
            return ResourceManager.Instance.GetConstant(Name);
        }
    }
}
