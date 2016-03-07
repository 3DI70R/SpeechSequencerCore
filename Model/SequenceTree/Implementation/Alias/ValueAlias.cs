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
        public override IAliasEntryNode CreateNode(IPlaybackContext context)
        {
            return new ValueAliasNode(ObjectFactory.Instance.CreateChildrenAsSingleValue(XmlData));
        }
    }
}
