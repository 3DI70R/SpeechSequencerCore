using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class DelayDecorator : AudioDecoratorNode
    {
        private OffsetSampleProvider m_offsetSample;

        [XmlAttributeBinding]
        [Description("Задержка перед началом воспроизведения")]
        public double Delay { get; set; } = 0;

        [XmlAttributeBinding]
        [Description("Сколько секунд необходимо пропустить перед началом воспроизведения")]
        public double Skip { get; set; } = 0;

        [XmlAttributeBinding]
        [Description("Сколько секунд тишины необходимо вставить после завершения воспроизведения")]
        public double LeadOut { get; set; } = 0;

        public override WaveFormat WaveFormat
        {
            get
            {
                return m_offsetSample.WaveFormat;
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            return m_offsetSample.Read(buffer, offset, count);
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);

            m_offsetSample = new OffsetSampleProvider(DecoratedNode);
            m_offsetSample.DelayBy = TimeSpan.FromSeconds(Delay);
            m_offsetSample.SkipOver = TimeSpan.FromSeconds(Skip);
            m_offsetSample.LeadOut = TimeSpan.FromSeconds(LeadOut);
        }
    }
}
