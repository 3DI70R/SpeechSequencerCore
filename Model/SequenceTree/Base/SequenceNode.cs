﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class SequenceNode : ISequenceNode
    {
        public XmlElement XmlData { get; set; }
        public IPlaybackContext Context { get; set; }

        public abstract IAudioNode ToAudio();

        public virtual void InitNewState(IPlaybackContext context)
        {
            Context = context;

            if (XmlData != null)
            {
                LoadDataFromXml(XmlData, context);
            }
        }

        protected virtual void LoadDataFromXml(XmlElement element, IPlaybackContext context)
        {
            XmlBinder.BindValues(this, element, context);
        }

        public virtual void Dispose() { }
    }
}
