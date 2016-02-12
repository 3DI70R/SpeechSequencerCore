using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Sequential")]
    public class SequentialAudioNode : AudioCollectionNode<object>
    {
        protected int m_nextNodeIndex;
        protected IAudioNode m_currentNode;
        protected ISampleProvider m_resampledCurrentNode;

        [XmlAttributeBinding]
        [Description("Откладывает инициализацию до воспроизведения")]
        public bool DeferredInit { get; set; }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = 0;

            while (read < count && m_resampledCurrentNode != null)
            {
                int readed = m_resampledCurrentNode.Read(buffer, read, count - read);
                read += readed;

                if (readed == 0)
                {
                    LoadNextNode();
                }
            }

            return read;
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);
            m_nextNodeIndex = 0;

            if(!DeferredInit)
            {
                InitAllNodes(context);
            }

            LoadNextNode();
        }
        protected void InitAllNodes(IPlaybackContext context)
        {
            for(int i = 0; i < ChildCount; i++)
            {
                GetNodeAt(i).InitNewState(context);
            }
        }
        protected void LoadNextNode()
        {
            if(m_nextNodeIndex < ChildCount)
            {
                m_currentNode = GetNodeAt(m_nextNodeIndex++);

                if (DeferredInit)
                {
                    m_currentNode.InitNewState(Context);
                }

                m_resampledCurrentNode = m_currentNode.ResampleIfNeeded(WaveFormat);
            }
            else
            {
                m_currentNode = null;
                m_resampledCurrentNode = null;
            }
        }
    }
}
