using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Notification
{
    /// <summary>
    /// This action is typically sent by the server (or the client) to notify the user about something of interest.
    /// </summary>
    public class MessageAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string MessageActionKind = "message";

        /// <inheritdoc/>
        public override string Kind => MessageActionKind;

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
