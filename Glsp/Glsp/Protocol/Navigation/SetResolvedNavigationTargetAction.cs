using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// An action sent from the server in response to a ResolveNavigationTargetAction. The response contains the 
    /// resolved element ids for the given target and may contain additional information in the args property.
    /// </summary>
    public class SetResolvedNavigationTargetAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetResolvedNavigationTargetActionKind = "setResolvedNavigationTarget";

        /// <inheritdoc/>
        public override string Kind => SetResolvedNavigationTargetActionKind;


        /// <summary>
        ///  The element ids of the resolved navigation target.
        /// </summary>
        public string[] ElementIds { get; set; }

        /// <summary>
        ///  Custom arguments that may be interpreted by the client.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }
    }
}
