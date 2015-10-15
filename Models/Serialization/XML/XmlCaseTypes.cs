using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Enumeration of the different case types for serialization
    /// </summary>
    public enum XmlCaseType
    {
        /// <summary>
        /// No changes in characters cases
        /// </summary>
        AsInput = 0,
        /// <summary>
        /// The first character will be converted to lower case, the remaining characters stay on their case
        /// </summary>
        CamelCase,
        /// <summary>
        /// The first character will be converted to upper case, the remaining characters stay on their case
        /// </summary>
        PascalCase,
        /// <summary>
        /// All characters are converted to upper case
        /// </summary>
        Upper,
        /// <summary>
        /// All characters are converted to lower case
        /// </summary>
        Lower,
        /// <summary>
        /// All characters are converted to their culture invariant upper case
        /// </summary>
        UpperInvariant,
        /// <summary>
        /// All characters are converted to their culture invariant lower case
        /// </summary>
        LowerInvariant,
        /// <summary>
        /// The first character will be converted to culture invariant lower case, other characters stay on their case
        /// </summary>
        CamelCaseInvariant,
        /// <summary>
        /// The first character will be converted to culture invariant upper case, other characters stay on their case
        /// </summary>
        PascalCaseInvariant
    }
}
