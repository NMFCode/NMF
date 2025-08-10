using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NMF.Glsp.Contracts;
using NMF.Glsp.Language;
using NMF.Glsp.Server;
using NMF.Models.Services;

namespace NMF.Glsp.Server.Websockets
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
            services.TryAddSingleton<IModelServer, ModelServer>();
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
        /// <param name="path">The path under which the GLSP server is called</param>
        public static void MapGlspWebSocketServer(this IEndpointRouteBuilder app, string path)
        {
            app.Map(path, async (HttpContext context, IGlspServer server) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    using (var rpc = GlspJsonRpcServerUtil.CreateServer(socket, server))
                    {
                        rpc.StartListening();
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                        await rpc.Completion;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
                    }
#if NET8_0_OR_GREATER
                    return Results.Empty;
#else
                    return Results.Ok();
#endif
                }
                else
                {
                    return Results.BadRequest();
                }
            });
        }
    }
}
