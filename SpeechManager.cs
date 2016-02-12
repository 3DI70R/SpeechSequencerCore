using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class SpeechManager
    {
        private static SpeechManager s_instance;

        public static SpeechManager Instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new SpeechManager();
                }

                return s_instance;
            }
        }

        private SpeechManager()
        {

        }

        public SpeechSynthesizer GetVoice(string voiceName)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoice("IVONA 2 Maxim OEM");
            return synth;
        }
    }
}
