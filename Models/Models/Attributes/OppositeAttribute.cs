using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Specifies the opposite of a given property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OppositeAttribute : Attribute
    {
        /// <summary>
        /// The opposite property
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="oppositeType">the type where the opposite property is defined</param>
        /// <param name="propertyName">the name of the opposite property</param>
        /// <exception cref="ArgumentNullException">thrown if either oppositeType or propertyName is null</exception>
        public OppositeAttribute(Type oppositeType, string propertyName)
        {
            if (oppositeType == null) throw new ArgumentNullException(nameof(oppositeType));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            Property = oppositeType.GetProperty(propertyName);
        }
    }
}
