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
    /// Requests a cut operation from the server, i.e., deleting the selected elements from the model. 
    /// Before submitting a CutOperation a client should ensure that the cut elements are put into the clipboard.
    /// </summary>
    public class CutOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "cut";

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
