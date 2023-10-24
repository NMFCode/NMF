using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// Sent from the client to the server to check wether the provided edge context information is valid i.e. creation
    /// of an edge with the given edge type and source/target element is allowed by the server. Typically this action 
    /// is dispatched by edge creation tools in the creation phase of an edge that’s associated with a dynamic EdgeTypeHint.
    /// </summary>
    public class RequestCheckEdgeAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestCheckEdge";


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
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
