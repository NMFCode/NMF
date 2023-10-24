using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// If the source and/or target element of an edge should be adapted, the client can send a ReconnectEdgeOperation to the server.
    /// </summary>
    public class ReconnectEdgeOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "reconnectEdge";

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
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
