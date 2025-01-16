using LspTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the options for formatting a document or a range of text.
    /// </summary>
    public class FormattingOptions
    {
        /// <summary>
        /// Gets or sets the size of a tab in spaces.
        /// </summary>
        [DataMember(Name = "tabSize")]
        [JsonProperty(Required = Required.Always)]
        public uint TabSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether spaces should be inserted instead of tabs.
        /// </summary>
        [DataMember(Name = "insertSpaces")]
        [JsonProperty(Required = Required.Always)]
        public bool InsertSpaces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether trailing whitespace should be trimmed.
        /// Optional.
        /// </summary>
        [DataMember(Name = "trimTrailingWhitespace")]
        [JsonProperty(Required = Required.Default)]
        public bool? TrimTrailingWhitespace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a final newline should be inserted at the end of the document.
        /// Optional.
        /// </summary>
        [DataMember(Name = "insertFinalNewline")]
        [JsonProperty(Required = Required.Default)]
        public bool? InsertFinalNewline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple final newlines at the end of the document should be trimmed.
        /// Optional.
        /// </summary>
        [DataMember(Name = "trimFinalNewlines")]
        [JsonProperty(Required = Required.Default)]
        public bool? TrimFinalNewlines { get; set; }

        /// <summary>
        /// Gets or sets additional formatting options that are not explicitly defined.
        /// </summary>
        [JsonExtensionData]
        [JsonProperty(Required = Required.Default)]
        public Dictionary<string, object> OtherOptions { get; set; }
    }
}
