using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to presenting an update of the progress of a long running process in the UI.
    /// </summary>
    public class UpdateProgressAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string UpdateProgressActionKind = "updateProgress";

        /// <inheritdoc/>
        public override string Kind => UpdateProgressActionKind;

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
