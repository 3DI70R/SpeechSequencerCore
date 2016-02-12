﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class AttributeProperty
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

        public AttributeProperty(PropertyInfo property, string attributeName, Func<string, object> converter)
        {
            m_propertyInfo = property;
            m_attributeName = attributeName;
            m_converter = converter;
        }
    }
}
