using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class XmlKnownTypeAttribute : Attribute
    {
        public Type Type { get; private set; }

        public XmlKnownTypeAttribute(Type type) { Type = type; }
    }
}
