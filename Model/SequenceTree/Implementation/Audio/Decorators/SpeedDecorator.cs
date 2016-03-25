using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class SpeedDecorator : AudioDecoratorNode
    {
        [XmlAttributeBinding]
        [Description("Скорость воспроизведения")]
        public float Speed { get; set;  } = 1;

        public override WaveFormat WaveFormat
        {
            get
            {
                WaveFormat origFormat = base.WaveFormat;
                return WaveFormat.CreateIeeeFloatWaveFormat((int) (origFormat.SampleRate * Speed), origFormat.Channels);
            }
        }
        public override int Read(float[] buffer, int offset, int count)
        {
            return DecoratedNode.Read(buffer, offset, count);
        }
    }
}
