using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace NMF.AnyText
{
    public partial class InlayEntry
    {

        /// <summary>
        /// The position of this hint.
        /// </summary>
        /// 
        public ParsePosition Position { get; init; }

        /// <summary>
        /// The label of this hint. A human readable string or an array of
        /// InlayHintLabelPart label parts.
        ///
        /// *Note* that neither the string nor the label part can be empty.
        /// </summary>
        public string Label { get; init; }

    }
}
