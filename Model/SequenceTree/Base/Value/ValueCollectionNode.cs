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

        protected override void LoadChildsFromXml(XmlElement element, Context context)
        {
            SequenceFactory.Instance.EnumerateChildren(element, (e) =>
            {
                AddNode(
                    (IValueNode) SequenceFactory.Instance.CreateSequence(e, context),
                    CreateParametersFromXml(e, context));
            });
        }

        public override IAudioNode ToAudio()
        {
            return ValueUtils.WrapValueAsSpeech(this);
        }
    }
}
