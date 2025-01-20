using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Context
{
    /// <summary>
    /// Triggers the enablement of the tool that is responsible for creating nodes and initializes it with the creation of nodes of the given elementTypeId.
    /// </summary>
    public class TriggerNodeCreationAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string TriggerNodeCreationActionKind = "triggerNodeCreation";

        /// <inheritdoc/>
        public override string Kind => TriggerNodeCreationActionKind;


        /// <summary>
        ///  The type of node that should be created by the node creation tool.
        /// </summary>
        public string ElementTypeId { get; init; }


        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; set; }
    }
}
