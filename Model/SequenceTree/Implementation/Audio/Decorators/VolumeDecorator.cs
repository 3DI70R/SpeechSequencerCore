﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class VolumeDecorator : AudioDecoratorNode
    {
        private VolumeSampleProvider m_volumeProvider;

        [XmlAttributeBinding]
        [Description("Громкость звука")]
        public float Volume { get; set; } = 1;

        public override int Read(float[] buffer, int offset, int count)
        {
            return m_volumeProvider.Read(buffer, offset, count);
        }
        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            m_volumeProvider = new VolumeSampleProvider(DecoratedNode);
            m_volumeProvider.Volume = Volume;
        }
    }
}
