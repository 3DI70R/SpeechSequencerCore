using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("AudioAlias")]
    public class AudioAlias : Alias
    {
        public override IAliasEntryNode CreateNode(IPlaybackContext context)
        {
            return new AudioAliasNode(NodeFactory.Instance.CreateChildrenAsSingleAudio(XmlData, context));
        }
    }
}
