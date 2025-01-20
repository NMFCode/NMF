using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Notification
{
    /// <summary>
    /// This action is sent by the server to the client to end the reporting of a progress.
    /// </summary>
    public class EndProgressAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string EndProgressActionKind = "endProgress";

        /// <inheritdoc/>
        public override string Kind => EndProgressActionKind;

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
