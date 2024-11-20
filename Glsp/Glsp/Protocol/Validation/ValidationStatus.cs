namespace NMF.Glsp.Protocol.Validation
{
    /// <summary>
    /// Denotes the validation status for editing text
    /// </summary>
    public class ValidationStatus
    {

        /// <summary>
        ///  The severity of the validation returned by the server.
        /// </summary>
        public string Severity { get; init; }

        /// <summary>
        ///  The validation status message which may be rendered in the view.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        ///  A potential error that encodes more details.
        /// </summary>
        public ResponseError Error { get; init; }
    }
}
