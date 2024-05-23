using NMF.Models.Repository;
using NMF.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes a converter that converts model elements to shallow JSON representation
    /// </summary>
    public class ShallowModelElementConverter : JsonConverter<IModelElement>
    {
        private readonly JsonModelSerializer _serializer;
        private readonly IModelServer _server;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serializer">the serializer to use</param>
        /// <param name="modelServer">The model server for which the property service should be created</param>
        public ShallowModelElementConverter(JsonModelSerializer serializer, IModelServer modelServer)
        {
            _serializer = serializer ?? new JsonModelSerializer(MetaRepository.Instance.Serializer);
            _server = modelServer;
        }

        /// <inheritdoc />
        public override IModelElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var selected = _server?.SelectedElement;
            var wrappedReader = new Utf8JsonStreamReader(reader);
            return (IModelElement)_serializer.DeserializeFragment(ref wrappedReader, _server?.Repository, selected?.Model);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IModelElement value, JsonSerializerOptions options)
        {
            _serializer.Serialize(value, writer, true, true);
        }
    }
}
