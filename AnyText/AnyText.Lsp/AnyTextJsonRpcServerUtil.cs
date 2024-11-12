using StreamJsonRpc;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.WebSockets;

namespace NMF.AnyText
{
    /// <summary>
    /// Helper class to configure Stream JSON RPC for the usage with the LSP implementation
    /// </summary>
    public static class AnyTextJsonRpcServerUtil
    {
        private static readonly TraceSource _traceSource = CreateTraceSource();

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="webSocket">The websocket connection</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(WebSocket webSocket)
        {
            var rpc = new JsonRpc(new WebSocketMessageHandler(webSocket, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            return rpc;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="pipe">The pipe used for the connection</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(IDuplexPipe pipe)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(pipe, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            return rpc;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="stream">The stream that represents the connection with the client</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(Stream stream)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(stream, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            return rpc;
        }

        public static JsonRpcTargetOptions CreateTargetOptions()
        {
            return new JsonRpcTargetOptions
            {
                UseSingleObjectParameterDeserialization = true,
                NotifyClientOfEvents = true,
                EventNameTransform = name => name.ToLowerInvariant()
            };
        }

        private static TraceSource CreateTraceSource()
        {
            var traceSource = new TraceSource("LSP", SourceLevels.All);
            // use error stream such that VS Code can see the stdout
            traceSource.Listeners.Add(new ConsoleTraceListener(true));
            return traceSource;
        }

        private static IJsonRpcMessageFormatter CreateFormatter()
        {
            return new JsonMessageFormatter();
        }
    }
}
