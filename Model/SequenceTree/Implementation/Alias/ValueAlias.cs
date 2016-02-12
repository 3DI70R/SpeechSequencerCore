using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("ValueAlias")]
    public class ValueAlias : Alias
    {
        public override IAliasEntryNode CreateNode()
        {
            return new ValueAliasNode(NodeFactory.Instance.CreateChildrenAsSingleValue(XmlData));
        }
    }
}
