using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NMF.Utilities;

namespace NMF.Serialization.Xmi
{
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

        private static readonly string TypeField = "type";
        public static readonly string XMLSchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        public static readonly string XMLSchemaInstancePrefix = "xsi";

        public static readonly string XMIPrefix = "xmi";
        public static readonly string XMINamespace = "http://www.omg.org/XMI";

        public string RootPrefix { get; set; }

        protected override bool GoToPropertyContent(System.Xml.XmlReader reader)
        {
            //do not move forward
            return true;
        }

        protected override XmlSerializationContext CreateSerializationContext(object root)
        {
            return new XmiSerializationContext(root);
        }

        protected override ITypeSerializationInfo GetElementTypeInfo(XmlReader reader, IPropertySerializationInfo property)
        {
            string attr = reader.GetAttribute(TypeField, XMLSchemaInstanceNamespace);
            if (attr != null)
            {
                int separator = attr.IndexOf(':');
                if (separator == -1) return GetTypeInfo(string.Empty, attr);
                var prefix = attr.Substring(0, separator);
                var localName = attr.Substring(separator + 1);
                return GetTypeInfo(reader.LookupNamespace(prefix), localName);
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

        protected override void WriteBeginElement(System.Xml.XmlWriter writer, object obj, ITypeSerializationInfo info)
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

        protected override void WriteEndElement(System.Xml.XmlWriter writer, object obj, ITypeSerializationInfo info)
        {
        }

        protected override void WriteBeginRootElement(System.Xml.XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            writer.WriteStartElement(info.NamespacePrefix ?? RootPrefix, info.ElementName, info.Namespace);

            writer.WriteAttributeString(XMIPrefix, "version", XMINamespace, "2.0");
            writer.WriteAttributeString("xmlns", XMLSchemaInstancePrefix, null, XMLSchemaInstanceNamespace);
        }

        protected override void WriteEndRootElement(System.Xml.XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            writer.WriteEndElement();
        }

        protected override bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            return false;
        }

        protected override void WriteElementProperties(System.Xml.XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            foreach (XmlPropertySerializationInfo pi in info.ElementProperties)
            {
                var value = pi.GetValue(obj, context);
                if (pi.ShouldSerializeValue(obj, value))
                {
                    if (pi.PropertyType.IsCollection)
                    {
                        var collectionType = pi.PropertyType.CollectionItemType.Type;
                        foreach (object item in value as IEnumerable)
                        {
                            writer.WriteStartElement(pi.NamespacePrefix, pi.ElementName, pi.Namespace);
                            var itemType = item.GetType();
                            if (itemType != collectionType)
                            {
                                var itemInfo = GetSerializationInfo(itemType, true);

                                WriteTypeQualifier(writer, itemInfo);
                            }
                            Serialize(item, writer, pi, false, pi.IdentificationMode, context);
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        writer.WriteStartElement(pi.NamespacePrefix, pi.ElementName, pi.Namespace);
                        if (value != null)
                        {
                            var type = value.GetType();
                            if (type != pi.PropertyType.Type.GetImplementationType())
                            {
                                WriteTypeQualifier(writer, GetSerializationInfo(type, true));
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        Serialize(value, writer, pi, false, pi.IdentificationMode, context);
                        writer.WriteEndElement();
                    }
                }
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

        protected override void InitializeElementProperties(System.Xml.XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            int currentDepth = reader.Depth;
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
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var found = false;
                    foreach (IPropertySerializationInfo p in info.ElementProperties)
                    {
                        if (IsPropertyElement(reader, p))
                        {
                            ReadElementFromProperty(reader, obj, context, p);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        foreach (IPropertySerializationInfo p in info.AttributeProperties)
                        {
                            if (IsPropertyElement(reader, p))
                            {
                                ReadElementFromProperty(reader, obj, context, p);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            OnUnknownElement(new UnknownElementEventArgs(obj, reader.ReadOuterXml()));
                        }
                    }
                }
            }
        }

        protected override bool OverrideIdentifiedObject(object obj, XmlReader reader, XmlSerializationContext context)
        {
            return false;
        }

        protected virtual void OnUnknownElement(UnknownElementEventArgs e)
        {
            if (UnknownElement != null) UnknownElement(this, e);
        }

        public event EventHandler<UnknownElementEventArgs> UnknownElement;

        private void ReadElementFromProperty(XmlReader reader, object obj, XmlSerializationContext context, IPropertySerializationInfo p)
        {
            var href = reader.GetAttribute("href");
            if (href == null)
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
                        p.SetValue(obj, p.ConvertFromString(content), context);
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
            else
            {
                if (p.PropertyType.IsCollection)
                {
                    EnqueueAddToPropertyDelay(p, obj, href, context);
                }
                else
                {
                    EnqueueSetPropertyDelay(p, obj, href, context);
                }
            }
        }

        protected override void InitializeTypeSerializationInfo(Type type, ITypeSerializationInfo serializationInfo)
        {
            base.InitializeTypeSerializationInfo(type, serializationInfo);

            if (!serializationInfo.IsIdentified && !serializationInfo.IsCollection && serializationInfo is XmlTypeSerializationInfo)
            {
                XmlTypeSerializationInfo info = serializationInfo as XmlTypeSerializationInfo;
                var id = IdAttribute;
                if (id != null && info != null)
                {
                    info.AttributeProperties.Add(id);
                    info.IdentifierProperty = id;
                }
            }
        }

        protected virtual IPropertySerializationInfo IdAttribute
        {
            get
            {
                return XmiArtificialIdAttribute.Instance;
            }
        }
    }
}
