using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Context
{
    /// <summary>
    /// Triggers the enablement of the tool that is responsible for creating edges and initializes it with the creation of edges of the given elementTypeId.
    /// </summary>
    public class TriggerEdgeCreationAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "triggerEdgeCreation";


        /// <summary>
         ///  The type of edge that should be created by the edge creation tool.
         /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
         ///  Custom arguments.
         /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
