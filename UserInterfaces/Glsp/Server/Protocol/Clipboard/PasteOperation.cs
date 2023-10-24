using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Clipboard
{
    /// <summary>
    /// Requests a paste operation from the server by providing the current clipboard data. Typically this means that elements 
    /// should be created based on the data in the clipboard.
    /// </summary>
    public class PasteOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "paste";

        /// <summary>
         ///  The clipboard data that should be pasted to the editor's last recorded mouse position (see `editorContext`).
         /// </summary>
        public IDictionary<string, string> ClipboardData { get; } = new Dictionary<string, string>();

        /// <summary>
         ///  The current editor context.
         /// </summary>
        public EditorContext EditorContext { get; set; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
