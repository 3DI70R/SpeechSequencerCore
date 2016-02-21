using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NAudio.Wave;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AudioCollectionNode<P> : SequenceCollectionNode<IAudioNode, P>, IAudioNode where P : new()
    {
        public virtual WaveFormat WaveFormat
        {
            get
            {
                return AudioManager.Instance.ResampleAudioFormat;
            }
        }

        public abstract int Read(float[] buffer, int offset, int count);

        protected override void LoadChildsFromXml(XmlElement element, IPlaybackContext context)
        {
            NodeFactory.Instance.EnumerateChildAudio(element, (e) =>
            {
                AddNode(
                    NodeFactory.Instance.CreateAudioNode(e, context),
                    CreateParametersFromXml(e, context));
            });
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
