using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Server
{
    /// <summary>
    /// Denotes a JSON converter from JSON to a dictionary of string and object
    /// </summary>
    public class StringDictionaryConverter : JsonConverter<IDictionary<string, object>>
    {
        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static readonly StringDictionaryConverter Instance = new StringDictionaryConverter();

        /// <inheritdoc />
        public override IDictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
            var result = new Dictionary<string, object>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) { return  result; }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    result[propertyName] = reader.TokenType switch
                    {
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        JsonTokenType.Number when reader.TryGetInt64(out long l) => l,
                        JsonTokenType.Number => reader.GetDouble(),
                        JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
                        JsonTokenType.String => reader.GetString()!,
                        _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
                    };
                }
            }
            throw new JsonException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IDictionary<string, object> value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue(); return;
            }

            writer.WriteStartObject();
            foreach ( var kvp in value )
            {
                writer.WritePropertyName(kvp.Key);
                if (kvp.Value != null)
                {
                    JsonSerializer.Serialize(writer, kvp.Value, kvp.Value.GetType(), options);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
            writer.WriteEndObject();
        }
    }
}
