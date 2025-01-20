namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// A general message serves as an envelope carrying an action to be transmitted between the client and the server via a DiagramServer.
    /// </summary>
    public class ActionMessage
    {
        /// <summary>
        ///  Used to identify a specific client session.
        /// </summary>
        public string ClientId { get; init; }

        /// <summary>
        ///  The action to execute.
        /// </summary>
        public BaseAction Action { get; init; }
    }
}
