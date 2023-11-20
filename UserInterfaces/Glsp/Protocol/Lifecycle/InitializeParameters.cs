using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Lifecycle
{
    /// <summary>
    /// Denotes parameters for the initialization of a client
    /// </summary>
    public class InitializeParameters
    {
        /// <summary>
        ///  Unique identifier for the current client application.
        /// </summary>
        public string ApplicationId { get; init; }

        /// <summary>
        ///  GLSP protocol version that this client is implementing.
        /// </summary>
        public string ProtocolVersion { get; init; }

        /// <summary>
        ///  Additional custom arguments e.g. application specific parameters.
        /// </summary>
        public IDictionary<string, string> Args { get; init; }
    }
}
