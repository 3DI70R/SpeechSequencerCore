using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class Settings
    {
        private static Settings s_instance = new Settings();
        public static Settings Instance { get { return s_instance; } }

        private Settings()
        {
            
        }
    }
}
