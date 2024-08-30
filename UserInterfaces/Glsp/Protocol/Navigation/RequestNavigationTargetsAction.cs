using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Navigation
{
    /// <summary>
    /// Action that is usually sent from the client to the server to request navigation targets for a specific navigation 
    /// type such as documentation or implementation in the given editor context.
    /// </summary>
    public class RequestNavigationTargetsAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestNavigationTargetsActionKind = "requestNavigationTargets";

        /// <inheritdoc/>
        public override string Kind => RequestNavigationTargetsActionKind;


        /// <summary>
        ///  Identifier of the type of navigation targets we want to retrieve, e.g., 'documentation', 'implementation', etc.
        /// </summary>
        public string TargetTypeId { get; init; }

        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            session.SendToClient(new SetNavigationTargetsAction
            {
                ResponseId = RequestId,
                Targets = new NavigationTarget[0],
            });
            return Task.CompletedTask;
        }
    }
}
