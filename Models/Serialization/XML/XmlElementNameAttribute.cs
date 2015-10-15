using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to provide the serialization name for the specified element. This persistance name can be changed by the serialization setting (various case types)
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class XmlElementNameAttribute : Attribute
    {
       private readonly string elementName;

        /// <summary>
        /// Creates a XmlElementNameAttribute
        /// </summary>
        /// <param name="elementName">The name to use in serialization for the specified element</param>
        public XmlElementNameAttribute(string elementName)
        {
            this.elementName = elementName;
        }

        /// <summary>
        /// The name for serialization of the specified element
        /// </summary>
        public string ElementName
        {
            get { return elementName; }
        }
    }
}
