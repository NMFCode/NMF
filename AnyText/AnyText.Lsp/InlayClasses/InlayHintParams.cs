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
    /// Denotes the parameters for an inlay request
    /// </summary>
    public class InlayHintParams : WorkDoneProgressParams
    {
        /// <summary>
        /// The text document that theinlay request refers to
        /// </summary>
        [DataMember(Name = "textDocument")]
        [JsonProperty(Required = Required.Always)]
        public TextDocumentIdentifier TextDocument { get; set; }

        /// <summary>
        /// The visible document range for which inlay hints should be computed.
        /// </summary>
        [DataMember(Name = "range")]
        public ParseRange Range { get; set; }
    }
}
