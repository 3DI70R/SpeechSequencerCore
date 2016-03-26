using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Parallel")]
    [Description("Воспроизводит лежащие в нём звуки параллельно")]
    public class ParallelAudioNode : AudioCollectionNode<ParallelAudioNode.Extra>
    {
        public class Extra
        {
            [XmlAttributeBinding]
            [Description("Не ожидать завершения воспроизведения")]
            public bool DontWait { get; set; } = false;
        }

        private float[] m_buffer;
        private List<ISampleProvider> m_resampledItems;

        public ParallelAudioNode()
        {
            m_resampledItems = new List<ISampleProvider>();
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int maxReaded = 0;
            m_buffer = BufferHelpers.Ensure(m_buffer, count);

            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = 0;
            }

            for (int i = 0; i < m_resampledItems.Count; i++)
            {
                ISampleProvider provider = m_resampledItems[i];
                int actualyReaded = provider.Read(m_buffer, 0, count);
                int position = offset;

                for (int j = 0; j < actualyReaded; j++)
                {
                    buffer[position++] += m_buffer[j];
                }

                if (!GetNodeParamsAt(i).DontWait)
                {
                    maxReaded = Math.Max(maxReaded, actualyReaded);
                }
            }
            
            return maxReaded;
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);

            m_resampledItems.Clear();
            for (int i = 0; i < ChildCount; i++)
            {
                IAudioNode node = GetNodeAt(i);
                node.InitNewState(context);

                m_resampledItems.Add(node.ResampleIfNeeded(WaveFormat));
            }
        }
    }
}
