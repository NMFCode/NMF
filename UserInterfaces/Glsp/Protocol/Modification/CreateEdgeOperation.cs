﻿using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public IDictionary<string, object> Args { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            var sourceElement = session.Root.Resolve(SourceElementId);
            var targetElement = session.Root.Resolve(TargetElementId);

            if (sourceElement != null && targetElement != null)
            {
                sourceElement.Skeleton.CreateEdge(sourceElement, this, targetElement, session.Trace);
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidOperationException("Source or target element not found.");
            }
        }
    }
}
