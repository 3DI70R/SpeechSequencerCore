﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class XmlAttributeBinding : VariableElementBinding
    {
        public XmlAttributeBinding() { }
        public XmlAttributeBinding(string name) : base(name) { }

        public override AbstractAttributeField CreateInstance(PropertyInfo property, string attributeName, Func<string, object> converter)
        {
            return new XmlAttributeField(property, attributeName, converter);
        }
    }
}
