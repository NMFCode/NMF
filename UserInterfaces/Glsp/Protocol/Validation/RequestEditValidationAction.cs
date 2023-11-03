using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Server.Contracts;

namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// Requests the validation of the given text in the context of the provided model element. Typically sent from the client to the server.
    /// </summary>
    public class RequestEditValidationAction : RequestAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RequestEditValidationActionKind = "requestEditValidation";

        /// <inheritdoc/>
        public override string Kind => RequestEditValidationActionKind;


        /// <summary>
        ///  Context in which the text is validated, e.g., 'label-edit'.
        /// </summary>
        public string ContextId { get; init; }

        /// <summary>
        ///  Model element that is being edited.
        /// </summary>
        public string ModelElementId { get; init; }

        /// <summary>
        ///  Text that should be considered for the model element.
        /// </summary>
        public string Text { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            session.SendToClient(new SetEditValidationResultAction
            {
                ResponseId = RequestId,
                Status = new ValidationStatus
                {
                    Message = "ok",
                    Severity = SeverityLevels.Info
                }
            });
        }
    }
}
