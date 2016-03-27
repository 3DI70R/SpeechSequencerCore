using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Value")]
    [Description("Возвращает внутренний текст в виде значения")]
    public class TextValueNode : ValueNode
    {
        public TextValueNode() { }
        public TextValueNode(string textValue)
        {
            Value = textValue;
        }

        protected override void LoadDataFromXml(XmlElement element, Context context)
        {
            Value = element.InnerText.Trim();
            base.LoadDataFromXml(element, context);
        }

        protected override string InitValue(Context context)
        {
            return Value;
        }
    }
}
