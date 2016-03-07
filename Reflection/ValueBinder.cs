using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public static class ValueBinder
    {
        private static Dictionary<Type, AbstractAttributeField[]> s_fieldsCache = new Dictionary<Type, AbstractAttributeField[]>();

        public static AbstractAttributeField[] GetAttributeProperties(this Type type)
        {
            AbstractAttributeField[] result;

            if(!s_fieldsCache.TryGetValue(type, out result))
            {
                List<AbstractAttributeField> resultList = new List<AbstractAttributeField>();

                foreach(PropertyInfo property in type.GetProperties())
                {
                    foreach (VariableElementBinding attribute in property.GetCustomAttributes<VariableElementBinding>())
                    {
                        resultList.Add(attribute.CreateInstance(property, attribute.Name ?? property.Name, GetConverter(property.PropertyType)));
                    }
                }

                result = resultList.ToArray();
            }

            return result;
        }
        public static Func<string, object> GetConverter(Type valueType)
        {
            if(valueType.Equals(typeof(string)))
            {
                return (s) => s;
            }
            if(valueType.Equals(typeof(int)))
            {
                return (s) => int.Parse(s);
            }
            else if(valueType.Equals(typeof(float)))
            {
                return (s) => float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
            }
            else if (valueType.Equals(typeof(double)))
            {
                return (s) => double.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
            }
            else if(valueType.Equals(typeof(bool)))
            {
                return (s) => "true".Equals(s);
            }
            else if(valueType.IsEnum)
            {
                return (s) => Enum.Parse(valueType, s);
            }

            throw new NotImplementedException("Cannot create converter for " + valueType);
        }

        public static void BindValues(object instance, XmlElement element, IPlaybackContext context)
        {
            foreach(AbstractAttributeField property in instance.GetType().GetAttributeProperties())
            {
                string value = property.GetValueFromElement(element, context);

                if(value != null)
                {
                    property.SetValue(instance, value);
                }
            }
        }
    }
}
