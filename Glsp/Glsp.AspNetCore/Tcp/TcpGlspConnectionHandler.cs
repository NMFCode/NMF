using Microsoft.AspNetCore.Connections;
using NMF.Glsp.Contracts;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Tcp
{
    /// <summary>
    /// Helper class to handle plain TCP connections with a GLSP server
    /// </summary>
    public class TcpGlspConnectionHandler : ConnectionHandler
    {
        private readonly IGlspServer _server;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="server">The GLSP server to connect to</param>
        public TcpGlspConnectionHandler(IGlspServer server)
        {
            _server = server;
        }

        /// <inheritdoc />
        public override Task OnConnectedAsync(ConnectionContext connection)
        {
            using (var rpc = GlspJsonRpcServerUtil.CreateServer(connection.Transport, _server))
            {
                rpc.StartListening();
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                return rpc.Completion;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            }
        }
    }
}
