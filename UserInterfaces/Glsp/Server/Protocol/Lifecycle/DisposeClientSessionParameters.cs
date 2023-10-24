using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Lifecycle
{
    /// <summary>
    /// When a graphical representation (diagram) is no longer needed, e.g. the tab 
    /// containing the diagram widget has been closed, a DisposeClientSession request has to be sent to the server
    /// </summary>
    public class DisposeClientSessionParameters
    {
        /// <summary>
        ///  Unique identifier of the client session that should be disposed.
        /// </summary>
        public string ClientSessionId { get; }

        /// <summary>
        ///  Additional custom arguments.
        /// </summary>
        public IDictionary<string, string> Args { get; }
    }
}
