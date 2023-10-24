using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// Sent from the client to the server in order to request hints on whether certain modifications are allowed 
    /// for a specific element type. The RequestTypeHintsAction is optional, but should usually be among the first 
    /// messages sent from the client to the server after receiving the model via RequestModelAction. The response 
    /// is a SetTypeHintsAction.
    /// </summary>
    public class RequestTypeHintsAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestTypeHints";

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
