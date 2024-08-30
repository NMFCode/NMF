using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Server
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
            if (value is GEdge edge)
            {
                writer.WriteString("sourceId", edge.SourceId);
                writer.WriteString("targetId", edge.TargetId);
                if (edge.RoutingPoints.Any())
                {
                    writer.WritePropertyName("routingPoints");
                    writer.WriteStartArray();
                    foreach (var routingPoint in edge.RoutingPoints)
                    {
                        WritePosition(writer, routingPoint);
                    }
                    writer.WriteEndArray();
                }
                if (edge.EdgeSourcePointX is double sourceX)
                {
                    writer.WriteNumber("edgeSourcePointX", sourceX);
                }
                if (edge.EdgeSourcePointY is double sourceY)
                {
                    writer.WriteNumber("edgeSourcePointX", sourceY);
                }
                if (edge.EdgeTargetPointX is double targetX)
                {
                    writer.WriteNumber("edgeTargetPointX", targetX);
                }
                if (edge.EdgeTargetPointY is double targetY)
                {
                    writer.WriteNumber("edgeTargetPointX", targetY);
                }
            }
            else if (value is GLabel label)
            {
                WriteLabel(writer, label, options);
                WriteSizeAndPosition(writer, value);
            }
            else
            {
                WriteSizeAndPosition(writer, value);
            }
            WriteDetails(writer, value, options);
            WriteCssClasses(writer, value);
            WriteChildren(writer, value, options);
            writer.WriteEndObject();
        }

        private static void WriteLabel(Utf8JsonWriter writer, GLabel label, JsonSerializerOptions options)
        {
            writer.WriteString("text", label.Text);
            if (label.EdgeLabelPlacement != null)
            {
                writer.WritePropertyName("edgePlacement");
                JsonSerializer.Serialize(writer, label.EdgeLabelPlacement, options);
            }
        }

        private static void WriteSizeAndPosition(Utf8JsonWriter writer, GElement value)
        {
            if (value.Size is Dimension size)
            {
                writer.WritePropertyName("size");
                writer.WriteStartObject();
                writer.WriteNumber("width", size.Width);
                writer.WriteNumber("height", size.Height);
                writer.WriteEndObject();
            }
            if (value.Position is Point position)
            {
                writer.WritePropertyName("position");
                WritePosition(writer, position);
            }
            if (value.Alignment is Point alignment)
            {
                writer.WritePropertyName("alignment");
                WritePosition(writer, alignment);
            }
        }

        private void WriteChildren(Utf8JsonWriter writer, GElement value, JsonSerializerOptions options)
        {
            if (value.Children.Any())
            {
                writer.WritePropertyName("children");
                writer.WriteStartArray();
                foreach (var item in value.Children)
                {
                    Write(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }

        private static void WriteCssClasses(Utf8JsonWriter writer, GElement value)
        {
            if (value.CssClasses.Any())
            {
                writer.WritePropertyName("cssClasses");
                writer.WriteStartArray();
                foreach (var css in value.CssClasses)
                {
                    writer.WriteStringValue(css);
                }
                writer.WriteEndArray();
            }
        }

        private static void WriteDetails(Utf8JsonWriter writer, GElement value, JsonSerializerOptions options)
        {
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
