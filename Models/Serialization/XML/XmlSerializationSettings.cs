using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NMF.Serialization
{
    /// <summary>
    /// Class to represent the serialization settings for a XmlSerializer
    /// </summary>
    public class XmlSerializationSettings
    {
        private static readonly XmlSerializationSettings def = new XmlSerializationSettings() {
            Indent = true,
            CaseSensitive = false,
            NameCase = XmlCaseType.AsInput,
            DefaultNamespace = string.Empty
        };

        private readonly XmlWriterSettings sett = new XmlWriterSettings();

        /// <summary>
        /// Default settings used for the XmlSerializer
        /// </summary>
        static public XmlSerializationSettings Default
        {
            get { return def; }
        }

        internal XmlWriterSettings GetXmlWriterSettings()
        {
            return sett;
        }

        /// <summary>
        /// Gets or sets the encoding of the Xml file
        /// </summary>
        public Encoding Encoding
        {
            get { return sett.Encoding; }
            set { sett.Encoding = value; }
        }

        /// <summary>
        /// Gets or sets the default location
        /// </summary>
        public string DefaultNamespace
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the XmlSerializer should indent new Xml elements for more readable formatting
        /// </summary>
        public bool Indent
        {
            get { return sett.Indent; }
            set { sett.Indent = value; }
        }

        /// <summary>
        /// Indicates whether the deserialization is case sensitive
        /// </summary>
        public bool CaseSensitive
        {
            get;
            set;
        }

        /// <summary>
        /// The strategy for converting character cases for serialization
        /// </summary>
        public XmlCaseType NameCase
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the persistance form of the given identifier
        /// </summary>
        /// <param name="input">The original identifier</param>
        /// <returns>The identifier that should be persisted</returns>
        public string GetPersistanceString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            switch (NameCase)
            {
                case XmlCaseType.CamelCase:
                    return Char.ToLower(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.PascalCase:
                    return Char.ToUpper(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.CamelCaseInvariant:
                    return Char.ToLowerInvariant(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.PascalCaseInvariant:
                    return Char.ToUpperInvariant(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.Upper:
                    return input.ToUpper();
                case XmlCaseType.Lower:
                    return input.ToLower();
                case XmlCaseType.LowerInvariant:
                    return input.ToLowerInvariant();
                case XmlCaseType.UpperInvariant:
                    return input.ToUpperInvariant();
                default:
                    return input;
            }
        }

        internal bool TreatAsEqual(string arg1, string arg2)
        {
            if (string.IsNullOrEmpty(arg1))
            {
                return string.IsNullOrEmpty(arg2);
            }
            else if (string.IsNullOrEmpty(arg2))
            {
                return false;
            }
            if (CaseSensitive)
            {
                return arg1 == arg2;
            }
            else
            {
                return string.Equals(arg1, arg2, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
