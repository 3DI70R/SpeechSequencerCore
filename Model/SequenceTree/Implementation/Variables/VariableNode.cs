using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class VariableNode : SequenceNode
    {
        protected ISequenceNode m_sequence;

        [XmlAttributeBinding]
        [Description("Имя переменной, значение которой нужно загрузить")]
        public string Name { get; set; } = "";

        protected override void OnInitNewState(Context context)
        {
            base.OnInitNewState(context);
            m_sequence = context.GetVariable(Name);
        }
    }
}
