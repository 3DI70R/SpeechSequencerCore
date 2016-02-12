using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface ISequenceNode : IDisposable
    {
        XmlElement XmlData { get; set; }

        void InitNewState(IPlaybackContext context);
    }
}
