using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Random")]
    [Description("Случайное значение")]
    public class RandomValueNode : JoinValueNode
    {
        private string m_value;

        [XmlAttributeBinding]
        [Description("Количество выборок элементов")]
        public int Count { get; set; } = 1;

        public override string Value
        {
            get
            {
                return m_value;
            }
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            m_value = ValueUtils.JoinValues(Divider, Count, (i) =>
            {
                IValueNode value = GetNodeAt(context.SharedRandom.Next(ChildCount));
                value.InitNewState(LocalContext);
                return value.Value;
            });
        }
    }
}
