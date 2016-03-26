using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class Context
    {
        private Random m_contextRandom;
        private Dictionary<string, Func<ISequenceNode>> m_variables;

        public virtual Random SharedRandom
        {
            get
            {
                return m_contextRandom;
            }
        }

        public Context()
        {
            m_contextRandom = new Random();
            m_variables = new Dictionary<string, Func<ISequenceNode>>();
        }
        public Context(Context context)
        {
            m_contextRandom = context.m_contextRandom;
            m_variables = new Dictionary<string, Func<ISequenceNode>>(context.m_variables);
        }

        public virtual ISequenceNode GetVariable(string varName)
        {
            return m_variables[varName]();
        }
        public virtual void SetVariableFactory(string varName, Func<ISequenceNode> creator)
        {
            m_variables[varName] = creator;
        }
    }
}
