using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Context
{
    /// <summary>
    /// The RequestContextActions is sent from the client to the server to request the available actions for the context with id contextId.
    /// </summary>
    public class RequestContextActions : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestContextActionsKind = "requestContextActions";

        /// <inheritdoc/>
        public override string Kind => RequestContextActionsKind;


        /// <summary>
        ///  The identifier for the context.
        /// </summary>
        public string ContextId { get; init; }

        /// <summary>
        ///  The current editor context.
        /// </summary>
        public EditorContext EditorContext { get; init; }

        /// <inheritdoc/>
        public override Task Execute(IGlspSession session)
        {
            var actions = CreateActions(session)?.ToArray();
            session.SendToClient(new SetContextActions
            {
                ResponseId = RequestId,
                Actions = actions
            });
            return Task.CompletedTask;
        }

        private IEnumerable<LabeledAction> CreateActions(IGlspSession session)
        {

            if (ContextId == Contexts.ToolPalette)
            {
                return session.Language.AllRules.SelectMany(rule => rule.GetRootSkeleton().SuggestActions(null, null, ContextId, EditorContext));
            }

            if (session.Root == null || EditorContext?.SelectedElementIds == null)
            {
                return session.Language.StartRule.GetRootSkeleton().SuggestActions(null, null, ContextId, EditorContext);
            }

            var actions = new List<LabeledAction>();
            var selected = EditorContext.SelectedElementIds
                .Select(session.Root.Resolve).ToList();

            if (!selected.Contains(session.Root))
            {
                selected.Add(session.Root);
            }

            foreach (var item in selected)
            {
                actions.AddRange(item.Skeleton.SuggestActions(item, selected, ContextId, EditorContext));
            }

            return actions;
        }
    }
}
