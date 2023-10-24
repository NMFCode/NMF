using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Context
{
    /// <summary>
    /// The RequestContextActions is sent from the client to the server to request the available actions for the context with id contextId.
    /// </summary>
    public class RequestContextActions : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestContextActions";


        /// <summary>
         ///  The identifier for the context.
         /// </summary>
        public string ContextId { get; init; }

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
