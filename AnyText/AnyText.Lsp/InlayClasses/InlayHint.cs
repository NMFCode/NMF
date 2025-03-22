using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using LspTypes;

namespace NMF.AnyText.InlayClasses
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
        public Position Position { get; init; }

        /// <summary>
        /// The label of this hint. A human readable string or an array of
        /// InlayHintLabelPart label parts.
        ///
        /// *Note* that neither the string nor the label part can be empty.
        /// </summary>
        [DataMember(Name = "label")]
        [JsonProperty(Required = Required.Always)]
        public object Label { get; init; }

        /// <summary>
        /// The kind of this hint. Can be omitted, in which case the client should fall back to a reasonable default.
        /// </summary>
        public InlayHintKind? Kind { get; set; }

        /// <summary>
        /// Optional text edits that are performed when accepting this inlay hint.
        /// </summary>
        public TextEdit[]? TextEdits { get; set; }

        /// <summary>
        /// The tooltip text when you hover over this item.
        /// </summary>
        public object? Tooltip { get; set; } // Can be string or MarkupContent

        /// <summary>
        /// Render padding before the hint.
        /// </summary>
        public bool? PaddingLeft { get; set; }

        /// <summary>
        /// Render padding after the hint.
        /// </summary>
        public bool? PaddingRight { get; set; }

        /// <summary>
        /// A data entry field that is preserved on an inlay hint between
        /// a `textDocument/inlayHint` and a `inlayHint/resolve` request.
        /// </summary>
        public object? Data { get; set; } // LSPAny equivalent

    }
}
