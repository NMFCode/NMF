using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Context
{
    /// <summary>
    /// Triggers the enablement of the tool that is responsible for creating nodes and initializes it with the creation of nodes of the given elementTypeId.
    /// </summary>
    public class TriggerNodeCreationAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "triggerNodeCreation";


        /// <summary>
         ///  The type of node that should be created by the node creation tool.
         /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
         ///  Custom arguments.
         /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
