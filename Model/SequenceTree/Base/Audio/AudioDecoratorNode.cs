using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AudioDecoratorNode : AudioNode, IDecoratorNode
    {
        public virtual IAudioNode DecoratedNode { get; set; }
        public abstract bool IsRedundant { get; }

        public override WaveFormat WaveFormat
        {
            get
            {
                return DecoratedNode.WaveFormat;
            }
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);
            DecoratedNode.InitNewState(context);
        }

        public void InitDecorator(IPlaybackContext context)
        {
            XmlBinder.BindValues(this, XmlData, context);
        }
    }
}
