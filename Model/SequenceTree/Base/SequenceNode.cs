using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class SequenceNode : ISequenceNode
    {
        private Dictionary<string, Func<ISequenceNode>> m_variables = new Dictionary<string, Func<ISequenceNode>>();

        public XmlElement XmlData { get; set; }
        public Context LocalContext { get; set; }

        public abstract IAudioNode ToAudio();

        public void InitNewState(Context context)
        {
            LocalContext = new Context(context);

            if(m_variables != null)
            {
                foreach (KeyValuePair<string, Func<ISequenceNode>> variable in m_variables)
                {
                    LocalContext.SetVariableFactory(variable.Key, variable.Value);
                }
            }

            ValueBinder.BindValues(this, XmlData, LocalContext);

            if (XmlData != null)
            {
                LoadDataFromXml(XmlData, LocalContext);
            }

            OnInitNewState(LocalContext);
        }

        protected virtual void OnInitNewState(Context context) { }
        protected virtual void LoadDataFromXml(XmlElement element, Context context) {  }

        public virtual void Dispose() { }

        public void OverrideVariable(string name, Func<ISequenceNode> variable)
        {
            m_variables[name] = variable;
        }
    }
}
