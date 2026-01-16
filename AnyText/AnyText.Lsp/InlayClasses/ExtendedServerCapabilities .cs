using LspTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
    /// <summary>
    /// Extends the server capabilities from the LSP types package with features from 3.17.0
    /// </summary>
    [DataContract]
    public class ExtendedServerCapabilities : ServerCapabilities
    {
        /// <summary>
        /// Gets or sets the settings to provide inlay hints
        /// </summary>
        [DataMember(Name = "inlayHintProvider")]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlayHintOptions InlayHintProvider { get; set; }
    }
}
