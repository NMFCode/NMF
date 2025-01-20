using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// If a navigation target cannot be resolved or the resolved target is something that is not part of our source 
    /// model, e.g., a separate documentation file, a NavigateToExternalTargetAction may be sent. Since the target 
    /// it outside of the model scope such an action would be typically handled by an integration layer (such as 
    /// the surrounding IDE).
    /// </summary>
    public class NavigateToExternalTargetAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string NavigateToExternalTargetActionKind = "navigateToExternalTarget";

        /// <inheritdoc/>
        public override string Kind => NavigateToExternalTargetActionKind;

        /// <summary>
        ///  The target to which we navigate.
        /// </summary>
        public NavigationTarget NavigationTarget { get; init; }
    }
}
