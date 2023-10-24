using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// This action is typically sent by the server (or the client) to notify the user about something of interest.
    /// </summary>
    public class MessageAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "message";

        /// <summary>
         ///  The severity of the message.
         /// </summary>
        public string Severity { get; set; }

        /// <summary>
         ///  The message text.
         /// </summary>
        public string Message { get; set; }

        /// <summary>
         ///  Further details on the message.
         /// </summary>
        public string Details { get; set; }
    }
}
