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
    public static class XmlBinder
    {
        public const string c_varNamespace = "Variable";

        private static Dictionary<Type, AttributeProperty[]> s_fieldsCache = new Dictionary<Type, AttributeProperty[]>();

        public static AttributeProperty[] GetAttributeProperties(this Type type)
        {
            AttributeProperty[] result;

            if(!s_fieldsCache.TryGetValue(type, out result))
            {
                List<AttributeProperty> resultList = new List<AttributeProperty>();

                foreach(PropertyInfo property in type.GetProperties())
                {
                    XmlAttributeBinding attribute = property.GetCustomAttribute<XmlAttributeBinding>();

                    if(attribute != null)
                    {
                        resultList.Add(new AttributeProperty(property, attribute.Name ?? property.Name, GetConverter(property.PropertyType)));
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

        public static string GetAttributeValue(this XmlElement element, string attrName, IPlaybackContext context)
        {
            if (element.HasAttribute(attrName, string.Empty))
            {
                return element.GetAttribute(attrName);
            }
            else if (element.HasAttribute(attrName, c_varNamespace))
            {
                IValueNode value = (IValueNode) context.GetVariableNode(element.GetAttribute(attrName, c_varNamespace));
                value.InitNewState(context);
                return value.Value;
            }
            else
            {
                XmlElement valueNode = element[element.Name + "." + attrName];

                if(valueNode != null)
                {
                    IValueNode value = NodeFactory.Instance.CreateChildrenAsSingleValue(valueNode);
                    value.InitNewState(context);
                    return value.Value;
                }
            }

            return null;
        }

        public static void BindValues(object instance, XmlElement element, IPlaybackContext context)
        {
            foreach(AttributeProperty property in instance.GetType().GetAttributeProperties())
            {
                string value = element.GetAttributeValue(property.Name, context);

                if(value != null)
                {
                    property.SetValue(instance, value);
                }
            }
        }
    }
}
