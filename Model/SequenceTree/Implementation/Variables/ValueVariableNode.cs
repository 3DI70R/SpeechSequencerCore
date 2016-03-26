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
        public string Value
        {
            get
            {
                return ((IValueNode) m_sequence).Value;
            }
        }

        public override IAudioNode ToAudio()
        {
            return this.WrapValueAsSpeech();
        }
    }
}
