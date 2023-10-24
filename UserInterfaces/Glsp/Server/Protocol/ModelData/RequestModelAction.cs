using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server in order to request a graphical model. Usually this is the first message 
    /// that is sent from the client to the server, so it is also used to initiate the communication. The response 
    /// is a SetModelAction or an UpdateModelAction.
    /// </summary>
    public class RequestModelAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestModel";

        /// <summary>
         ///  Additional options used to compute the graphical model.
         /// </summary>
        public IDictionary<string, string> Options { get; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            session.SendToClient(new SetModelAction
            {
                ResponseId = RequestId,
                NewRoot = session.Root
            });
        }
    }
}
