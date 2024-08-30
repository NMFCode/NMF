using NMF.Glsp.Protocol.BaseProtocol;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// Response to a RequestEditValidationAction containing the validation result for applying a text on a certain model element.
    /// </summary>
    public class SetEditValidationResultAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetEditValidationResultActionKind = "setEditValidationResult";

        /// <inheritdoc/>
        public override string Kind => SetEditValidationResultActionKind;


        /// <summary>
        ///  Validation status.
        /// </summary>
        public ValidationStatus Status { get; init; }

        /// <summary> 
        /// Additional arguments for custom behavior.
        /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
