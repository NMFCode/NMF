using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes that the annotated reference is anchored at the given type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AnchorAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the anchor
        /// </summary>
        public Type AnchorType { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="anchorType">the type of the anchor</param>
        public AnchorAttribute(Type anchorType)
        {
            AnchorType = anchorType;
        }
    }
}
