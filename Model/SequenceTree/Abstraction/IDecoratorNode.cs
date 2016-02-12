using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface IDecoratorNode : ISequenceNode, IAudioNode
    {
        void InitDecorator(IPlaybackContext context);

        IAudioNode DecoratedNode { get; set; }
        bool IsRedundant { get; }
    }
}
