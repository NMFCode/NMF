namespace NMF.Glsp.Protocol.Validation
{
    public class ResponseError
    {
        /// <summary>
        ///  Code identifying the error kind.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///  Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  Additional custom data, e.g., a serialized stacktrace.
        /// </summary>
        public object Data { get; set; }
    }
}
