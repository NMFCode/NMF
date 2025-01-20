using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to request presenting the progress of a long running process in the UI.
    /// </summary>
    public class StartProgressAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string StartProgressActionKind = "startProgress";

        /// <inheritdoc/>
        public override string Kind => StartProgressActionKind;

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
