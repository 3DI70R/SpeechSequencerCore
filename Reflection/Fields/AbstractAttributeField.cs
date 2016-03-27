using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public abstract class AbstractAttributeField
    {
        private readonly string m_attributeName;
        private readonly PropertyInfo m_propertyInfo;
        private readonly Func<string, object> m_converter;

        public string Name
        {
            get
            {
                return m_attributeName;
            }
        }

        public void SetValue(object instance, string value)
        {
            m_propertyInfo.SetValue(instance, m_converter(value));
        }

        public abstract string GetValue(XmlElement element, Context context);

        public AbstractAttributeField(PropertyInfo property, string attributeName, Func<string, object> converter)
        {
            m_propertyInfo = property;
            m_attributeName = attributeName;
            m_converter = converter;
        }
    }
}
