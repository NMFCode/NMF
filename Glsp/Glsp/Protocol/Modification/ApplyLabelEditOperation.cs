using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Notification;
using System;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// A very common use case in domain models is the support of labels that display textual information to the user. 
    /// For instance, the GGraph model has support for labels that can be attached to a node, edge, or port, and that 
    /// contain some text that is rendered in the view. To apply new text to such a label element the client may send 
    /// an ApplyLabelEditOperation to the server.
    /// </summary>
    public class ApplyLabelEditOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ApplyLabelEditOperationKind = "applyLabelEdit";

        /// <inheritdoc/>
        public override string Kind => ApplyLabelEditOperationKind;

        /// <summary>
        ///  Identifier of the label model element.
        /// </summary>
        public string LabelId { get; init; }

        /// <summary>
        ///  Text that should be applied on the label.
        /// </summary>
        public string Text { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
        {
            var label = session.Root.Resolve(LabelId) as GLabel;
            if (label == null)
            {
                throw new InvalidOperationException("Label does not exist");
            }

            var validationResult = label.Validate(Text);
            if (validationResult.Severity != SeverityLevels.Ok)
            {
                session.SendToClient(new MessageAction
                {
                    Severity = validationResult.Severity,
                    Message = validationResult.Message,
                });
                session.Synchronize();
                return Task.CompletedTask;
            }

            label.Text = Text;
            return Task.CompletedTask;
        }
    }
}
