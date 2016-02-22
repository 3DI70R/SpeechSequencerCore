using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class AudioAliasNode : AliasEntryNode<IAudioNode>, IAudioNode
    {
        public WaveFormat WaveFormat
        {
            get
            {
                return RootNode.WaveFormat;
            }
        }

        public AudioAliasNode(IAudioNode node) : base(node) { }

        public int Read(float[] buffer, int offset, int count)
        {
            return RootNode.Read(buffer, offset, count);
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
