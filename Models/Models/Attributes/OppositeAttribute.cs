using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OppositeAttribute : Attribute
    {
        public PropertyInfo Property { get; private set; }

        public OppositeAttribute(Type oppositeType, string propertyName)
        {
            if (oppositeType == null) throw new ArgumentNullException("oppositeType");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Property = oppositeType.GetProperty(propertyName);
        }
    }
}
