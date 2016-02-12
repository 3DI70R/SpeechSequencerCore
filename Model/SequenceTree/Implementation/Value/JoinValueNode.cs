using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Join")]
    [Description("Объединяет значения лежащие внутри")]
    public class JoinValueNode : ValueCollectionNode<object>
    {
        private string m_value;

        [XmlAttributeBinding]
        [Description("Разделитель")]
        public string Divider { get; set; } = " ";

        public override string Value
        {
            get
            {
                return m_value;
            }
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);

            m_value = ValueUtils.JoinValues(Divider, ChildCount, (i) =>
            {
                IValueNode value = GetNodeAt(i);
                value.InitNewState(context);
                return value.Value;
            });
        }
    }
}
