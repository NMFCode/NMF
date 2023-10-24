using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Validation
{
    /// <summary>
    /// Response to a RequestEditValidationAction containing the validation result for applying a text on a certain model element.
    /// </summary>
    public class SetEditValidationResultAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "setEditValidationResult";


        /// <summary>
         ///  Validation status.
         /// </summary>
        public ValidationStatus Status { get; init; }

        //// 
         ///  Additional arguments for custom behavior.
         /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
