using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Clipboard
{
    /// <summary>
    /// Server response to a RequestClipboardDataAction containing the selected elements as clipboard-compatible format.
    /// </summary>
    public class SetClipboardDataAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetClipboardDataActionKind = "setClipboardData";

        /// <inheritdoc/>
        public override string Kind => SetClipboardDataActionKind;


        /// <summary>
        ///  The selected elements from the editor context as clipboard data.
        /// </summary>
        public IDictionary<string, string> ClipboardData { get; } = new Dictionary<string, string>();
    }
}
