﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NMF.Serialization.Xmi
{
    /// <summary>
    /// Denotes a serializer implementation that serializes objects to XMI
    /// </summary>
    public class XmiSerializer : XmlSerializer
    {
        /// <summary>
        /// Creates a new XmiSerializer with default settings and no preloaded types
        /// </summary>
        public XmiSerializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new XmiSerializer with default settings
        /// </summary>
        /// <param name="additionalTypes">Set of types to preload into the serializer</param>
        /// <remarks>Types will be loaded with default settings</remarks>
        public XmiSerializer(IEnumerable<Type> additionalTypes) : this(XmlSerializationSettings.Default, additionalTypes) { }

        /// <summary>
        /// Creates a new XmiSerializer with the specified settings
        /// </summary>
        /// <param name="settings">Serializer-settings for the serializer. Can be null or Nothing in Visual Basic. In this case, the default settings will be used.</param>
        public XmiSerializer(XmlSerializationSettings settings) : base(settings) {}

        /// <summary>
        /// Creates a new XmiSerializer with the specified settings and the given preloaded types
        /// </summary>
        /// <param name="additionalTypes">Set of types to load into the serializer</param>
        /// <param name="settings">The settings to use for the serializer</param>
        /// <remarks>The types will be loaded with the specified settings</remarks>
        public XmiSerializer(XmlSerializationSettings settings, IEnumerable<Type> additionalTypes) : base(settings, additionalTypes) { }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public XmiSerializer(XmlSerializer parent) : base(parent) { }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public XmiSerializer(XmlSerializer parent, XmlSerializationSettings settings) : base(parent, settings) { }

        private static readonly string TypeField = "type";
        /// <summary>
        /// Denotes the namespace for XML schema instance
        /// </summary>
        public static readonly string XMLSchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        
        /// <summary>
        /// Denotes the standard prefix to use for the XML schema instance namespace
        /// </summary>
        public static readonly string XMLSchemaInstancePrefix = "xsi";

        /// <summary>
        /// Denotes the standard prefix for the XMI namespace
        /// </summary>
        public static readonly string XMIPrefix = "xmi";

        /// <summary>
        /// Denotes the XMI namespace
        /// </summary>
        public static readonly string XMINamespace = "http://www.omg.org/XMI";

        /// <summary>
        /// Gets or sets the root prefix
        /// </summary>
        public string RootPrefix { get; set; }

        /// <inheritdoc />
        protected override bool GoToPropertyContent(XmlReader reader)
        {
            //do not move forward
            return true;
        }

        /// <inheritdoc />
        protected override XmlSerializationContext CreateSerializationContext(object root)
        {
            return new XmiSerializationContext(root);
        }

        /// <inheritdoc />
        protected override ITypeSerializationInfo GetElementTypeInfo(XmlReader reader, IPropertySerializationInfo property)
        {
            string attr = reader.GetAttribute(TypeField, XMLSchemaInstanceNamespace)
                ?? reader.GetAttribute(TypeField, XMINamespace);
            if (attr != null)
            {
                var prefix = string.Empty;
                var localName = attr;
                int separator = attr.IndexOf(':');
                if (separator != -1)
                {
                    prefix = attr.Substring(0, separator);
                    localName = attr.Substring(separator + 1);
                }
                var ns = reader.LookupNamespace(prefix);
                return GetTypeInfo(ns, localName) ?? HandleUnknownType(property, ns, localName);
            }
            else
            {
                if (property.PropertyType.IsCollection)
                {
                    return property.PropertyType.CollectionItemType;
                }
                else
                {
                    return property.PropertyType;
                }
            }
        }

        /// <inheritdoc />
        protected override void WriteBeginElement(XmlWriter writer, object obj, ITypeSerializationInfo info)
        {
            string prefix = info.NamespacePrefix;
            if (prefix == null)
            {
                prefix = writer.LookupPrefix(info.Namespace);
            }
            else if (writer.LookupPrefix(info.Namespace) == null)
            {
                writer.WriteAttributeString("xmlns", prefix, null, info.Namespace);
            }
            string value = prefix + ":" + info.ElementName;
            writer.WriteAttributeString(XMLSchemaInstancePrefix, TypeField, XMLSchemaInstanceNamespace, value);
        }

        /// <inheritdoc />
        protected override void WriteEndElement(XmlWriter writer, object obj, ITypeSerializationInfo info)
        {
            // no explicit end element in XMI
        }

        /// <inheritdoc />
        protected override void WriteBeginRootElement(XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            writer.WriteStartElement(info.NamespacePrefix ?? RootPrefix, info.ElementName, info.Namespace);

            writer.WriteAttributeString(XMIPrefix, "version", XMINamespace, "2.0");
            writer.WriteAttributeString("xmlns", XMLSchemaInstancePrefix, null, XMLSchemaInstanceNamespace);
        }

        /// <inheritdoc />
        protected override void WriteEndRootElement(XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            writer.WriteEndElement();
        }

        /// <inheritdoc />
        protected override bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            return false;
        }

        /// <inheritdoc />
        protected override void WriteElementProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (info.DefaultProperty != null)
            {
                var content = info.DefaultProperty.GetValue(obj, context);
                if (Settings.SerializeDefaultValues || info.DefaultProperty.ShouldSerializeValue(obj, content))
                {
                    writer.WriteString(GetAttributeValue(content, info.DefaultProperty.PropertyType, false, context));
                }
            }
            foreach (var pi in info.ElementProperties)
            {
                WriteElementProperty(writer, obj, context, pi);
            }
        }

        private void WriteElementProperty(XmlWriter writer, object obj, XmlSerializationContext context, IPropertySerializationInfo pi)
        {
            var value = pi.GetValue(obj, context);
            if (value != null && (Settings.SerializeDefaultValues || pi.ShouldSerializeValue(obj, value)))
            {
                if (pi.PropertyType.IsCollection)
                {
                    WriteCollection(writer, context, pi, value);
                }
                else
                {
                    writer.WriteStartElement(pi.NamespacePrefix, pi.ElementName, pi.Namespace);
                    var type = GetSerializationInfoForInstance(value, true);
                    if (type != pi.PropertyType)
                    {
                        WriteTypeQualifier(writer, type);
                    }
                    Serialize(value, writer, pi, false, pi.IdentificationMode, context);
                    writer.WriteEndElement();
                }
            }
        }

        private void WriteCollection(XmlWriter writer, XmlSerializationContext context, IPropertySerializationInfo pi, object value)
        {
            var collectionType = pi.PropertyType.CollectionItemType;
            foreach (object item in value as IEnumerable)
            {
                writer.WriteStartElement(pi.NamespacePrefix, pi.ElementName, pi.Namespace);
                var itemInfo = GetSerializationInfoForInstance(item, true);
                if (collectionType.IsExplicitTypeInformationRequired(itemInfo))
                {
                    WriteTypeQualifier(writer, itemInfo);
                }
                if (itemInfo.IsStringConvertible)
                {
                    writer.WriteString(itemInfo.ConvertToString(item));
                }
                else
                {
                    Serialize(item, writer, pi, false, pi.IdentificationMode, context);
                }
                writer.WriteEndElement();
            }
        }

        private void WriteTypeQualifier(XmlWriter writer, ITypeSerializationInfo type)
        {
            if (type.NamespacePrefix != null)
            {
                if (writer.LookupPrefix(type.Namespace) != type.NamespacePrefix)
                {
                    writer.WriteAttributeString("xmlns", type.NamespacePrefix, null, type.Namespace);
                }
                 writer.WriteAttributeString(XMLSchemaInstancePrefix, TypeField, XMLSchemaInstanceNamespace,
                     type.NamespacePrefix + ":" + type.ElementName);
            }
            else
            {
                writer.WriteStartAttribute(XMLSchemaInstancePrefix, TypeField, XMLSchemaInstanceNamespace);
                writer.WriteQualifiedName(type.ElementName, type.Namespace);
                writer.WriteEndAttribute();
            }
        }

        /// <inheritdoc />
        protected override void InitializeElementProperties(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            int currentDepth = reader.Depth;
            var propertiesInitialized = (List<IPropertySerializationInfo>)null;
            while (reader.Depth < currentDepth || reader.Read())
            {
                if (reader.Depth == currentDepth)
                {
                    break;
                }
                else if (reader.Depth < currentDepth)
                {
                    return;
                }
                ProcessElement(reader, obj, info, context, ref propertiesInitialized);
            }
        }

        private void ProcessElement(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context, ref List<IPropertySerializationInfo> propertiesInitialized)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                var found = TryInitializeElementProperty(reader, obj, context, info.ElementProperties, ref propertiesInitialized);
                if (!found && !(Settings.ResolveMissingAttributesAsElements && TryInitializeElementProperty(reader, obj, context, info.AttributeProperties, ref propertiesInitialized)))
                {
                    base.OnUnknownElement(new UnknownElementEventArgs(obj, reader.ReadOuterXml()));
                }
            }
            else if ((reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA))
            {
                if (info.DefaultProperty == null)
                {
                    throw new InvalidOperationException("Simple content unexpected for type " + info.ToString());
                }
                InitializePropertyFromText(info.DefaultProperty, obj, reader.Value, context);
            }
        }

        private bool TryInitializeElementProperty(XmlReader reader, object obj, XmlSerializationContext context, IEnumerable<IPropertySerializationInfo> candidates, ref List<IPropertySerializationInfo> propertiesInitialized)
        {
            var property = candidates.FirstOrDefault(p => IsPropertyElement(reader, p));
            if (property != null)
            {
                if (property.RequiresInitialization)
                {
                    propertiesInitialized ??= new List<IPropertySerializationInfo>();
                    if (!propertiesInitialized.Contains(property))
                    {
                        property.Initialize(obj, context);
                        propertiesInitialized.Add(property);
                    }
                }

                ReadElementFromProperty(reader, obj, context, property);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool OverrideIdentifiedObject(object obj, XmlReader reader, XmlSerializationContext context)
        {
            return false;
        }

        /// <inheritdoc />
        protected override void HandleUnknownAttribute(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (reader.NamespaceURI == XmiArtificialIdAttribute.Instance.Namespace && reader.LocalName == XmiArtificialIdAttribute.Instance.ElementName)
            {
                XmiArtificialIdAttribute.Instance.SetValue(obj, reader.Value, context);
            }
            else
            {
                base.HandleUnknownAttribute(reader, obj, info, context);
            }
        }

        private void ReadElementFromProperty(XmlReader reader, object obj, XmlSerializationContext context, IPropertySerializationInfo p)
        {
            var href = reader.GetAttribute("href");
            if (href == null)
            {
                ReadElementFromPropertyCore(reader, obj, context, p);
            }
            else
            {
                if (p.PropertyType.IsCollection)
                {
                    CreateAddToPropertyDelay(p, obj, href, context);
                }
                else
                {
                    CreateSetPropertyDelay(p, obj, href, context);
                }
            }
        }

        private void ReadElementFromPropertyCore(XmlReader reader, object obj, XmlSerializationContext context, IPropertySerializationInfo p)
        {
            if (p.PropertyType.IsStringConvertible || (p.PropertyType.IsCollection && p.PropertyType.CollectionItemType.IsStringConvertible))
            {
                string content = reader.ReadElementContentAsString();
                if (p.PropertyType.IsCollection)
                {
                    p.AddToCollection(obj, p.PropertyType.CollectionItemType.ConvertFromString(content), context);
                }
                else
                {
                    p.SetValue(obj, ConvertString(content, p, context), context);
                }
            }
            else
            {
                object current = DeserializeInternal(reader, p, context);
                if (p.PropertyType.IsCollection)
                {
                    p.AddToCollection(obj, current, context);
                }
                else
                {
                    p.SetValue(obj, current, context);
                }
            }
        }

        /// <inheritdoc />
        protected override void InitializeTypeSerializationInfo(Type type, ITypeSerializationInfo serializationInfo)
        {
            base.InitializeTypeSerializationInfo(type, serializationInfo);

            if (!serializationInfo.IsIdentified && !serializationInfo.IsCollection && serializationInfo is XmlTypeSerializationInfo)
            {
                var id = IdAttribute;
                if (id != null && serializationInfo is XmlTypeSerializationInfo info)
                {
                    if (!info.AttributeProperties.Contains(id))
                    {
                        info.DeclaredAttributeProperties.Add(id);
                    }
                    info.IdentifierProperty = id;
                }
            }
        }

        /// <summary>
        /// Gets the attribute used for identifiers
        /// </summary>
        protected virtual IPropertySerializationInfo IdAttribute
        {
            get
            {
                return XmiArtificialIdAttribute.Instance;
            }
        }
    }
}
