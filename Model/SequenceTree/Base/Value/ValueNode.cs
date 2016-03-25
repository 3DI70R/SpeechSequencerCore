using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class ValueNode : SequenceNode, IValueNode
    {
        public virtual string Value { get; set; } = string.Empty;

        public override void InitNewState(Context context)
        {
            base.InitNewState(context);

            try
            {
                Value = LoadValue(context);
            }
            catch (Exception e)
            {
                Value = "Произошла ошибка: " + e.Message;
            }
        }

        public abstract string LoadValue(Context context);

        public override IAudioNode ToAudio()
        {
            return ValueUtils.WrapValueAsSpeech(this);
        }
    }
}
