using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Serialization
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class XmlOppositeAttribute : Attribute
    {
        public XmlOppositeAttribute(string oppositeProperty)
        {
            OppositeType = null;
            OppositeProperty = oppositeProperty;
        }

        public XmlOppositeAttribute(Type oppositeType, string oppositeProperty)
        {
            OppositeType = oppositeType;
            OppositeProperty = oppositeProperty;
        }

        public Type OppositeType { get; private set; }

        public string OppositeProperty { get; private set; }
    }
}
