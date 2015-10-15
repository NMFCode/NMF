using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Defines an attribute to mark a property being used for the constructor of an object
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class XmlConstructorParameterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new XmlConstructorParameterAttribute to mark a property being used for a constructor
        /// </summary>
        /// <param name="index"></param>
        public XmlConstructorParameterAttribute(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Gets the index, the property is used in the constructor
        /// </summary>
        public int Index
        {
            get;
            private set;
        }
    }
}
