using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Lifecycle
{
    /// <summary>
    /// When a new graphical representation (diagram) is created a InitializeClientSession request has 
    /// to be sent to the server. Each individual diagram on the client side counts as one session and 
    /// has to provide a unique clientSessionId and its diagramType. In addition, custom arguments can 
    /// be provided in the args map to allow for custom initialization behavior on the server.
    /// </summary>
    public class InitializeClientSessionParameters
    {
        /// <summary>
        ///  Unique identifier for the new client session.
        /// </summary>
        public string ClientSessionId { get; }

        /// <summary>
        ///  Unique identifier of the diagram type for which the session should be configured.
        /// </summary>
        public string DiagramType { get; }

        /// <summary>
        ///  The set of action kinds that can be handled by the client.
        ///  Used by the server to know which dispatched actions should be forwarded to the client.
        /// </summary>
        public string[] ClientActionKinds { get; }

        /// <summary>
        ///  Additional custom arguments.
        /// </summary>
        public IDictionary<string, string> Args { get; }
    }
}
