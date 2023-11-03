namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// A reject action is a response fired to indicate that a request must be rejected.
    /// </summary>
    public class RejectAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string RejectActionKind = "rejectRequest";

        /// <inheritdoc/>
        public override string Kind => RejectActionKind;

        /// <summary>
        ///  A human-readable description of the reject reason. Typically this is an error message
        ///  that has been thrown when handling the corresponding RequestAction.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        ///  Optional additional details.
        /// </summary>
        public object Detail { get; init; }
    }
}
