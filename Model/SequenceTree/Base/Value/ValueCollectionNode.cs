using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class ValueCollectionNode<P> : CollectionNode<IValueNode, P>, IValueNode where P : new()
    {
        public abstract string Value { get; }

        protected override void LoadChildsFromXml(XmlElement element, IPlaybackContext context)
        {
            ObjectFactory.Instance.EnumerateChildValues(element, (e) =>
            {
                AddNode(
                    ObjectFactory.Instance.CreateValueNode(e),
                    CreateParametersFromXml(e, context));
            });
        }

        public override IAudioNode ToAudio()
        {
            return ValueUtils.WrapValueAsSpeech(this);
        }
    }
}
