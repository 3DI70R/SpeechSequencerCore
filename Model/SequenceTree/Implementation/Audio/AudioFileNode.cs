using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Audio")]
    [Description("Воспроизводит аудио файл указанный как значение")]
    public class AudioFileNode : ValuePlaybackNode
    {
        public AudioFileNode() { }
        public AudioFileNode(string filePath)
        {
            ValueHolder = new TextValueNode(filePath);
        }

        protected override ISampleProvider CreateProvider(string value, Context context)
        {
            return new AudioFileReader(ValueHolder.Value);
        }
    }
}
