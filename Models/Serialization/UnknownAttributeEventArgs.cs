using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes event arguments for an unknown attribute
    /// </summary>
    public class UnknownAttributeEventArgs : EventArgs
    {
        /// <summary>
        /// The context object
        /// </summary>
        public object Context { get; }
        
        /// <summary>
        /// The namespace of the attribute
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The local name of the attribute
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The attribute value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The context object</param>
        /// <param name="ns">The namespace of the attribute</param>
        /// <param name="name">The local name of the attribute</param>
        /// <param name="value">The attribute value</param>
        /// <exception cref="ArgumentNullException">Thrown if either is null</exception>
        public UnknownAttributeEventArgs(object context, string ns, string name, string value)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Namespace = ns;
            Name = name;
            Value = value;
            Context = context;
        }
    }
}
