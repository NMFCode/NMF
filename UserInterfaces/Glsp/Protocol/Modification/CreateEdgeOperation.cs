using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// In order to create an edge in the model the client can send a CreateEdgeOperation with the necessary information to create that edge.
    /// </summary>
    public class CreateEdgeOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string CreateEdgeOperationKind = "createEdge";

        /// <inheritdoc/>
        public override string Kind => CreateEdgeOperationKind;

        /// <summary>
        ///  The source element.
        /// </summary>
        public string SourceElementId { get; init; }

        /// <summary>
        ///  The target element.
        /// </summary>
        public string TargetElementId { get; init; }


        /// <summary>
        ///  The type of edge that should be created by the edge creation tool.
        /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override void Execute(IGlspSession session)
        {
            var sourceElement = session.Root.Resolve(SourceElementId);
            var targetElement = session.Root.Resolve(TargetElementId);

            if (sourceElement != null && targetElement != null)
            {
                sourceElement.Skeleton.CreateEdge(sourceElement, this, targetElement, session.Trace);
            }
            else
            {
                throw new InvalidOperationException("Source or target element not found.");
            }
        }
    }
}
