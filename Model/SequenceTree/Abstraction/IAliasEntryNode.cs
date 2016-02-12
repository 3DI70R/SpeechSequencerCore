using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface IAliasEntryNode : ISequenceNode
    {
        void OverrideVariableCreator(string varName, Func<ISequenceNode> creator);
    }
}
