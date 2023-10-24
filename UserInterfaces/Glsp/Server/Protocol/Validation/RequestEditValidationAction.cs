using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Validation
{
    /// <summary>
    /// Requests the validation of the given text in the context of the provided model element. Typically sent from the client to the server.
    /// </summary>
    public class RequestEditValidationAction : RequestAction
    {
        /// <inheritdoc/>
        public override string Kind => "requestEditValidation";


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
            throw new NotImplementedException();
        }
    }
}
