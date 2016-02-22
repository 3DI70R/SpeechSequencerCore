using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Variable")]
    [Description("Нода грузящая вместо себя определённую переменную")]
    public class VariableNode : SequenceNode, IAudioNode, IValueNode
    {
        public enum VariableType
        {
            Audio,
            Value,
        }

        private IValueNode m_valueNode;
        private IAudioNode m_audioNode;

        [XmlAttributeBinding]
        [Description("Имя переменной, значение которой нужно загрузить")]
        public string Name { get; set; } = "";

        [XmlAttributeBinding]
        [Description("Тип переменной")]
        public VariableType Type { get; set; } = VariableType.Value;

        public string Value
        {
            get
            {
                return m_valueNode.Value;
            }
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return m_audioNode.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return m_audioNode.Read(buffer, offset, count);
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);

            ISequenceNode node = context.GetVariableNode(Name);
 
            if(Type == VariableType.Audio)
            {
                m_audioNode = node.ToAudio();
                m_audioNode.InitNewState(context);
            }
            else
            {
                m_valueNode = (IValueNode) node;
                m_valueNode.InitNewState(context);
            }
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
