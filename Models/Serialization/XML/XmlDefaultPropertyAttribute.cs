using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Marks a property as default property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlDefaultPropertyAttribute : Attribute
    {
        private readonly bool isDefault;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="isDefault">True, if the annotated property is the default property, otherwise False</param>
        public XmlDefaultPropertyAttribute(bool isDefault)
        {
            this.isDefault = isDefault;
        }

        /// <summary>
        /// Gets a value indicating whether this is the default property or not
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
        }
    }
}
