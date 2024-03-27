using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Lifecycle;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Contracts
{
    /// <summary>
    /// Denotes the interface for a GLSP server
    /// </summary>
    public interface IGlspServer
    {
        /// <summary>
        ///  Send an `initialize` request to the server. The server needs to be initialized in order to accept and
        ///  process other requests and notifications. The {@link InitializeResult} ist cached and can be retrieved
        ///  via the {@link GLSPClient.initializeResult} property.
        ///  Only the first method invocation actually sends a request to the server. Subsequent invocations simply
        ///  return the cached result.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="protocolVersion"></param>
        /// <param name="args"></param>
        /// <returns>A promise of the {@link InitializeResult}.</returns>
        [JsonRpcMethod("initialize")]
        Task<InitializeResult> InitializeAsync(string applicationId, string protocolVersion, IDictionary<string, object> args = null);

        /// <summary>
        ///  Send an `initializeClientSession` request to the server. One client application may open several session.
        ///  Each individual diagram on the client side counts as one session and has to provide
        ///  a unique clientId.
        /// </summary>
        /// <param name="clientSessionId">Unique identifier for the new client session</param>
        /// <param name="diagramType">Unique identifier of the diagram type for which the session should be configured.</param>
        /// <param name="args">Additional custom arguments.</param>
        /// <param name="clientActionKinds">The set of action kinds that can be handled by the client.
        ///  Used by the server to know which dispatched actions should be forwarded to the client.</param>
        /// <returns>A promise that resolves if the initialization was successful</returns>
        [JsonRpcMethod("initializeClientSession")]
        Task InitializeClientSessionAsync(string clientSessionId, string diagramType, string[] clientActionKinds = null, IDictionary<string, object> args = null);

        /// <summary>
        ///  Sends a `disposeClientSession` request to the server. This request has to be sent at the end of client session lifecycle
        ///  e.g. when an editor widget is closed.
        /// </summary>
        /// <param name="clientSessionId">Unique identifier of the client session that should be disposed.</param>
        /// <param name="args">Additional custom arguments.</param>
        /// <returns>A promise that resolves if the disposal was successful</returns>
        [JsonRpcMethod("disposeClientSession")]
        Task DisposeClientSessionAsync(string clientSessionId, IDictionary<string, object> args = null);

        /// <summary>
        ///  Send a `shutdown` notification to the server.
        /// </summary>
        [JsonRpcMethod("shutdown")]
        Task ShutdownAsync();

        /// <summary>
        ///  Send an action message to the server.
        /// </summary>
        /// <param name="clientId">the client ID</param>
        /// <param name="action">The message</param>
        [JsonRpcMethod("process")]
        Task ProcessAsync(string clientId, BaseAction action);

        /// <summary>
        /// Gets raised when the client should process an action message
        /// </summary>
        event EventHandler<ActionMessage> Process;
    }

}
