namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// A request action is tied to the expectation of receiving a corresponding response action. The requestId property 
    /// is used to match the received response with the original request.
    /// </summary>
    public abstract class RequestAction : ExecutableAction
    {

        /// <summary>
        ///  Unique id for this request. In order to match a response to this request, the response needs to have the same id.
        /// </summary>
        public string RequestId { get; init; }
    }
}
