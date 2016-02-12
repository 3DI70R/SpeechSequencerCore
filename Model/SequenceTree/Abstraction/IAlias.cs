using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface IAlias
    {
        string Name { get; }
        XmlElement XmlData { get; }
        bool IsAudioAlias { get; }
        int ArgumentCount { get; }
        string GetAliasArgumentName(int index);
        Func<ISequenceNode> GetDefaultArgumentValue(int index);

        IAliasEntryNode CreateNode();
        void InitFromXml(XmlElement element);
    }
}
