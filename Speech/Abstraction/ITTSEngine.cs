using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public interface ITTSEngine
    {
        string Name { get; }

        ISampleProvider SpeakText(string text);
    }
}
