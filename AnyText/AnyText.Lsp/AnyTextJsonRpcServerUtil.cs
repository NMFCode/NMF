﻿using StreamJsonRpc;
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
        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="webSocket">The websocket connection</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(WebSocket webSocket, ILspServer server)
        {
            var rpc = new JsonRpc(new WebSocketMessageHandler(webSocket, CreateFormatter()));
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="pipe">The pipe used for the connection</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(IDuplexPipe pipe, ILspServer server)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(pipe, CreateFormatter()));
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }

        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport
        /// </summary>
        /// <param name="stream">The stream that represents the connection with the client</param>
        /// <param name="server">The server implementation</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(Stream stream, ILspServer server)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(stream, CreateFormatter()));
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
            return rpc;
        }
        /// <summary>
        /// Creates a StreamJSON RPC object for the given transport without localRpcTarget
        /// </summary>
        /// <param name="stream">The stream that represents the connection with the client</param>
        /// <returns>A JSON RPC object that manages the connection to the client</returns>
        public static JsonRpc CreateServer(Stream stream)
        {
            var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(stream, CreateFormatter()));
            return rpc;
        }

        /// <summary>
        /// Registers an ILspServer implementation as a local RPC target on the specified JsonRpc instance.
        /// </summary>
        /// <param name="rpc">The JsonRpc instance that will handle incoming RPC calls.</param>
        /// <param name="server">The ILspServer implementation to be registered as the local target.</param>
        public static void AddLocalRpcTarget(JsonRpc rpc, ILspServer server)
        {
            rpc.AddLocalRpcTarget(server, CreateTargetOptions());
        }
        private static JsonRpcTargetOptions CreateTargetOptions()
        {
            return new JsonRpcTargetOptions
            {
                UseSingleObjectParameterDeserialization = true,
                NotifyClientOfEvents = true,
                EventNameTransform = name => name.ToLowerInvariant()
            };
        }
        /// <summary>
        /// Creates and configures a <see cref="TraceSource"/> instance for logging trace information.
        /// </summary>
        /// <param name="sourceLevels">The SourceLevel used to filter messages by type and severity. Defaults to <see cref="SourceLevels.All"/>.</param>
        /// <returns>A <see cref="TraceSource"/> instance configured for the specified logging level.</returns>
        public static TraceSource CreateTraceSource(SourceLevels sourceLevels = SourceLevels.All)
        {
            var traceSource = new TraceSource("LSP", sourceLevels);
            // Use error stream (stderr) so that VS Code can capture the output
            traceSource.Listeners.Add(new ConsoleTraceListener(useErrorStream: true));
            return traceSource;
        }


        private static IJsonRpcMessageFormatter CreateFormatter()
        {
            return new JsonMessageFormatter();
        }
    }
}