using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class SequenceCollectionNode<N, P> : SequenceNode
        where N : ISequenceNode
        where P : new ()
    {
        private struct NodeInfo
        {
            public N Node;
            public P Params;
        }

        private List<NodeInfo> m_nodes = new List<NodeInfo>();

        public int ChildCount
        {
            get
            {
                return m_nodes.Count;
            }
        }

        public void AddNode(N node)
        {
            AddNode(node, default(P));
        }
        public virtual void AddNode(N node, P parameters)
        {
            if(node != null && !IsNodeExists(node))
            {
                m_nodes.Add(new NodeInfo()
                {
                    Node = node,
                    Params = parameters
                });
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public N GetNodeAt(int index)
        {
            return m_nodes[index].Node;
        }
        public P GetNodeParamsAt(int index)
        {
            return m_nodes[index].Params;
        }
        public void RemoveNodeAt(int index)
        {
            m_nodes.RemoveAt(index);
        }

        protected bool IsNodeExists(N node)
        {
            foreach (NodeInfo info in m_nodes)
            {
                if (info.Node.Equals(node))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void LoadDataFromXml(XmlElement element, IPlaybackContext context)
        {
            base.LoadDataFromXml(element, context);
            m_nodes.Clear();
            LoadChildsFromXml(element, context);
        }
        protected abstract void LoadChildsFromXml(XmlElement element, IPlaybackContext context);

        protected virtual P CreateParametersFromXml(XmlElement element, IPlaybackContext context)
        {
            P parameters = new P();
            XmlBinder.BindValues(parameters, element, context);
            return parameters;
        }
    }
}
