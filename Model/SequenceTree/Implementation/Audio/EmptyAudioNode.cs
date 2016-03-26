using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class EmptyAudioNode : SequenceNode, IAudioNode
    {
        public WaveFormat WaveFormat
        {
            get
            {
                return AudioManager.Instance.ResampleAudioFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
