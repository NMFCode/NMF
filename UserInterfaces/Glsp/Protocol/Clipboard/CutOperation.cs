using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;

namespace NMF.Glsp.Protocol.Clipboard
{
    /// <summary>
    /// Requests a cut operation from the server, i.e., deleting the selected elements from the model. 
    /// Before submitting a CutOperation a client should ensure that the cut elements are put into the clipboard.
    /// </summary>
    public class CutOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string CutOperationKind = "cut";

        /// <inheritdoc/>
        public override string Kind => CutOperationKind;

        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }


        /// <inheritdoc/>
        public override void Execute(IGlspSession session)
        {
            throw new NotImplementedException();
        }
    }
}
