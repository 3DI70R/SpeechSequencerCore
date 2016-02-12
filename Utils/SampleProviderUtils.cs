using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public static class SampleProviderUtils
    {
        public static ISampleProvider[] ResampleChilds(IEnumerable<ISampleProvider> nodes, WaveFormat format)
        {
            List<ISampleProvider> providers = new List<ISampleProvider>();

            foreach(ISampleProvider sample in nodes)
            {
                providers.Add(sample);
            }

            return providers.ToArray();
        }
        public static ISampleProvider ResampleIfNeeded(this ISampleProvider node, WaveFormat format)
        {
            if (!node.WaveFormat.Equals(format))
            {
                ISampleProvider provider = node;

                if (node.WaveFormat.Channels != format.Channels)
                {
                    if (node.WaveFormat.Channels == 1 && format.Channels == 2)
                    {
                        provider = provider.ToStereo();
                    }
                    else if (node.WaveFormat.Channels == 2 && format.Channels == 1)
                    {
                        provider = provider.ToMono();
                    }
                    else
                    {
                        throw new ArgumentException("Cannot change channel count from " + node.WaveFormat.Channels + " to " + format.Channels);
                    }
                }

                return new WdlResamplingSampleProvider(provider, format.SampleRate);
            }
            else
            {
                return node;
            }
        }

        public static void ToAudioFile(this ISampleProvider provider, string audioFileName)
        {
            IWaveProvider wave = provider.ToWaveProvider();
            WaveFileWriter writer = new WaveFileWriter(audioFileName, provider.WaveFormat);

            int readed;
            byte[] buffer = new byte[65536];
            while ((readed = wave.Read(buffer, 0, buffer.Length)) != 0)
            {
                writer.Write(buffer, 0, readed);
            }
            writer.Flush();
        }
    }
}