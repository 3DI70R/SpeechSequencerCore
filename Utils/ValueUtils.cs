using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public static class ValueUtils
    {
        public delegate string LoadNextValue(int index);

        public static string JoinValues(string divider, int count, LoadNextValue loadValueDelegate)
        {
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < count; i++)
            {
                string value = loadValueDelegate(i);

                if(!string.IsNullOrWhiteSpace(value))
                {
                    if (builder.Length != 0)
                    {
                        builder.Append(divider);
                    }

                    builder.Append(value);
                }
            }

            return builder.ToString();
        }
    }
}
