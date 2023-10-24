using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to presenting an update of the progress of a long running process in the UI.
    /// </summary>
    public class UpdateProgressAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "updateProgress";

        /// <summary>
         ///  The ID of the progress reporting to update.
         /// </summary>
        public string ProgressId { get; set; }

        /// <summary>
         ///  The message to show in the progress reporting.
         /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  The percentage (value range: 0 to 100) to show in the progress reporting.
        /// </summary>
        public double? Percentage { get; set; }
    }
}
