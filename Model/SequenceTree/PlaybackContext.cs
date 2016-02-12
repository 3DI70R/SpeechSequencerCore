using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class PlaybackContext : IPlaybackContext
    {
        private Random m_contextRandom = new Random();
        private Dictionary<string, Func<ISequenceNode>> m_variables = new Dictionary<string, Func<ISequenceNode>>();

        public virtual Random SharedRandom
        {
            get
            {
                return m_contextRandom;
            }
        }

        public virtual ISequenceNode GetVariableNode(string varName)
        {
            return m_variables[varName]();
        }
        public virtual void SetVariableCreator(string varName, Func<ISequenceNode> creator)
        {
            m_variables[varName] = creator;
        }

        public virtual void SetStringValue(string varName, string value)
        {
            m_variables[varName] = () => new TextValueNode(value);
        }

        public IPlaybackContext Clone()
        {
            PlaybackContext context = new PlaybackContext();
            context.m_contextRandom = new Random(m_contextRandom.Next());
            context.m_variables = new Dictionary<string, Func<ISequenceNode>>(m_variables);
            return context;
        }
    }
}
