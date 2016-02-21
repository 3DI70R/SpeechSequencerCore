using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AudioNode : SequenceNode, IAudioNode
    {
        public abstract WaveFormat WaveFormat { get; }
        public abstract int Read(float[] buffer, int offset, int count);

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
