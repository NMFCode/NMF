using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.Models.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Denotes extension methods to add LSP server implementations to an ASP.NET Core dependency injection container
    /// </summary>
    public static class AnyTextWebSocketServerExtensions
    {
        /// <summary>
        ///     Registers the LSP server itself
        /// </summary>
        /// <param name="services">The service container the LSP server should be added to</param>
        public static void AddLspServer(this IServiceCollection services)
        {
            services.AddSingleton<ILspServer, LspServer>();
            services.TryAddSingleton<IModelServer, ModelServer>();
        }

        /// <summary>
        ///     Adds a language
        /// </summary>
        /// <typeparam name="TLanguage">The type of the language</typeparam>
        /// <param name="services">The service container the language should be added to</param>
        public static void AddLanguage<TLanguage>(this IServiceCollection services)
            where TLanguage : Grammar
        {
            services.AddSingleton<TLanguage>();
            services.AddSingleton<Grammar>(s => s.GetRequiredService<TLanguage>());
        }

        /// <summary>
        ///     Maps the LSP server
        /// </summary>
        /// <param name="app">The endpoint route builder</param>
        /// <param name="path">The path under which the LSP server is called</param>
        public static void MapLspWebSocketServer(this IEndpointRouteBuilder app, string path)
        {
            app.Map(path, async (HttpContext context, ILspServer server) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    using (var rpc = AnyTextJsonRpcServerUtil.CreateServer(socket, server))
                    {
                        server.SetRpc(rpc);
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

                return Results.BadRequest();
            });
        }
    }
}