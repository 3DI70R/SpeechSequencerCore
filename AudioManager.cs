using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class AudioManager
    {
        private static AudioManager s_instance;

        private WaveOut m_waveOut;
        private WaveFormat m_waveFormat;
        private IAudioNode m_initedNode;

        public static AudioManager Instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new AudioManager();
                }

                return s_instance;
            }
        }

        public WaveFormat ResampleAudioFormat
        {
            get
            {
                return m_waveFormat;
            }
        }

        private AudioManager()
        {
            m_waveOut = new WaveOut();
            m_waveOut.NumberOfBuffers = 4;
            //m_waveOut.DeviceNumber = 3;
            m_waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44000, 2);
        }

        public AudioManager Init(IAudioNode node)
        {
            m_waveOut.Stop();
            m_waveOut.Init(node);
            m_initedNode = node;

            return this;
        }

        public void Play()
        {
            m_waveOut.Play();
        }

        public void Pause()
        {
            if(m_waveOut.PlaybackState == PlaybackState.Playing)
            {
                m_waveOut.Pause();
            }
        }
        public void Resume()
        {
            if(m_waveOut.PlaybackState == PlaybackState.Paused)
            {
                m_waveOut.Resume();
            }
        }
        public void TogglePause()
        {
            if (m_waveOut.PlaybackState == PlaybackState.Playing)
            {
                m_waveOut.Pause();
            }
            else if(m_waveOut.PlaybackState == PlaybackState.Paused)
            {
                m_waveOut.Resume();
            }
        }

        public string[] GetAvailableAudioDevices()
        {
            List<string> devices = new List<string>();

            for(int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities caps = WaveOut.GetCapabilities(i);
                devices.Add(caps.ProductName);
            }

            return devices.ToArray();
        }

        public int GetAudioDevice()
        {
            return m_waveOut.DeviceNumber;
        }
        public void SetAudioDevice(int index)
        {
            m_waveOut.DeviceNumber = index;
        }
    }
}
