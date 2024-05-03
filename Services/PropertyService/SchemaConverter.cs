using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Denotes a converter that serializes schema elements to JSON
    /// </summary>
    public class SchemaConverter : JsonConverter<SchemaElement>
    {
        /// <inheritdoc />
        public override SchemaElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, SchemaElement value, JsonSerializerOptions options)
        {
            if (value.Writer == null || value.ModelElement == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                value.Writer.WriteSchema(value.ModelElement, writer);
            }
        }
    }
}
