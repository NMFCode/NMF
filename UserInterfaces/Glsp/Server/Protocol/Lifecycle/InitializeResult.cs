using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Lifecycle
{
    /// <summary>
    /// The initial server response after a client connected
    /// </summary>
    public class InitializeResult
    {
        /// <summary>
        ///  GLSP protocol version that the server is implementing.
        /// </summary>
        public string ProtocolVersion { get; init; }

        /// <summary>
        ///  The actions (grouped by `diagramType`) that the server can handle.
        /// </summary>
        public IDictionary<string, ICollection<string>> ServerActions { get; init; }
    }
}
