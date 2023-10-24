using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to end the reporting of a progress.
    /// </summary>
    public class EndProgressAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "endProgress";

        /// <summary>
         ///  The ID of the progress reporting to update.
         /// </summary>
        public string ProgressId { get; set; }

        /// <summary>
         ///  The message to show in the progress reporting.
         /// </summary>
        public string Message { get; set; }
    }
}
