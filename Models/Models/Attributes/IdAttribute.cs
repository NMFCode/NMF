using System;

namespace NMF.Models
{
    /// <summary>
    /// Marks a property as an id
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdAttribute : Attribute
    {
        /// <summary>
        /// true, if the property is an id, otherwise false
        /// </summary>
        public bool IsId { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="isId">true, if the property is an id, otherwise false</param>
        public IdAttribute(bool isId)
        {
            IsId = isId;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public IdAttribute() : this(true) { }
    }
}
