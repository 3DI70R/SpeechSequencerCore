﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class LoopDecorator : AudioDecoratorNode
    {
        protected int m_currentLoop;

        [XmlAttributeBinding("Loop")]
        [Description("Количество повторений (0 = бесконечно)")]
        public int Count { get; set; } = 1;

        public override int Read(float[] buffer, int offset, int count)
        {
            int readed = 0;

            while(readed < count && (m_currentLoop < Count || Count == 0))
            {
                readed += DecoratedNode.Read(buffer, offset, count - readed);

                if(readed < count)
                {
                    DecoratedNode.InitNewState(Context);
                    m_currentLoop++;
                }
            }

            return readed;
        }

        public override bool IsRedundant
        {
            get
            {
                return Count == 1;
            }
        }

        public override void InitNewState(IPlaybackContext context)
        {
            base.InitNewState(context);
            m_currentLoop = 0;
        }
    }
}