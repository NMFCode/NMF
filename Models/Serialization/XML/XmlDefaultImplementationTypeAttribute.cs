using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    [Obsolete]
    [AttributeUsage(AttributeTargets.Interface, Inherited=false)]
    public class XmlDefaultImplementationTypeAttribute : Attribute
    {
        public Type DefaultImplementationType { get; private set; }

        public XmlDefaultImplementationTypeAttribute(Type implementationType)
        {
            DefaultImplementationType = implementationType;
        }
    }
}
