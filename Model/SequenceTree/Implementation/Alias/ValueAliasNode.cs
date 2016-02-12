using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ValueAliasNode : AliasEntryNode<IValueNode>, IValueNode
    {
        public string Value
        {
            get
            {
                return WrappedNode.Value;
            }
        }

        public ValueAliasNode(IValueNode node) : base(node) { }
    }
}
