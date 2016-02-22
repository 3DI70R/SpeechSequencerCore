using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Speak")]
    [Description("Проговаривает текст указанный как значение")]
    public class TTSNode : ValuePlaybackNode
    {
        private SpeechSynthesizer m_synth;

        [XmlAttributeBinding]
        public string Voice { get; set; } = string.Empty;

        public TTSNode()
        {
            m_synth = new SpeechSynthesizer();
        }

        public virtual string GetTextToSpeak()
        {
            return ValueHolder.Value;
        }

        protected override ISampleProvider CreateProvider(string value, IPlaybackContext context)
        {
            return SpeechManager.Instance.SpeakText(value, Voice);
        }
    }
}
