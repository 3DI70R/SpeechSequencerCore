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

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);

            try
            {
                Value = InitValue(context);
            }
            catch (Exception e)
            {
                Value = "Произошла ошибка: " + e.Message;
            }
        }

        protected abstract string InitValue(Context context);

        public override IAudioNode ToAudio()
        {
            return ValueUtils.WrapValueAsSpeech(this);
        }
    }
}
