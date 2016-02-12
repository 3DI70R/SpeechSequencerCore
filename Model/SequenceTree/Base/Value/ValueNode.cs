using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class ValueNode : SequenceNode, IValueNode
    {
        private string m_value;

        public virtual string Value { get { return m_value; } }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);

            try
            {
                m_value = LoadValue(context);
            }
            catch (Exception e)
            {
                m_value = "Произошла ошибка: " + e.Message;
            }
        }

        public abstract string LoadValue(IPlaybackContext context);
    }
}
