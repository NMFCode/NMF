using NMF.Models.Repository;
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
        private readonly JsonModelSerializer _serializer = new JsonModelSerializer(MetaRepository.Instance.Serializer);

        /// <inheritdoc />
        public override IModelElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IModelElement value, JsonSerializerOptions options)
        {
            _serializer.Serialize(value, writer, true);
        }
    }
}
