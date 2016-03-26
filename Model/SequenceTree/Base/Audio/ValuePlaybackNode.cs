using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class ValuePlaybackNode : AudioNode
    {
        private ISampleProvider m_provider;

        public IValueNode ValueHolder { get; set; } = new TextValueNode("");

        public override WaveFormat WaveFormat
        {
            get
            {
                return m_provider.WaveFormat;
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            return m_provider.Read(buffer, offset, count);
        }

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            ValueHolder.InitNewState(LocalContext);
            m_provider = CreateProvider(ValueHolder.Value, context);
        }
        protected override void LoadDataFromXml(XmlElement element, Context context)
        {
            base.LoadDataFromXml(element, context);
            ValueHolder = (IValueNode) SequenceFactory.Instance.CreateChildrenAsSequence(element, context);
        }

        protected abstract ISampleProvider CreateProvider(string value, Context context); 
    }
}
