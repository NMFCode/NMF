using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to override a TypeConverter for Xml serialization
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class XmlTypeConverterAttribute : Attribute
    {
        private readonly Type type;

        /// <summary>
        /// Creates a new XmlTypeConverterAttribute using the provided type
        /// </summary>
        /// <param name="type">The type to use as TypeConverter</param>
        public XmlTypeConverterAttribute(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// The converter type to use in serialization
        /// </summary>
        public Type Type
        {
            get
            {
                return type;
            }
        }
    }
}
