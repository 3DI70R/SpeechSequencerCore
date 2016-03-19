using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class NoteSequencerNode<P> : MultiUseAudioCollectionNode<P> where P : new()
    {
        public static readonly int c_bufferSize = 8192;

        [XmlAttributeBinding("FadeOut")]
        public float FadeOutLength { get; set; } = 0.25f;

        private class PlayingSample
        {
            private ISampleProvider sourceSample;

            public readonly int channel;
            public readonly int playbackStartTick;
            public readonly float[] buffer;

            public int fadeOutStart;
            public int bufferStartTick;
            public int bufferActualLength;

            public PlayingSample(ISampleProvider sample, int channel, int currentTick)
            {
                this.channel = channel;
                this.sourceSample = sample;
                this.buffer = new float[c_bufferSize];
                this.bufferStartTick = currentTick;
                this.playbackStartTick = currentTick;
                this.fadeOutStart = int.MaxValue;

                FillBufferWithAnotherChunkOfData();
            }

            public void FillBufferWithAnotherChunkOfData()
            {
                if (sourceSample != null)
                {
                    bufferActualLength = sourceSample.Read(buffer, 0, buffer.Length);
                }
                else
                {
                    bufferActualLength = 0;
                }
            }
        }

        private List<PlayingSample> m_playingSamples;
        protected int m_currentTick;
        protected int m_fadeOutTicks;

        public NoteSequencerNode()
        {
            m_playingSamples = new List<PlayingSample>();
        }

        public abstract bool IsFinished { get; }

        public override int Read(float[] buffer, int offset, int count)
        {
            int actuallyRead = 0;

            for (int i = 0; i < count; i++)
            {
                if (m_playingSamples.Count == 0 && IsFinished)
                {
                    break;
                }

                int bufferIndex = offset + i;
                buffer[bufferIndex] = 0;

                OnSampleRead(m_currentTick);

                for (int j = m_playingSamples.Count - 1; j >= 0; j--)
                {
                    PlayingSample sample = m_playingSamples[j];

                    int fadeCount = (m_currentTick - m_currentTick % 2) - sample.fadeOutStart;
                    float fadeOut = 1;

                    if(fadeCount > 0)
                    {
                        float fadeFactor = fadeCount / (float) m_fadeOutTicks;

                        if(fadeFactor > 1)
                        {
                            fadeOut = 0;
                            sample.bufferActualLength = 0;
                        }
                        else
                        {
                            fadeOut = 1 - fadeFactor;
                        }
                    }

                    int sampleBufferIndex = m_currentTick - sample.bufferStartTick;

                    if (sample.bufferActualLength == sampleBufferIndex)
                    {
                        sample.FillBufferWithAnotherChunkOfData();
                        sampleBufferIndex = 0;
                        sample.bufferStartTick = m_currentTick;
                    }

                    if (sample.bufferActualLength == 0)
                    {
                        m_playingSamples.RemoveAt(j);
                        continue;
                    }
                    else
                    {
                        buffer[bufferIndex] += sample.buffer[sampleBufferIndex] * fadeOut;
                    }
                }

                actuallyRead++;
                m_currentTick++;
            }

            return actuallyRead;
        }

        public void Reset()
        {
            m_playingSamples.Clear();
        }

        public void PlaySound(ISampleProvider sample, int channel)
        {
            m_playingSamples.Add(new PlayingSample(sample, channel, m_currentTick));
        }
        public void FadeOut(int channel)
        {
            foreach(PlayingSample sample in m_playingSamples)
            {
                if (sample.channel == channel && m_currentTick < sample.fadeOutStart)
                {
                    sample.fadeOutStart = m_currentTick;
                }
            }
        }
        public void StopSound(int channel)
        {
            for(int i = m_playingSamples.Count - 1; i >= 0; i--)
            {
                if(m_playingSamples[i].channel == channel)
                {
                    m_playingSamples.RemoveAt(i);
                }
            }
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);
            m_fadeOutTicks = (int) (WaveFormat.SampleRate * FadeOutLength);
        }

        public virtual void OnSampleRead(int sampleIndex)
        {

        }
    }
}
