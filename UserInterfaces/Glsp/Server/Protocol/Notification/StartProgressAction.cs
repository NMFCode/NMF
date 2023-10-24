using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to request presenting the progress of a long running process in the UI.
    /// </summary>
    public class StartProgressAction : BaseAction
    {
        /// <inheritdoc/>
        public override string Kind => "startProgress";

        /// <summary>
         ///  An ID that can be used in subsequent `updateProgress` and `endProgress` events to make them refer to the same progress reporting.
         /// </summary>
        public string ProgressId { get; set; }

        /// <summary>
         ///  Short title of the progress reporting. Shown in the UI to describe the long running process.
         /// </summary>
        public string Title { get; set; }

        /// <summary>
         ///  Optional additional progress message. Shown in the UI to describe the long running process.
         /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///  Progress percentage to display (value range: 0 to 100). If omitted no percentage is shown.
        /// </summary>
        public double? Percentage { get; set; }
    }
}
