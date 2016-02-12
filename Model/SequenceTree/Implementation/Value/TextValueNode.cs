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
    public class TextValueNode : SequenceNode, IValueNode
    {
        public string Value { get; set; } = string.Empty;

        public TextValueNode() { }
        public TextValueNode(string textValue)
        {
            Value = textValue;
        }

        protected override void LoadDataFromXml(XmlElement element, IPlaybackContext context)
        {
            Value = element.InnerText.Trim();
            base.LoadDataFromXml(element, context);
        }
    }
}
