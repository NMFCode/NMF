using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Clipboard
{
    /// <summary>
    /// Requests a paste operation from the server by providing the current clipboard data. Typically this means that elements 
    /// should be created based on the data in the clipboard.
    /// </summary>
    public class PasteOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string PasteOperationKind = "paste";

        /// <inheritdoc/>
        public override string Kind => PasteOperationKind;

        /// <summary>
        ///  The clipboard data that should be pasted to the editor's last recorded mouse position (see `editorContext`).
        /// </summary>
        public IDictionary<string, string> ClipboardData { get; } = new Dictionary<string, string>();

        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; set; }

        /// <inheritdoc/>
        public override void Execute(IGlspSession session)
        {
            throw new NotImplementedException();
        }
    }
}
