﻿using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Notification;
using System.Threading.Tasks;

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
        public override Task ExecuteAsync(IGlspSession session)
        {
            var el = session.Root.Resolve(ModelElementId);
            if (el is GLabel label)
            {
                session.SendToClient(new SetEditValidationResultAction
                {
                    ResponseId = RequestId,
                    Status = label.Validate(Text)
                });
                return Task.CompletedTask;
            }

            session.SendToClient(new SetEditValidationResultAction
            {
                ResponseId = RequestId,
                Status = new ValidationStatus
                {
                    Message = "ok",
                    Severity = SeverityLevels.Info
                }
            });
            return Task.CompletedTask;
        }
    }
}
