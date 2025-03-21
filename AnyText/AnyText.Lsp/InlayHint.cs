using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace NMF.AnyText
{
    [DataContract]
    public partial class InlayHint
    {

        /// <summary>
        /// The position of this hint.
        /// </summary>
        /// 
        [DataMember(Name = "position")]
        [JsonProperty(Required = Required.Always)]
        public ParsePosition Position { get; init; }

        /// <summary>
        /// The label of this hint. A human readable string or an array of
        /// InlayHintLabelPart label parts.
        ///
        /// *Note* that neither the string nor the label part can be empty.
        /// </summary>
        [DataMember(Name = "label")]
        [JsonProperty(Required = Required.Always)]
        public StringOrInlayHintLabelParts Label { get; init; }

    }
}
