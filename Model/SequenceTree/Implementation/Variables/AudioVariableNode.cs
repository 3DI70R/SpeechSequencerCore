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
        private IAudioNode m_audio;

        public WaveFormat WaveFormat
        {
            get
            {
                return m_audio.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return m_audio.Read(buffer, offset, count);
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);

            m_audio = m_sequence.ToAudio();
            m_audio.InitNewState(context);
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
