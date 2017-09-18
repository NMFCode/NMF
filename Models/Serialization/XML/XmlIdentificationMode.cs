using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Defines states how to handle with identification of instances
    /// </summary>
    public enum XmlIdentificationMode
    {
        /// <summary>
        /// Let the Serializer decide
        /// </summary>
        /// <remarks>This will lead the serializer to write the full object on first occurence and uses of references afterwards</remarks>
        AsNeeded = 0,
        /// <summary>
        /// Use the identifier only
        /// </summary>
        Identifier = 1,
        /// <summary>
        /// Write the full object
        /// </summary>
        /// <remarks>If this attribute appears somewhere else in the resulting Xml-file, be sure that every property before this one is marked to use identifiers!</remarks>
        FullObject = 2
    }
}
