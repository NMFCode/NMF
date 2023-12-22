// See https://aka.ms/new-console-template for more information
using Microsoft;
using Nerdbank.Streams;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Lifecycle;
using NMF.Glsp.Server.Contracts;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var cancellationToken = CancellationToken.None;

using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
{
    await socket.ConnectAsync("localhost", 60610);
    using (var socketStream = new NetworkStream(socket))
    {
        var jsonFormatter = new SystemTextJsonFormatter();
        jsonFormatter.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        using (var rpc = new JsonRpc(new HeaderDelimitedMessageHandler(socketStream, jsonFormatter)))
        {
            try
            {
                rpc.TraceSource = new TraceSource("JSONRPC", SourceLevels.All);
                var idx = rpc.TraceSource.Listeners.Add(new ConsoleTraceListener());
                var server = rpc.Attach<IGlspServer>();
                rpc.StartListening();
                var initializationResult = await server.InitializeAsync(Guid.NewGuid().ToString(), "1.0.0");
                Console.WriteLine(initializationResult.ProtocolVersion);
                var sessionId = Guid.NewGuid().ToString();
                await server.InitializeClientSessionAsync(sessionId, initializationResult.ServerActions.Keys.First(), null, null);
                await server.ProcessAsync(sessionId, new RequestContextActions
                {
                    ContextId = Contexts.ToolPalette,
                    RequestId = "_1"
                });
                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                rpc.TraceSource.Flush();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
