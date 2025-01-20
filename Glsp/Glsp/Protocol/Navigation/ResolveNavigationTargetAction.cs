using NMF.Glsp.Contracts;
using NMF.Glsp.Protocol.BaseProtocol;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// If a client cannot navigate to a target directly, a ResolveNavigationTargetAction may be sent to the server 
    /// to resolve the navigation target to one or more model elements. This may be useful in cases where the 
    /// resolution of each target is expensive or the client architecture requires an indirection.
    /// </summary>
    public class ResolveNavigationTargetAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ResolveNavigationTargetActionKind = "resolveNavigationTarget";

        /// <inheritdoc/>
        public override string Kind => ResolveNavigationTargetActionKind;

        /// <summary>
        ///  The navigation target to resolve.
        /// </summary>
        public NavigationTarget NavigationTarget { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            session.SendToClient(new SetResolvedNavigationTargetAction
            {
                ResponseId = RequestId,
                ElementIds = new string[0]
            });
            return Task.CompletedTask;
        }
    }
}
