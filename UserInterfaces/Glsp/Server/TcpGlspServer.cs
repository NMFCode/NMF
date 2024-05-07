using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NMF.Glsp.Server.Contracts;
using System.Threading;

namespace NMF.Glsp.Server
{
    /// <summary>
    /// Denotes a plain TCP based GLSP server
    /// </summary>
    public class TcpGlspServer
    {
        private readonly IGlspServer _server;
        private readonly List<Task> _openConnections = new List<Task>();

        /// <summary>
        /// Creates a TCP server for the given GLSP implementation
        /// </summary>
        /// <param name="server"></param>
        public TcpGlspServer(IGlspServer server)
        {
            _server = server;
        }

        /// <summary>
        /// Gets or sets the IP address on which the server is running
        /// </summary>
        public IPAddress IPAddress { get; set; } = IPAddress.Any;

        /// <summary>
        /// Gets or sets the port on which the GLSP server is running
        /// </summary>
        public int Port { get; set; } = 5007;

        /// <summary>
        /// Runs the GLSP server
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the server</param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress, Port));
            socket.Listen();
            while (!cancellationToken.IsCancellationRequested)
            {
                var connection = await socket.AcceptAsync(cancellationToken);
                _ = HandleConnectionAsync(connection, cancellationToken);
            }
            await Task.WhenAll(_openConnections.ToArray());
        }

        private async Task HandleConnectionAsync(Socket connection, CancellationToken cancellationToken)
        {
            using (var transport = new NetworkStream(connection))
            {
                var rpc = GlspJsonRpcServerUtil.CreateServer(transport, _server);
                rpc.StartListening();
                cancellationToken.Register(rpc.Dispose);
                lock (_openConnections)
                {
                    _openConnections.Add(rpc.Completion);
                }
                await rpc.Completion.ContinueWith(t =>
                {
                    lock (_openConnections)
                    {
                        _openConnections.Remove(t);
                    }
                }, TaskScheduler.Default);
            }
        }
    }
}
