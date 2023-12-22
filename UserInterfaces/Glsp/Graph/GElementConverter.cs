using NMF.Glsp.Protocol.Types;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Graph
{
    internal class GElementConverter : JsonConverter<GElement>
    {
        public override GElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("This converter is only used for serialization purposes");
        }

        public override void Write(Utf8JsonWriter writer, GElement value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("type", value.Type);
            if (value.Position != null)
            {
                writer.WritePropertyName("position");
                WritePosition(writer, value.Position.Value);
            }
            if (value.Size != null)
            {
                var size = value.Size.Value;
                writer.WritePropertyName("size");
                writer.WriteStartObject();
                writer.WriteNumber("width", size.Width);
                writer.WriteNumber("height", size.Height);
                writer.WriteEndObject();
            }
            if (value is GEdge edge)
            {
                writer.WriteString("sourceId", edge.SourceId);
                writer.WriteString("targetId", edge.TargetId);
                writer.WritePropertyName("routingPoints");
                writer.WriteStartArray();
                foreach (var routingPoint in edge.RoutingPoints)
                {
                    WritePosition(writer, routingPoint);
                }
                writer.WriteEndArray();
            }
            else if (value is GLabel label)
            {
                writer.WriteString("text", label.Text);
            }
            foreach (var detail in value.Details)
            {
                writer.WritePropertyName(detail.Key);
                if (detail.Value != null)
                {
                    JsonSerializer.Serialize(writer, detail.Value, detail.Value.GetType(), options);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
            writer.WritePropertyName("cssClasses");
            writer.WriteStartArray();
            foreach (var css in value.CssClasses)
            {
                writer.WriteStringValue(css);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("children");
            writer.WriteStartArray();
            foreach (var item in value.Children)
            {
                Write(writer, item, options);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private static void WritePosition(Utf8JsonWriter writer, Point point)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", point.X);
            writer.WriteNumber("y", point.Y);
            writer.WriteEndObject();
        }
    }
}
