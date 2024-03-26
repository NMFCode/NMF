using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Server;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
        public override async Task Execute(IGlspSession session)
        {
            var sourceUri = Options["sourceUri"] as string;

            session.Initialize(new Uri(sourceUri, UriKind.RelativeOrAbsolute));

            var layoutRequest = new RequestBoundsAction
            {
                NewRoot = session.Root
            };
            var layoutResponse = await session.RequestAsync(layoutRequest);
            if (layoutResponse is ComputedBoundsAction computedBounds)
            {
                computedBounds.UpdateBounds(session);
            }

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
