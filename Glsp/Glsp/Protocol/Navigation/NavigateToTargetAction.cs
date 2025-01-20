using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// Action that triggers the navigation to a particular navigation target. This may be used by the client internally or 
    /// may be sent from the server.
    /// </summary>
    public class NavigateToTargetAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string NavigateToTargetActionKind = "navigateToTarget";

        /// <inheritdoc/>
        public override string Kind => NavigateToTargetActionKind;

        /// <summary>
        ///  The target to which we navigate.
        /// </summary>
        public NavigationTarget Target { get; init; }
    }
}
