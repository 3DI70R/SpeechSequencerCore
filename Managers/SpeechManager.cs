using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class SpeechManager
    {
        private static SpeechManager s_instance = new SpeechManager();
        public static SpeechManager Instance { get { return s_instance; } }

        private Dictionary<string, ITTSEngine> m_ttsCollection;
        private ITTSEngine m_defaultTts;

        private SpeechManager()
        {
            m_ttsCollection = new Dictionary<string, ITTSEngine>();

            InitSapiVoices();
        }

        public ITTSEngine GetEngineByName(string name)
        {
            ITTSEngine tts;

            if(m_ttsCollection.TryGetValue(name, out tts))
            {
                return tts;
            }
            else
            {
                return m_defaultTts;
            }
        }

        public ISampleProvider SpeakText(string text, string voice)
        {
            return GetEngineByName(voice).SpeakText(text);
        }

        private void InitSapiVoices()
        {
            string[] voices = SapiTTS.GetAllVoices();

            foreach (string voice in voices)
            {
                try
                {
                    SapiTTS sapi = new SapiTTS(voice);
                    sapi.VoiceName = voice;

                    m_ttsCollection[voice] = sapi;

                    if (m_defaultTts == null)
                    {
                        m_defaultTts = sapi;
                    }
                }
                catch (Exception e)
                {
                    
                }
            }

            m_defaultTts = m_ttsCollection["RHVoice Aleksandr (Russian)"];
        }
    }
}
