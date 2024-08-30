using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// If the source and/or target element of an edge should be adapted, the client can send a ReconnectEdgeOperation to the server.
    /// </summary>
    public class ReconnectEdgeOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ReconnectEdgeOperationKind = "reconnectEdge";

        /// <inheritdoc/>
        public override string Kind => ReconnectEdgeOperationKind;

        /// <summary>
        ///  The edge element that should be reconnected.
        /// </summary>
        public string EdgeElementId { get; set; }

        /// <summary>
        ///  The (new) source element of the edge.
        /// </summary>
        public string SourceElementId { get; set; }

        /// <summary>
        ///  The (new) target element of the edge.
        /// </summary>
        public string TargetElementId { get; set; }

        /// <summary>
        ///  Additional arguments for custom behavior.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            var edge = session.Root.Resolve(EdgeElementId) as GEdge;
            if (edge != null && (edge.SourceId == SourceElementId || edge.SupportsChangingSourceId)
                             && (edge.TargetId == TargetElementId || edge.SupportsChangingTargetId))
            {
                edge.SourceId = SourceElementId;
                edge.TargetId = TargetElementId;
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidOperationException("Changing source or target element of this edge is not supported");
            }
        }
    }
}
