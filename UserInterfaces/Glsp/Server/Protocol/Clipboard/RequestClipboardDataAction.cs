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
    /// Requests the clipboard data for the current editor context, i.e., the selected elements, in a clipboard-compatible format.
    /// </summary>
    public class RequestClipboardDataAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestClipboardData";


        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
