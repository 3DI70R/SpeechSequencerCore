using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class CachedSample : ISampleProvider
    {
        private WaveFormat m_waveFormat;
        private float[] m_samples;
        private int m_position;

        public CachedSample(WaveFormat format, float[] samples)
        {
            m_waveFormat = format;
            m_samples = samples;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return m_waveFormat;
            }
        }
        public int Read(float[] buffer, int offset, int count)
        {
            int readed = Math.Min(count, m_samples.Length - m_position);

            for (int i = 0; i < readed; i++)
            {
                buffer[offset + i] = m_samples[m_position++];
            }

            return readed;
        }
    }
}
