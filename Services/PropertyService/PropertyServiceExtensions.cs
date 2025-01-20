using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NMF.Models.Repository;
using NMF.Models.Services.Forms.Rpc;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Denotes extension methods for a property service
    /// </summary>
    public static class PropertyServiceExtensions
    {
        /// <summary>
        /// Adds the application logic to enable a REST controller to obtain and update the selected element
        /// </summary>
        /// <param name="services">the service collection</param>
        public static void AddSelectionController(this IServiceCollection services)
        {
            var serializer = PropertyServiceRpcUtil.Serializer;

            services.AddControllers()
                .AddApplicationPart(typeof(PropertyServiceExtensions).Assembly)
                .AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.Converters.Add(new ShallowModelElementConverter(serializer, null));
                    json.JsonSerializerOptions.Converters.Add(new SchemaConverter());
                });

            services.AddPropertyService();
        }

        /// <summary>
        /// Adds the property service
        /// </summary>
        /// <param name="services">the service collection</param>
        public static void AddPropertyService(this IServiceCollection services)
        {
            var serializer = PropertyServiceRpcUtil.Serializer;
            services.TryAddSingleton(serializer);
            services.TryAddSingleton<IPropertyService, PropertyService>();
        }

        /// <summary>
        /// Maps the GLSP server
        /// </summary>
        /// <param name="app">The endpoint route builder</param>
        /// <param name="path">The path under which the GLSP server is called</param>
        public static void MapPropertyServiceWebSocket(this IEndpointRouteBuilder app, string path)
        {
            app.Map(path, async (HttpContext context, IPropertyService server, IModelServer modelServer) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    using (var rpc = PropertyServiceRpcUtil.CreateServer(socket, server, modelServer))
                    {
                        rpc.StartListening();
                        await rpc.Completion;
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
