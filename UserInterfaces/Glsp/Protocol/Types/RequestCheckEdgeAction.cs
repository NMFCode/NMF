using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Sent from the client to the server to check wether the provided edge context information is valid i.e. creation
    /// of an edge with the given edge type and source/target element is allowed by the server. Typically this action 
    /// is dispatched by edge creation tools in the creation phase of an edge that’s associated with a dynamic EdgeTypeHint.
    /// </summary>
    public class RequestCheckEdgeAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestCheckEdgeActionKind = "requestCheckEdge";

        /// <inheritdoc/>
        public override string Kind => RequestCheckEdgeActionKind;


        /// <summary>
        ///  The element type of the edge being created.
        /// </summary>
        public string EdgeType { get; init; }

        /// <summary>
        ///  The ID of the edge source element.
        /// </summary>
        public string SourceElementId { get; init; }

        /// <summary>
        ///  The ID of the edge target element to check.
        /// </summary>
        public string TargetElementId { get; init; }

        /// <inheritdoc/>
        public override Task Execute(IGlspSession session)
        {
            var sourceElement = session.Root.Resolve(SourceElementId)?.CreatedFrom;
            var targetElement = session.Root.Resolve(TargetElementId)?.CreatedFrom;

            return Task.CompletedTask;
        }
    }
}
