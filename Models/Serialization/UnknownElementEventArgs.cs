using System;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes event arguments for an unknown element
    /// </summary>
    public class UnknownElementEventArgs : EventArgs
    {    
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The context object</param>
        /// <param name="propertyXml">The outer XML of the unknown element</param>
        /// <exception cref="ArgumentNullException">Thrown if either is null</exception>
        public UnknownElementEventArgs(object context, string propertyXml)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (propertyXml == null) throw new ArgumentNullException(nameof(propertyXml));

            Context = context;
            PropertyXml = propertyXml;
        }

        /// <summary>
        /// The context object
        /// </summary>
        public object Context { get; }

        /// <summary>
        /// The outer XML of the unknown element
        /// </summary>
        public string PropertyXml { get; }
    }
}
