using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace NMF.Serialization.Json
{
    /// <summary>
    /// Denotes a Json Serializer based on System.Text.Json
    /// </summary>
    public class JsonSerializer : Serializer
    {
        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        public JsonSerializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        public JsonSerializer(XmlSerializationSettings settings) : this(settings, null) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public JsonSerializer(Serializer parent) : base(parent) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public JsonSerializer(Serializer parent, XmlSerializationSettings settings) : base(parent, settings) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="knownTypes">A collection of known types</param>
        public JsonSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes)
            : base(settings, knownTypes)
        {
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="target">The Utf8JsonWriter to write the JSON-code on</param>
        /// <param name="source">The object to be serialized</param>
        /// <param name="shallow">true, if only attributes should be serialized, otherwise false</param>
        /// <param name="fragment">true, if the element should be serialized as fragment</param>
        public void Serialize(object source, Utf8JsonWriter target, bool shallow = false, bool fragment = false)
        {
            source = SelectRoot(source, fragment);
            XmlSerializationContext context = CreateSerializationContext(source);
            Serialize(source, target, null, XmlIdentificationMode.FullObject, context, shallow);
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write the JSON-code on</param>
        /// <param name="obj">The object to be serialized</param>
        /// <param name="property">The property for which the object is serialized</param>
        /// <param name="context">The serialization context</param>
        /// <param name="identificationMode">A value indicating whether it is allowed to the serializer to use identifier</param>
        /// <param name="shallow">true, if only attributes should be serialized, otherwise false</param>
        /// <remarks>If a converter is provided that is able to convert the object to string and convert the string back to this object, just the string-conversion is printed out</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void Serialize(object obj, Utf8JsonWriter writer, IPropertySerializationInfo property, XmlIdentificationMode identificationMode, XmlSerializationContext context, bool shallow = false)
        {
            if (obj == null) return;
            if (property != null && property.IsStringConvertible)
            {
                writer.WriteStringValue(property.ConvertToString(obj));
                return;
            }

            ITypeSerializationInfo info = GetSerializationInfoForInstance(obj, true);
            if (WriteIdentifiedObject(writer, obj, identificationMode, info, context)) return;
            
            if (info.IsCollection)
            {
                writer.WriteStartArray();
                foreach (var item in (IEnumerable)obj)
                {
                    Serialize(item, writer, null, property?.IdentificationMode ?? XmlIdentificationMode.FullObject, context, shallow);
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartObject();
                if (property == null || property.PropertyType.IsExplicitTypeInformationRequired(info))
                {
                    writer.WriteString("$type", GetFullyQualifiedName(info.Namespace, info.ElementName));
                }
                if (info.ConstructorProperties != null)
                {
                    WriteConstructorProperties(writer, obj, info, context);
                }
                WriteProperties(writer, obj, info, context, shallow);
                writer.WriteEndObject();
            }

        }

        private string GetFullyQualifiedName(string @namespace, string elementName)
        {
            if (string.IsNullOrEmpty(@namespace))
            {
                return elementName;
            }
            return @namespace + "$" + elementName;
        }

        /// <summary>
        /// Writes the properties necessary for the constrctor call of this element
        /// </summary>
        /// <param name="writer">The JSON writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteConstructorProperties(Utf8JsonWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            for (int i = 0; i <= info.ConstructorProperties.GetUpperBound(0); i++)
            {
                IPropertySerializationInfo pi = info.ConstructorProperties[i];
                writer.WriteString(pi.ElementName, pi.ConvertToString(pi.GetValue(obj, context)));
            }
        }

        /// <summary>
        /// Writes the properties of the given object
        /// </summary>
        /// <param name="writer">The Json writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="shallow">true, if only attributes should be serialized, otherwise false</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteProperties(Utf8JsonWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context, bool shallow)
        {
            foreach (IPropertySerializationInfo pi in info.AttributeProperties)
            {
                var value = pi.GetValue(obj, context);
                if (Settings.SerializeDefaultValues || pi.ShouldSerializeValue(obj, value))
                {
                    WriteAttributeValue(writer, context, pi, value);
                }
                if (pi.IsIdentifier)
                {
                    string id = pi.GetValue(obj, context)?.ToString();
                    context.RegisterId(id, obj, pi.PropertyType);
                }
            }
            if (!shallow)
            {
                foreach (IPropertySerializationInfo pi in info.ElementProperties)
                {
                    var value = pi.GetValue(obj, context);
                    if (Settings.SerializeDefaultValues || pi.ShouldSerializeValue(obj, value))
                    {
                        writer.WritePropertyName(pi.ElementName);

                        Serialize(value, writer, pi, pi.IdentificationMode, context);
                    }
                }
            }
        }

        private void WriteAttributeValue(Utf8JsonWriter writer, XmlSerializationContext context, IPropertySerializationInfo property, object value)
        {
            writer.WritePropertyName(property.ElementName);
            if (property.PropertyType.IsCollection)
            {
                writer.WriteStartArray();
                foreach (var item in value as IEnumerable)
                {
                    writer.WriteStringValue(GetAttributeValue(item, property.PropertyType.CollectionItemType, true, context));
                }
                writer.WriteEndArray();
            }
            else if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(GetAttributeValue(value, property.PropertyType, false, context));
            }
        }

        /// <summary>
        /// Writes the provided identified object
        /// </summary>
        /// <param name="writer">The Json writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        /// <param name="identificationMode">The identification mode for the current object</param>
        /// <returns>true, if the object could be written as identified object, otherwise false</returns>
        protected virtual bool WriteIdentifiedObject(Utf8JsonWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (!info.IsIdentified) return false;
            string id = info.IdentifierProperty.GetValue(obj, context)?.ToString();
            if (identificationMode == XmlIdentificationMode.Identifier || (identificationMode == XmlIdentificationMode.AsNeeded && context.ContainsId(id, info)))
            {
                writer.WriteStringValue(id);
                return true;
            }
            else if (identificationMode == XmlIdentificationMode.FullObject && context.ContainsId(id, info))
            {
                writer.WriteStartObject();
                writer.WriteString(info.IdentifierProperty.ElementName, info.IdentifierProperty.ConvertToString(info.IdentifierProperty.GetValue(obj, context)));
                writer.WriteEndObject();
                return true;
            }
            return false;
        }

        internal object DeserializeInternal(ref Utf8JsonStreamReader reader, IPropertySerializationInfo property, XmlSerializationContext context)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Null => null,
                JsonTokenType.Number => ReadNumber(ref reader, property),
                JsonTokenType.String => property.ConvertFromString(reader.GetString()),
                JsonTokenType.StartObject => DeserializeObject(ref reader, property, context),
                JsonTokenType.StartArray => DeserializeArray(ref reader, property, context),
                _ => throw new JsonException($"Token {reader.TokenType} was unexpected at {reader.TokenStartIndex}")
            };
        }

        private object ReadNumber(ref Utf8JsonStreamReader reader, IPropertySerializationInfo property)
        {
            var effectiveType = property.PropertyMinType ?? property.PropertyType.MappedType;
            if (effectiveType.IsGenericType && effectiveType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                effectiveType = effectiveType.GetGenericArguments()[0];
            }
            if (effectiveType == typeof(int))
            {
                return reader.TryGetInt32(out var value) ? value : null;
            }
            else if (effectiveType == typeof(long))
            {
                return reader.TryGetInt64(out var value) ? value : null;
            }
            else if (effectiveType == typeof(double))
            {
                return reader.TryGetDouble(out var value) ? value : null;
            }
            else if (effectiveType == typeof(float))
            {
                return reader.TryGetSingle(out var value) ? value : null;
            }
            else if (effectiveType == typeof(short))
            {
                return reader.TryGetInt16(out var value) ? value : null;
            }
            else if (effectiveType == typeof(uint))
            {
                return reader.TryGetUInt32(out var value) ? value : null;
            }
            else if (effectiveType == typeof(ulong))
            {
                return reader.TryGetUInt64(out var value) ? value : null;
            }
            else if (effectiveType == typeof(ushort))
            {
                return reader.TryGetUInt16(out var value) ? value : null;
            }
            else if (effectiveType == typeof(sbyte))
            {
                return reader.TryGetSByte(out var value) ? value : null;
            }
            return null;
        }

        private object DeserializeArray(ref Utf8JsonStreamReader reader, IPropertySerializationInfo property, XmlSerializationContext context)
        {
            var type = property.PropertyType;
            var collection = type.CreateObject(Array.Empty<object>());
            if (reader.TokenType != JsonTokenType.StartArray) { Throw(); }

            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.EndArray)
                {
                    var value = DeserializeInternal(ref reader, null, context);
                    type.AddToCollection(collection, value);
                }
                else
                {
                    return collection;
                }
            }
            throw new JsonException();
        }

        /// <summary>
        /// Deserializes the contents from the given reader
        /// </summary>
        /// <param name="reader">the JSON reader to read from</param>
        /// <returns>the object contained in the JSON format</returns>
        public object Deserialize(ref Utf8JsonStreamReader reader)
        {
            return Deserialize(ref reader, false);
        }

        /// <summary>
        /// Deserializes the contents from the given reader
        /// </summary>
        /// <param name="reader">the JSON reader to read from</param>
        /// <param name="fragment">true, if the stream contains a fragment, otherwise false</param>
        /// <returns>the object contained in the JSON format</returns>
        public object Deserialize(ref Utf8JsonStreamReader reader, bool fragment)
        {
            if (!fragment && reader.TokenType == JsonTokenType.None)
            {
                reader.Read();
            }

            ITypeSerializationInfo tsi = null;
            var root = CreateObject(ref reader, ref tsi);
            var context = CreateSerializationContext(SelectRoot(root, fragment));
            Initialize(ref reader, context, root, tsi);
            context.Cleanup();
            return root;
        }

        private object DeserializeObject(ref Utf8JsonStreamReader reader, IPropertySerializationInfo property, XmlSerializationContext context)
        {
            ITypeSerializationInfo info = property.PropertyType;
            object deserialized = CreateObject(ref reader, ref info);
            Initialize(ref reader, context, deserialized, info);
            return deserialized;
        }

        /// <summary>
        /// Creates the output object for the given reader position
        /// </summary>
        /// <param name="reader">the JSON reader with the current position</param>
        /// <param name="info">the type info</param>
        /// <returns>the created object</returns>
        protected object CreateObject(ref Utf8JsonStreamReader reader, ref ITypeSerializationInfo info)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Read();
            }
            if (reader.TokenType != JsonTokenType.PropertyName) { Throw(); }
            var propertyName = reader.GetString();
            if (propertyName == "$type")
            {
                reader.Read();
                var typeString = reader.GetString();
                info = GetTypeFromTypeString(typeString);
                if (info == null)
                {
                    throw new InvalidOperationException($"Type {typeString} could not be resolved");
                }
                reader.Read();
            }
            else if (info == null)
            {
                throw new JsonException("missing $type");
            }
            if (info.ConstructorProperties != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return info.CreateObject(Array.Empty<object>());
            }
        }

        /// <summary>
        /// Initializes the provided object given the reader position
        /// </summary>
        /// <param name="reader">the JSON reader</param>
        /// <param name="context">the serialization context</param>
        /// <param name="deserialized">the deserialized object that should be initialized</param>
        /// <param name="info">the type information for the object</param>
        protected void Initialize(ref Utf8JsonStreamReader reader, XmlSerializationContext context, object deserialized, ITypeSerializationInfo info)
        {
            ISupportInitialize initSupport = deserialized as ISupportInitialize;
            initSupport?.BeginInit();
            InitializeCore(ref reader, context, deserialized, info);
            initSupport?.EndInit();
        }

        private void InitializeCore(ref Utf8JsonStreamReader reader, XmlSerializationContext context, object deserialized, ITypeSerializationInfo info)
        {
            do
            {
                if (reader.TokenType == JsonTokenType.EndObject) { return; }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    ReadProperty(ref reader, context, ref deserialized, info, propertyName);
                }
            } while (reader.Read());
            throw new JsonException();
        }

        private void ReadProperty(ref Utf8JsonStreamReader reader, XmlSerializationContext context, ref object deserialized, ITypeSerializationInfo info, string propertyName)
        {
            var property = info.AttributeProperties
                                    .Concat(info.ElementProperties)
                                    .FirstOrDefault(p => Settings.TreatAsEqual(p.ElementName, propertyName));

            if (property == null)
            {
                base.OnUnknownAttribute(new UnknownAttributeEventArgs(deserialized, null, propertyName, ParseElementJson(ref reader)));
            }
            else
            {
                if (property.PropertyType.IsCollection)
                {
                    ReadCollection(ref reader, context, deserialized, property);
                }
                else
                {
                    if (reader.TokenType != JsonTokenType.String || property.PropertyType.IsStringConvertible)
                    {
                        var value = DeserializeInternal(ref reader, property, context);
                        property.SetValue(deserialized, value, context);
                        if (property == info.IdentifierProperty && value is string idString)
                        {
                            if (OverrideIdentifiedObject(deserialized, reader, context))
                            {
                                deserialized = RegisterOrReplace(deserialized, context, info, idString);
                            }
                            else
                            {
                                context.RegisterId(idString, deserialized, info);
                            }
                        }
                    }
                    else
                    {
                        CreateSetPropertyDelay(property, deserialized, reader.GetString(), context);
                    }
                }
            }
        }

        private void ReadCollection(ref Utf8JsonStreamReader reader, XmlSerializationContext context, object deserialized, IPropertySerializationInfo property)
        {
            var collection = property.GetValue(deserialized, context);
            var info = property.PropertyType.CollectionItemType;
            if (reader.TokenType != JsonTokenType.StartArray) Throw();
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (info.IsStringConvertible)
                    {
                        property.AddToCollection(collection, info.ConvertFromString(reader.GetString()), context);
                    }
                    else
                    {
                        CreateAddToPropertyDelay(property, deserialized, reader.GetString(), context);
                    }
                }
                else
                {
                    property.PropertyType.AddToCollection(collection, DeserializeInternal(ref reader, property, context));
                }
                reader.Read();
            }
        }

        private static object RegisterOrReplace(object obj, XmlSerializationContext context, ITypeSerializationInfo info, string id)
        {
            if (!context.ContainsId(id, info))
            {
                context.RegisterId(id, obj, info);
            }
            else
            {
                obj = context.Resolve(id, info);
            }

            return obj;
        }

        /// <summary>
        /// Determines whether the already identified element should be overridden
        /// </summary>
        /// <param name="obj">The object that would be overridden</param>
        /// <param name="reader">The current reader position</param>
        /// <param name="context">The serialization context</param>
        /// <returns>true, if the element shall be overridden, otherwise false</returns>
        protected virtual bool OverrideIdentifiedObject(object obj, Utf8JsonStreamReader reader, XmlSerializationContext context)
        {
            return true;
        }

        private static string ParseElementJson(ref Utf8JsonStreamReader reader)
        {
            return JsonDocument.ParseValue(ref reader._jsonReader).ToString();
        }

        private ITypeSerializationInfo GetTypeFromTypeString(string typeString)
        {
            var colon = typeString.LastIndexOf('$');
            if (colon == -1)
            {
                return GetTypeInfo(null, typeString);
            }
            return GetTypeInfo(typeString.Substring(0, colon), typeString.Substring(colon + 1));
        }

        private void Throw()
        {
            throw new JsonException("Input Json has invalid format");
        }
    }
}
