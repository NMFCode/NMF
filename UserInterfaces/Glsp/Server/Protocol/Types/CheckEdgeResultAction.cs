using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// Sent from the server to the client as a response for a {@link RequestCheckEdgeAction}. It provides a boolean 
    /// indicating whether the edge context information provided by the corresponding request action is valid i.e. 
    /// creation of an edge with the given edge type and source/target element is allowed.
    /// </summary>
    public class CheckEdgeResultAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "checkEdgeTargetResult";


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
