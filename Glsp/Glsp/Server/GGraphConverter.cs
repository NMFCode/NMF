using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    internal class GGraphConverter : JsonConverter<GGraph>
    {
        private readonly GElementConverter _converter;

        public GGraphConverter(GElementConverter converter)
        {
            _converter = converter;
        }

        public override GGraph Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("This converter is only used for serialization purposes");
        }

        public override void Write(Utf8JsonWriter writer, GGraph value, JsonSerializerOptions options)
        {
            _converter.Write(writer, value, options);
        }
    }
}
