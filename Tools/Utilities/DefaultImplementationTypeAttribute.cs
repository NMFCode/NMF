using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Utilities
{
    /// <summary>
    /// This attribute represents that there is a default implementation for the given interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class DefaultImplementationTypeAttribute : Attribute
    {
        /// <summary>
        /// The default implementation type for this interface
        /// </summary>
        public Type DefaultImplementationType { get; private set; }

        /// <summary>
        /// Creates a new attribute with the default implementation interface
        /// </summary>
        /// <param name="implementationType"></param>
        public DefaultImplementationTypeAttribute(Type implementationType)
        {
            DefaultImplementationType = implementationType;
        }
    }
}
