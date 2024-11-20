namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// A response action is sent to respond to a request action. The responseId must match the requestId of the preceding 
    /// request. In case the responseId is empty or undefined, the action is handled as standalone, i.e. it was fired 
    /// without a preceding request.
    /// </summary>
    public abstract class ResponseAction : BaseAction
    {

        /// <summary>
        ///  Id corresponding to the request this action responds to.
        /// </summary>
        public string ResponseId { get; init; }
    }
}
