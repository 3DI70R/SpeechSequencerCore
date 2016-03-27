using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class XmlAttributeField : AbstractAttributeField
    {
        public XmlAttributeField(PropertyInfo property, string attributeName, Func<string, object> converter) : base(property, attributeName, converter) { }

        public override string GetValueFromElement(XmlElement element, Context context)
        {
            if (element.HasAttribute(Name, string.Empty))
            {
                return element.GetAttribute(Name);
            }
            else if (element.HasAttribute(Name, ResourceManager.VariableNamespace))
            {
                IValueNode value = (IValueNode) context.GetVariable(element.GetAttribute(Name, ResourceManager.VariableNamespace));
                value.InitNewState(context);
                return value.Value;
            }
            else
            {
                XmlElement valueNode = element[element.Name + "." + Name];

                if (valueNode != null)
                {
                    IValueNode value = (IValueNode) SequenceFactory.Instance.CreateChildrenAsSequence(valueNode, context);
                    value.InitNewState(context);
                    return value.Value;
                }
            }

            return null;
        }
    }
}
