using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to hide a property from serialization
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class XmlIgnorePropertyAttribute : Attribute
    {
        private readonly string property;

        /// <summary>
        /// Creates a XmlIgnorePropertyAttribute
        /// </summary>
        /// <param name="serializeAsAttribute">Value that indicates if the element should be serialized as Xml-attribute</param>
        public XmlIgnorePropertyAttribute(string property)
        {
            this.property = property;
        }

        /// <summary>
        /// Name of the property that should be hidden for serialization
        /// </summary>
        public string Property
        {
            get { return property; }
        }
    }
}
