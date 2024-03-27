using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using StreamJsonRpc;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text.Json;

namespace NMF.Glsp.Server
{
    /// <summary>
    /// Helper class to configure Stream JSON RPC for the usage with the GLSP implementation
    /// </summary>
    public static class JsonRpcServerUtil
    {
        private static readonly TraceSource _traceSource = CreateTraceSource();

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="webSocket">The websocket connection</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(WebSocket webSocket, IGlspServer server)
        {
            var rpc = new JsonRpc(new WebSocketMessageHandler(webSocket, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }

        private static JsonRpcTargetOptions CreateTargetOptions()
        {
            return new JsonRpcTargetOptions
            {
                NotifyClientOfEvents = true,
                EventNameTransform = name => name.ToLowerInvariant()
            };
        }

        private static TraceSource CreateTraceSource()
        {
            var traceSource = new TraceSource("JSONRPC", SourceLevels.All);
            traceSource.Listeners.Add(new ConsoleTraceListener());
            return traceSource;
        }

        private static SystemTextJsonFormatter CreateFormatter()
        {
            var formatter = new SystemTextJsonFormatter();
            formatter.JsonSerializerOptions.IncludeFields = false;
            formatter.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            formatter.JsonSerializerOptions.Converters.Add(new BaseActionConverter());
            formatter.JsonSerializerOptions.Converters.Add(StringDictionaryConverter.Instance);
            formatter.JsonSerializerOptions.Converters.Add(new GElementConverter());
            return formatter;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="pipe">The pipe used for the connection</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(IDuplexPipe pipe, IGlspServer server)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(pipe, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="stream">The stream that represents the connection with the client</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(Stream stream, IGlspServer server)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(stream, CreateFormatter()));
            rpc.TraceSource = _traceSource;
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }
    }
}
