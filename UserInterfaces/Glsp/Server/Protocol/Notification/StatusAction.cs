using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// This action is typically sent by the server (or the client) to signal a state change. If a timeout is given the 
    /// respective status should disappear after the timeout is reached.
    /// </summary>
    public class StatusAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "status";


        /// <summary>
         ///  The severity of the status.
         /// </summary>
        public string Severity { get; set; }

        /// <summary>
         ///  The message describing the status.
         /// </summary>
        public string Message { get; set; }

        /// <summary>
         ///  Timeout after which a displayed status disappears.
         /// </summary>
        public double Timeout { get; set; }
    }
}
