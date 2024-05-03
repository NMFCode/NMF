using Microsoft.Extensions.DependencyInjection;
using NMF.Models.Repository;

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
            var serializer = new JsonModelSerializer(MetaRepository.Instance.Serializer);

            services.AddControllers()
                .AddApplicationPart(typeof(PropertyServiceExtensions).Assembly)
                .AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.Converters.Add(new ShallowModelElementConverter(serializer));
                    json.JsonSerializerOptions.Converters.Add(new SchemaConverter());
                });
            services.AddSingleton(serializer);
        }
    }
}
