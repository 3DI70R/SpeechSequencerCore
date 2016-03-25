using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class MultiUseAudioCollectionNode<P> : CollectionNode<Func<IAudioNode>, P>, IAudioNode where P : new()
    {
        public WaveFormat WaveFormat
        {
            get
            {
                return AudioManager.Instance.ResampleAudioFormat;
            }
        }

        public abstract int Read(float[] buffer, int offset, int count);

        public override IAudioNode ToAudio()
        {
            return this;
        }

        protected override void LoadChildsFromXml(XmlElement element, Context context)
        {
            SequenceFactory.Instance.EnumerateChildren(element, (e) =>
            {
                AddNode(() =>
                {
                    return SequenceFactory.Instance.CreateSequence(e, context).ToAudio();
                },
                CreateParametersFromXml(e, context));
            });
        }
    }
}
