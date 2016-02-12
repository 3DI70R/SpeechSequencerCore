using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface IPlaybackContext
    {
        Random SharedRandom { get; }

        ISequenceNode GetVariableNode(string variableName);
        void SetVariableCreator(string variableName, Func<ISequenceNode> nodeCreator);

        IPlaybackContext Clone();
    }
}
