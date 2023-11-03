using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Sent from the client to the server in order to request a graphical model. Usually this is the first message 
    /// that is sent from the client to the server, so it is also used to initiate the communication. The response 
    /// is a SetModelAction or an UpdateModelAction.
    /// </summary>
    public class RequestModelAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestModelActionKind = "requestModel";

        /// <inheritdoc/>
        public override string Kind => RequestModelActionKind;

        /// <summary>
        ///  Additional options used to compute the graphical model.
        /// </summary>
        public IDictionary<string, string> Options { get; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            var sourceUri = Options["sourceUri"];

            session.Initialize(new Uri(sourceUri));

            session.SendToClient(new SetModelAction
            {
                ResponseId = RequestId,
                NewRoot = session.Root
            });
        }
    }
}
