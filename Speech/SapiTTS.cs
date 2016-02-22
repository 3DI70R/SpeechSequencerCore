using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Speech.Synthesis;
using System.IO;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("Sapi")]
    public class SapiTTS : ITTSEngine
    {
        private string m_name;
        private SpeechSynthesizer m_synth;

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public string VoiceName
        {
            get
            {
                return m_synth.Voice.Name;
            }
            set
            {
                m_synth.SelectVoice(value);
            }
        }

        public int Rate
        {
            get
            {
                return m_synth.Rate;
            }
            set
            {
                m_synth.Rate = value;
            }
        }

        public SapiTTS(string name)
        {
            m_name = name;
            m_synth = new SpeechSynthesizer();
        }

        public ISampleProvider SpeakText(string text)
        {
            MemoryStream stream = new MemoryStream();

            m_synth.SetOutputToWaveStream(stream);
            m_synth.Speak(text);

            stream.Position = 0;

            return new WaveFileReader(stream).ToSampleProvider();
        }

        public static string[] GetAllVoices()
        {
            List<string> voices = new List<string>();
            SpeechSynthesizer synth = new SpeechSynthesizer();

            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                voices.Add(voice.VoiceInfo.Name);
            }

            return voices.ToArray();
        }
    }
}
