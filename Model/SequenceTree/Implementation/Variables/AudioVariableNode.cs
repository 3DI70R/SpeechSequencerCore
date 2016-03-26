using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("VarAsAudio")]
    [Description("Загружает переменную в виде аудио")]
    public class AudioVariableNode : VariableNode, IAudioNode
    {
        public WaveFormat WaveFormat
        {
            get
            {
                return ((IAudioNode)m_sequence).WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return ((IAudioNode) m_sequence).Read(buffer, offset, count);
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
