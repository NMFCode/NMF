using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Contracts
{
    public interface IGlspServer
    {
        /// <summary>
        ///  Send an `initialize` request to the server. The server needs to be initialized in order to accept and
        ///  process other requests and notifications. The {@link InitializeResult} ist cached and can be retrieved
        ///  via the {@link GLSPClient.initializeResult} property.
        ///  Only the first method invocation actually sends a request to the server. Subsequent invocations simply
        ///  return the cached result.
        /// </summary>
        /// <param name="parameters">Initialize parameters</param>
        /// <returns>A promise of the {@link InitializeResult}.</returns>
        Task<InitializeResult> InitializeServer(InitializeParameters parameters);

        /// <summary>
        ///  Send an `initializeClientSession` request to the server. One client application may open several session.
        ///  Each individual diagram on the client side counts as one session and has to provide
        ///  a unique clientId.
        /// </summary>
        /// <param name="parameters">InitializeClientSession parameters</param>
        /// <returns>A promise that resolves if the initialization was successful</returns>
        Task InitializeClientSession(InitializeClientSessionParameters parameters);

        /// <summary>
        ///  Sends a `disposeClientSession` request to the server. This request has to be sent at the end of client session lifecycle
        ///  e.g. when an editor widget is closed.
        /// </summary>
        /// <param name="parameters">DisposeClientSession parameters</param>
        /// <returns>A promise that resolves if the disposal was successful</returns>
        Task DisposeClientSession(DisposeClientSessionParameters parameters);

        /// <summary>
        ///  Send a `shutdown` notification to the server.
        /// </summary>
        Task ShutdownServer();

        /// <summary>
        ///  Send an action message to the server.
        /// </summary>
        /// <param name="message">The message</param>
        void SendActionMessage(ActionMessage message);
    }

}
