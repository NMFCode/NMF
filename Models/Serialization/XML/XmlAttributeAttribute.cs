using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to decide whether the specified property should be serialized as attribute.
    /// </summary>
    /// <remarks>A property can only be serialized as attribute, if the property Type supports conversion to and from string</remarks>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class XmlAttributeAttribute : Attribute
    {
        private readonly bool serializeAsAttribute;
        
        /// <summary>
        /// Creates a XmlAttributeAttribute
        /// </summary>
        /// <param name="serializeAsAttribute">Value that indicates if the element should be serialized as Xml-attribute</param>
        public XmlAttributeAttribute(bool serializeAsAttribute)
        {
            this.serializeAsAttribute = serializeAsAttribute;
        }

        /// <summary>
        /// Value that indicates whether the element should be serialized as Xml-Attribute
        /// </summary>
        public bool SerializeAsAttribute
        {
            get { return serializeAsAttribute; }
        }
    }
}
