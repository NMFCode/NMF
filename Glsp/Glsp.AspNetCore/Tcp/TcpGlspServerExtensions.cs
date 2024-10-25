using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Routing;
using NMF.Glsp.Server.Tcp;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Denotes extension methods to use an ASP.NET Core server to offer plain TCP GLSP connections
    /// </summary>
    public static class TcpGlspServerExtensions
    {
        /// <summary>
        /// Instructs the connection to use plain TCP for this connection
        /// </summary>
        /// <param name="builder">The connection builder</param>
        /// <returns>The input builder for chaining</returns>
        public static IConnectionBuilder UseTcpGlspServer(this IConnectionBuilder builder)
        {
            return builder.UseConnectionHandler<TcpGlspConnectionHandler>();
        }

        /// <summary>
        /// Maps the connection builder
        /// </summary>
        /// <param name="app">The endpoint route builder</param>
        public static void MapGlspTcpServer(this IEndpointRouteBuilder app)
        {
            app.MapConnectionHandler<TcpGlspConnectionHandler>("/_glsp");
        }
    }
}
