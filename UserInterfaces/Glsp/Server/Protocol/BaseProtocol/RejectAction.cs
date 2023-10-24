using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.BaseProtocol
{
    /// <summary>
    /// A reject action is a response fired to indicate that a request must be rejected.
    /// </summary>
    public class RejectAction : ResponseAction
    {
        /// <inheritdoc/>
        public override string Kind => "rejectRequest";

        /// <summary>
         ///  A human-readable description of the reject reason. Typically this is an error message
         ///  that has been thrown when handling the corresponding RequestAction.
         /// </summary>
        public string Message { get; init; }

        /// <summary>
        ///  Optional additional details.
        /// </summary>
        public object Detail { get; init; }
    }
}
