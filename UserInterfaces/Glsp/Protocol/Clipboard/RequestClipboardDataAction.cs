using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Clipboard
{
    /// <summary>
    /// Requests the clipboard data for the current editor context, i.e., the selected elements, in a clipboard-compatible format.
    /// </summary>
    public class RequestClipboardDataAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestClipboardDataActionKind = "requestClipboardData";

        /// <inheritdoc/>
        public override string Kind => RequestClipboardDataActionKind;


        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            throw new NotImplementedException();
        }
    }
}
