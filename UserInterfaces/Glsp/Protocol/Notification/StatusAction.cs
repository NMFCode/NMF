using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Notification
{
    /// <summary>
    /// This action is typically sent by the server (or the client) to signal a state change. If a timeout is given the 
    /// respective status should disappear after the timeout is reached.
    /// </summary>
    public class StatusAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string StatusActionKind = "status";

        /// <inheritdoc/>
        public override string Kind => StatusActionKind;


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
