using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebSockets;
using NMF.Glsp.Language;
using NMF.Glsp.Server;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Denotes extension methods to add GLSP server implementations to an ASP.NET Core dependency injection container
    /// </summary>
    public static class GlspWebSocketServerExtensions
    {
        /// <summary>
        /// Registers the GLSP server itself
        /// </summary>
        /// <param name="services">The service container the GLSP server should be added to</param>
        public static void AddGlspServer(this IServiceCollection services)
        {
            services.AddSingleton<IGlspServer, GlspServer>();
        }

        /// <summary>
        /// Adds a graphical language
        /// </summary>
        /// <typeparam name="TLanguage">The type of graphical language</typeparam>
        /// <param name="services">The service container the graphical language should be added to</param>
        public static void AddLanguage<TLanguage>(this IServiceCollection services)
            where TLanguage : GraphicalLanguage
        {
            services.AddSingleton<IClientSessionProvider, TLanguage>();
        }

        /// <summary>
        /// Maps the GLSP server
        /// </summary>
        /// <param name="app">The endpoint route builder</param>
        /// <param name="contributionId">The id of the contribution under which the GLSP server is called</param>
        public static void MapGlspWebSocketServer(this IEndpointRouteBuilder app, string contributionId)
        {
            app.Map($"/services/glsp/{contributionId}", async (HttpContext context, IGlspServer server) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    using (var rpc = JsonRpcServerUtil.CreateServer(socket, server))
                    {
                        rpc.StartListening();
                        await rpc.Completion;
                    }
                    return Results.Empty;
                }
                else
                {
                    return Results.BadRequest();
                }
            });
        }
    }
}
