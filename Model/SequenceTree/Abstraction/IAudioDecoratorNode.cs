using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface IAudioDecoratorNode : ISequenceNode, IAudioNode
    {
        IAudioNode DecoratedNode { get; set; }
    }
}
