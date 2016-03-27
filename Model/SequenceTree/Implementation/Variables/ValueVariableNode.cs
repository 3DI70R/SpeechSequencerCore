using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("VarAsValue")]
    [Description("Загружает переменную в виде значения")]
    public class ValueVariableNode : VariableNode, IValueNode
    {
        private IValueNode m_value;

        public string Value
        {
            get
            {
                return m_value.Value;
            }
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            m_value = (IValueNode) m_sequence;
            m_value.InitNewState(context);
        }

        public override IAudioNode ToAudio()
        {
            return this.WrapValueAsSpeech();
        }
    }
}
