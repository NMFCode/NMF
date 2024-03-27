using System;

namespace NMF.Models
{
    /// <summary>
    /// Marks a reference as a containment
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainmentAttribute : Attribute
    {
        /// <summary>
        /// Indicates whether the reference is a containment
        /// </summary>
        public bool IsContainment { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="isContainment">true, if the reference is a containment, otherwise false</param>
        public ContainmentAttribute(bool isContainment)
        {
            IsContainment = isContainment;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ContainmentAttribute() : this(true) { }
    }
}
