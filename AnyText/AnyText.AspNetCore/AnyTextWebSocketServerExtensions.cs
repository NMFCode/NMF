using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.Models;
using NMF.Models.Services;
using NMF.Synchronizations;

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
        ///     Registers the LSP server itself
        /// </summary>
        /// <param name="services">The service container the LSP server should be added to</param>
        public static void AddSynchronizingLspServer(this IServiceCollection services)
        {
            services.AddSingleton<ILspServer, SynchronizingLspServer>();
            services.TryAddSingleton<IModelServer, ModelServer>();
        }

        /// <summary>
        /// Declares a synchronization between multiple files
        /// </summary>
        /// <typeparam name="TLeft">the type of the left model</typeparam>
        /// <typeparam name="TRight">the type of the right model</typeparam>
        /// <typeparam name="TSynchronization">the type of the synchronization</typeparam>
        /// <typeparam name="TStartRule">the type of the start rule</typeparam>
        /// <param name="services">The service container the synchronization should be added to</param>
        /// <param name="leftExtension">the file extension of the left model</param>
        /// <param name="rightExtension">the file extension of the right model</param>
        /// <param name="isAutomatic">true, if the synchronization should be started automatically</param>
        public static void AddSynchronization<TLeft, TRight, TSynchronization, TStartRule>(this IServiceCollection services, string leftExtension, string rightExtension, bool isAutomatic = false)
            where TLeft : class, IModelElement
            where TRight : class, IModelElement
            where TSynchronization : ReflectiveSynchronization, new()
            where TStartRule : SynchronizationRule<TLeft, TRight>
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IModelSynchronization), new ModelSynchronization<TLeft, TRight, TSynchronization, TStartRule>
            {
                LeftExtension = leftExtension,
                RightExtension = rightExtension,
                IsAutomatic = isAutomatic
            }));
        }

        /// <summary>
        /// Declares a synchronization between multiple files
        /// </summary>
        /// <typeparam name="T">the type of the model</typeparam>
        /// <param name="services">The service container the synchronization should be added to</param>
        /// <param name="leftExtension">the file extension of the left model</param>
        /// <param name="rightExtension">the file extension of the right model</param>
        /// <param name="isAutomatic">true, if the synchronization should be started automatically</param>
        public static void AddSynchronization<T>(this IServiceCollection services, string leftExtension, string rightExtension, bool isAutomatic = false)
            where T : class, IModelElement
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IModelSynchronization), new HomogenModelSync<T>
            {
                LeftExtension = leftExtension,
                RightExtension = rightExtension,
                IsAutomatic = isAutomatic
            }));
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