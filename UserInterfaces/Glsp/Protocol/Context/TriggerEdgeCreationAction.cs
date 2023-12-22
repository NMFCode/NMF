using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Context
{
    /// <summary>
    /// Triggers the enablement of the tool that is responsible for creating edges and initializes it with the creation of edges of the given elementTypeId.
    /// </summary>
    public class TriggerEdgeCreationAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string TriggerEdgeCreationActionKind = "triggerEdgeCreation";

        /// <inheritdoc/>
        public override string Kind => TriggerEdgeCreationActionKind;


        /// <summary>
        ///  The type of edge that should be created by the edge creation tool.
        /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; set; }
    }
}
