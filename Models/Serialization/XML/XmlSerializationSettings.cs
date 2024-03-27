using System;
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
            SerializeDefaultValues = false,
            DefaultNamespace = string.Empty,
            ResolveMissingAttributesAsElements = true
        };

        private readonly XmlWriterSettings sett = new XmlWriterSettings();
        /// <summary>
        /// Denotes whether the settings object is fixed and therefore any property change should be disallowed
        /// </summary>
        protected bool isFixed = false;

        /// <summary>
        /// Default settings used for the XmlSerializer
        /// </summary>
        static public XmlSerializationSettings Default
        {
            get { return def; }
        }

        /// <summary>
        /// Create a fixed version of these settings
        /// </summary>
        /// <returns>A new instance of serialization settings where changes are not allowed any more</returns>
        public virtual XmlSerializationSettings Fix()
        {
            var copy = new XmlSerializationSettings
            {
                Indent = Indent,
                CaseSensitive = CaseSensitive,
                NameCase = NameCase,
                SerializeDefaultValues = SerializeDefaultValues,
                DefaultNamespace = DefaultNamespace,
                Encoding = Encoding
            };
            copy.isFixed = true;
            return copy;
        }

        /// <summary>
        /// Creates the settings to write XML documents
        /// </summary>
        /// <returns>An instance of XmlWriterSettings</returns>
        protected internal virtual XmlWriterSettings CreateXmlWriterSettings()
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

        private string defaultNamespace;
        private bool caseSensitive;
        private bool serializeDefaultValues;
        private bool resolveMissingAttributesAsElements = true;
        private XmlCaseType nameCase;

        private void CheckAccess()
        {
            if (isFixed)
            {
                throw new InvalidOperationException("The serialization settings cannot be changed after the settings have been used to construct a serializer.");
            }
        }

        /// <summary>
        /// Gets or sets the default location
        /// </summary>
        public string DefaultNamespace
        {
            get => defaultNamespace;
            set { CheckAccess(); defaultNamespace = value; }
        }

        /// <summary>
        /// True, if the serializer should check element properties if an attribute cannot be resolved
        /// </summary>
        public bool ResolveMissingAttributesAsElements
        {
            get => resolveMissingAttributesAsElements;
            set { CheckAccess(); resolveMissingAttributesAsElements = value; }
        }

        /// <summary>
        /// Indicates whether the XmlSerializer should indent new Xml elements for more readable formatting
        /// </summary>
        public bool Indent
        {
            get { return sett.Indent; }
            set { CheckAccess(); sett.Indent = value; }
        }

        /// <summary>
        /// Indicates whether the deserialization is case sensitive
        /// </summary>
        public bool CaseSensitive
        {
            get => caseSensitive;
            set { CheckAccess(); caseSensitive = value; }
        }

        /// <summary>
        /// Indicates whether properties should be serialized even if the values match the defaults
        /// </summary>
        public bool SerializeDefaultValues
        {
            get => serializeDefaultValues;
            set {
                CheckAccess();
                serializeDefaultValues = value;
            }
        }

        /// <summary>
        /// The strategy for converting character cases for serialization
        /// </summary>
        public XmlCaseType NameCase
        {
            get => nameCase;
            set {
                CheckAccess();
                nameCase = value;
            }
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
                    return char.ToLower(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.PascalCase:
                    return char.ToUpper(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.CamelCaseInvariant:
                    return char.ToLowerInvariant(input[0]).ToString() + input.Substring(1);
                case XmlCaseType.PascalCaseInvariant:
                    return char.ToUpperInvariant(input[0]).ToString() + input.Substring(1);
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

        /// <summary>
        /// Determines whether the two strings should be treated as equal given the current settings
        /// </summary>
        /// <param name="arg1">the first string</param>
        /// <param name="arg2">the second string</param>
        /// <returns>true, if they should be treated as equal, otherwise false</returns>
        public bool TreatAsEqual(string arg1, string arg2)
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
