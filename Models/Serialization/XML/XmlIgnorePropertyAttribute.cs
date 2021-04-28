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
        /// <param name="property">Name of the property that should be hidden for serialization</param>
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
