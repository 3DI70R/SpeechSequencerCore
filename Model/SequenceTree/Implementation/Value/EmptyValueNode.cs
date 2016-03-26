using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class EmptyValueNode : SequenceNode, IValueNode
    {
        public string Value
        {
            get
            {
                return string.Empty;
            }
        }

        public override IAudioNode ToAudio()
        {
            return new EmptyAudioNode();
        }
    }
}
