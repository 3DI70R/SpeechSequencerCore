using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AliasEntryNode<T> : SequenceNode, IAliasEntryNode where T : ISequenceNode
    {
        private T m_wrappedNode;
        private Dictionary<string, Func<ISequenceNode>> m_variables = new Dictionary<string, Func<ISequenceNode>>();

        public T WrappedNode
        {
            get
            {
                return m_wrappedNode;
            }
        }

        public AliasEntryNode(T node)
        {
            m_wrappedNode = node;
        }

        public void OverrideVariableCreator(string varName, Func<ISequenceNode> creator)
        {
            m_variables[varName] = creator;
        }

        public override void InitNewState(IPlaybackContext context)
        {
            IPlaybackContext cloned = context.Clone();

            foreach(KeyValuePair<string, Func<ISequenceNode>> pair in m_variables)
            {
                cloned.SetVariableCreator(pair.Key, pair.Value);
            }

            base.InitNewState(cloned);
            m_wrappedNode.InitNewState(cloned);
        }
    }
}
