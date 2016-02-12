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
                return WrappedNode.WaveFormat;
            }
        }

        public AudioAliasNode(IAudioNode node) : base(node) { }

        public int Read(float[] buffer, int offset, int count)
        {
            return WrappedNode.Read(buffer, offset, count);
        }
    }
}
