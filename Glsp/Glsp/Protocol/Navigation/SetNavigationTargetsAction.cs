using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// Response action from the server following a RequestNavigationTargetsAction. It contains all available navigation targets 
    /// for the queried target type in the provided editor context. The server may also provide additional information using the 
    /// arguments, e.g., warnings, that can be interpreted by the client.
    /// </summary>
    public class SetNavigationTargetsAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetNavigationTargetsActionKind = "setNavigationTargets";

        /// <inheritdoc/>
        public override string Kind => SetNavigationTargetsActionKind;


        /// <summary>
        ///  A list of navigation targets.
        /// </summary>
        public NavigationTarget[] Targets { get; init; }

        /// <summary>
        ///  Custom arguments that may be interpreted by the client.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }
    }
}
