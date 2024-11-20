using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Layout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public IDictionary<string, object> Options { get; set; }

        /// <inheritdoc/>
        public override async Task ExecuteAsync(IGlspSession session)
        {
            var sourceUri = Options["sourceUri"] as string;

            await session.InitializeAsync(new Uri(sourceUri, UriKind.RelativeOrAbsolute));

            session.SendToClient(new SetModelAction
            {
                ResponseId = RequestId,
                NewRoot = session.Root
            });
            session.SendToClient(new SetDirtyAction
            {
                IsDirty = false,
                Reason = "operation"
            });
        }
    }
}
