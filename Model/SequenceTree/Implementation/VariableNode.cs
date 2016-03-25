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
            Auto
        }

        private IValueNode m_valueNode;
        private IAudioNode m_audioNode;
        private ISequenceNode m_sequence;

        [XmlAttributeBinding]
        [Description("Имя переменной, значение которой нужно загрузить")]
        public string Name { get; set; } = "";

        [XmlAttributeBinding]
        [Description("Тип переменной")]
        public VariableType Type { get; set; } = VariableType.Auto;

        public string Value
        {
            get
            {
                if(Type == VariableType.Auto && m_valueNode == null)
                {
                    m_valueNode = (IValueNode) m_sequence;
                    m_valueNode.InitNewState(Context);
                }

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
            if(Type == VariableType.Auto && m_audioNode == null)
            {
                m_audioNode = m_sequence.ToAudio();
                m_audioNode.InitNewState(Context);
            }

            return m_audioNode.Read(buffer, offset, count);
        }

        public override void InitNewState(Context context)
        {
            base.InitNewState(context);

            m_sequence = context.GetVariable(Name);
 
            if(Type == VariableType.Audio)
            {
                m_audioNode = m_sequence.ToAudio();
                m_audioNode.InitNewState(context);
            }
            else if(Type == VariableType.Value)
            {
                m_valueNode = (IValueNode) m_sequence;
                m_valueNode.InitNewState(context);
            }
        }

        public override IAudioNode ToAudio()
        {
            return this;
        }
    }
}
