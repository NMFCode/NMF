using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Sent from the client to the server in order to request hints on whether certain modifications are allowed 
    /// for a specific element type. The RequestTypeHintsAction is optional, but should usually be among the first 
    /// messages sent from the client to the server after receiving the model via RequestModelAction. The response 
    /// is a SetTypeHintsAction.
    /// </summary>
    public class RequestTypeHintsAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestTypeHintsActionKind = "requestTypeHints";

        /// <inheritdoc/>
        public override string Kind => RequestTypeHintsActionKind;

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            var shapeHints = session.Language.CalculateShapeHints();
            var edgeHints = session.Language.CalculateEdgeHints();

            session.SendToClient(new SetTypeHintsAction
            {
                ResponseId = RequestId,
                ShapeHints = shapeHints.ToArray(),
                EdgeHints = edgeHints.ToArray(),
            });
        }
    }
}
