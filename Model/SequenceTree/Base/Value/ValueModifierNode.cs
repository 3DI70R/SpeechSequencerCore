using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class ValueModifierNode : ValueNode
    {
        private string m_value;

        public IValueNode ChildValue { get; set; } = new TextValueNode("");

        public override string LoadValue(IPlaybackContext context)
        {
            ChildValue.InitNewState(context);
            return ProcessValue(ChildValue.Value);
        }
        protected override void LoadDataFromXml(XmlElement element, IPlaybackContext context)
        {
            ChildValue = NodeFactory.Instance.CreateChildrenAsSingleValue(element);
        }

        public abstract string ProcessValue(string value);
    }
}
