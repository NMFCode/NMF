﻿using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Sent from the server to the client as a response for a {@link RequestCheckEdgeAction}. It provides a boolean 
    /// indicating whether the edge context information provided by the corresponding request action is valid i.e. 
    /// creation of an edge with the given edge type and source/target element is allowed.
    /// </summary>
    public class CheckEdgeResultAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string CheckEdgeResultActionKind = "checkEdgeTargetResult";

        /// <inheritdoc/>
        public override string Kind => CheckEdgeResultActionKind;


        /// <summary>
        ///  true if the selected element is a valid target for this edge,
        ///  false otherwise.
        /// </summary>
        public bool IsValid { get; init; }
        /// <summary>
        ///  The element type of the edge that has been checked.
        /// </summary>
        public string EdgeType { get; init; }

        /// <summary>
        ///  The ID of the source element of the edge that has been checked.
        /// </summary>
        public string SourceElementId { get; init; }
        /// <summary>
        ///  The ID of the target element of the edge that has been checked.
        /// </summary>
        public string TargetElementId { get; init; }
    }
}
