using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Can set the identification mode of a property
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class XmlIdentificationModeAttribute : Attribute
    {
        readonly XmlIdentificationMode mode;

        /// <summary>
        /// Creates a new XmlIdentificationModeAttribute with the given identification mode
        /// </summary>
        /// <param name="mode">The mode for identification</param>
        public XmlIdentificationModeAttribute(XmlIdentificationMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// The mode for identification
        /// </summary>
        public XmlIdentificationMode Mode
        {
            get { return mode; }
        }
    }
}
