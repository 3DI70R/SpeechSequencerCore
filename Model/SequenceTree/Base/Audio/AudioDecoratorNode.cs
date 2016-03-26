using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AudioDecoratorNode : AudioNode, IAudioDecoratorNode
    {
        public virtual IAudioNode DecoratedNode { get; set; }

        public override WaveFormat WaveFormat
        {
            get
            {
                return DecoratedNode.WaveFormat;
            }
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            DecoratedNode.InitNewState(LocalContext);
        }
    }
}
