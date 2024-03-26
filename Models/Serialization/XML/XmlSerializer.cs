using System.Reflection;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml;
using System.Text;

namespace NMF.Serialization
{

    /// <summary>
    /// Class to serialize objects in a Xml-format.
    /// </summary>
    public class XmlSerializer : Serializer
    {
        /// <summary>
        /// Creates a new XmlSerializer with default settings and no preloaded types
        /// </summary>
        public XmlSerializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new XmlSerializer with default settings
        /// </summary>
        /// <param name="additionalTypes">Set of types to preload into the serializer</param>
        /// <remarks>Types will be loaded with default settings</remarks>
        public XmlSerializer(IEnumerable<Type> additionalTypes) : this(XmlSerializationSettings.Default, additionalTypes) { }

        /// <summary>
        /// Creates a new XmlSerializer with the specified settings
        /// </summary>
        /// <param name="settings">Serializer-settings for the serializer. Can be null or Nothing in Visual Basic. In this case, the default settings will be used.</param>
        public XmlSerializer(XmlSerializationSettings settings) : this(settings, null)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer with the specified settings and the given preloaded types
        /// </summary>
        /// <param name="additionalTypes">Set of types to load into the serializer</param>
        /// <param name="settings">The settings to use for the serializer</param>
        /// <remarks>The types will be loaded with the specified settings</remarks>
        public XmlSerializer(XmlSerializationSettings settings, IEnumerable<Type> additionalTypes) : this(null, settings, additionalTypes)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public XmlSerializer(XmlSerializer parent) : this(parent, null) { }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public XmlSerializer(XmlSerializer parent, XmlSerializationSettings settings) : this(parent, settings, null)
        {
        }

        /// <summary>
        /// Creates a new XmlSerializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <param name="additionalTypes">Set of types to load into the serializer</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public XmlSerializer(XmlSerializer parent, XmlSerializationSettings settings, IEnumerable<Type> additionalTypes)
            : base(parent, settings, additionalTypes) { }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="path">The path for the resulting Xml-file</param>
        /// <param name="obj">The object to be serialized</param>
        /// <param name="fragment">A value that indicates whether the serializer should write a document definition</param>
        public void Serialize(object obj, string path, bool fragment = false)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                Serialize(obj, fs, fragment);
            }
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="stream">The stream for the resulting Xml-code</param>
        /// <param name="source">The object to be serialized</param>
        /// <param name="fragment">A value that indicates whether the serializer should write a document definition</param>
        public void Serialize(object source, Stream stream, bool fragment = false)
        {
            var sw = new StreamWriter(stream, Encoding.UTF8);
            Serialize(source, sw, fragment);
            sw.Flush();
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="writer">The TextWriter to write the Xml-code on</param>
        /// <param name="source">The object to be serialized</param>
        public void Serialize(object source, TextWriter writer)
        {
            Serialize(source, writer, false);
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="writer">The XmlWriter to write the Xml-code on</param>
        /// <param name="source">The object to be serialized</param>
        public void Serialize(object source, XmlWriter writer)
        {
            Serialize(source, writer, false);
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="target">The TextWriter to write the Xml-code on</param>
        /// <param name="fragment">A value that indicates whether the serializer should write a document definition</param>
        /// <param name="source">The object to be serialized</param>
        public void Serialize(object source, TextWriter target, bool fragment)
        {
            XmlWriter xml = XmlWriter.Create(target, Settings.CreateXmlWriterSettings());
            Serialize(source, xml, fragment);
            xml.Flush();
            xml.Close();
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="target">The XmlWriter to write the Xml-code on</param>
        /// <param name="fragment">A value that indicates whether the serializer should write a document definition</param>
        /// <param name="source">The object to be serialized</param>
        public void Serialize(object source, XmlWriter target, bool fragment)
        {
            if (!fragment) target.WriteStartDocument();
            source = SelectRoot(source, fragment);
            var info = GetSerializationInfoForInstance(source, true);
            WriteBeginRootElement(target, source, info);
            XmlSerializationContext context = CreateSerializationContext(source);
            Serialize(source, target, null, false, XmlIdentificationMode.FullObject, context);
            WriteEndRootElement(target, source, info);
            if (!fragment) target.WriteEndDocument();
        }

        /// <summary>
        /// Serializes the given object
        /// </summary>
        /// <param name="writer">The XmlWriter to write the Xml-code on</param>
        /// <param name="writeInstance">A value that indicates whether the serializer should write the element definition</param>
        /// <param name="obj">The object to be serialized</param>
        /// <param name="property">The property for which the object is serialized</param>
        /// <param name="context">The serialization context</param>
        /// <param name="identificationMode">A value indicating whether it is allowed to the serializer to use identifier</param>
        /// <remarks>If a converter is provided that is able to convert the object to string and convert the string back to this object, just the string-conversion is printed out</remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void Serialize(object obj, XmlWriter writer, IPropertySerializationInfo property, bool writeInstance, XmlIdentificationMode identificationMode, XmlSerializationContext context)
        {
            if (obj == null) return;
            if (property != null && property.IsStringConvertible)
            {
                writer.WriteString(property.ConvertToString(obj));
                return;
            }
            ITypeSerializationInfo info = GetSerializationInfoForInstance(obj, false);
            if (WriteIdentifiedObject(writer, obj, identificationMode, info, context)) return;
            if (writeInstance) WriteBeginElement(writer, obj, info);
            if (info.ConstructorProperties != null)
            {
                WriteConstructorProperties(writer, obj, info, context);
            }
            WriteAttributeProperties(writer, obj, info, context);
            WriteElementProperties(writer, obj, info, context);
            WriteCollectionMembers(writer, obj, info, context);
            if (writeInstance) WriteEndElement(writer, obj, info);
        }

        /// <summary>
        /// Writes the root element to the given writer
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="root">The root element</param>
        /// <param name="info">The serialization information of the objects type</param>
        protected virtual void WriteBeginRootElement(XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            WriteBeginElement(writer, root, info);
        }

        /// <summary>
        /// Writes the beginning of an element
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        protected virtual void WriteBeginElement(XmlWriter writer, object obj, ITypeSerializationInfo info)
        {
            writer.WriteStartElement(info.NamespacePrefix, info.ElementName, info.Namespace);
        }

        /// <summary>
        /// Writes the properties necessary for the constrctor call of this element
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteConstructorProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            for (int i = 0; i <= info.ConstructorProperties.GetUpperBound(0); i++)
            {
                IPropertySerializationInfo pi = info.ConstructorProperties[i];
                writer.WriteAttributeString(pi.NamespacePrefix, pi.ElementName, pi.Namespace, pi.ConvertToString(pi.GetValue(obj, context)));
            }
        }

        /// <summary>
        /// Writes the attribute properties of the given object
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteAttributeProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            foreach (IPropertySerializationInfo pi in info.AttributeProperties)
            {
                var value = pi.GetValue(obj, context);
                if (Settings.SerializeDefaultValues || pi.ShouldSerializeValue(obj, value)) WriteAttributeValue(writer, obj, value, pi, context);
                if (pi.IsIdentifier)
                {
                    string id = CStr(pi.GetValue(obj, context));
                    context.RegisterId(id, obj, pi.PropertyType);
                }
            }
        }

        /// <summary>
        /// Writes the attribute value to the given writer
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="context">The serialization context</param>
        /// <param name="value">The value of the attribute</param>
        /// <param name="property">The property serialization information</param>
        protected virtual void WriteAttributeValue(XmlWriter writer, object obj, object value, IPropertySerializationInfo property, XmlSerializationContext context)
        {
            ITypeSerializationInfo info = property.PropertyType;

            if (value == null) return;

            string valueString = GetAttributeValue(value, property.PropertyType, false, context);

            if (valueString != null)
            {
                writer.WriteStartAttribute(property.NamespacePrefix, property.ElementName, property.Namespace);

                writer.WriteString(valueString);
                writer.WriteEndAttribute();
            }
            else if (info.IsCollection)
            {
                WriteAttributeCollection(writer, value, property, context, info.CollectionItemType);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Property {0} cannot be serialized as string", property.ElementName));
            }
        }

        private void WriteAttributeCollection(XmlWriter writer, object value, IPropertySerializationInfo property, XmlSerializationContext context, ITypeSerializationInfo info)
        {
            var sb = new StringBuilder();
            if (value is IEnumerable enumerable)
            {
                foreach (object o in enumerable)
                {
                    if (o != null)
                    {
                        string str = GetAttributeValue(o, info, true, context);
                        if (str != null)
                        {
                            sb.Append(str);
                            sb.Append(' ');
                        }
                        else
                        {
                            throw new InvalidOperationException(string.Format("Object {0} cannot be serialized as string", o));
                        }
                    }
                }
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            writer.WriteAttributeString(property.NamespacePrefix, property.ElementName, property.Namespace, sb.ToString());
        }

        /// <summary>
        /// Writes the element properties of the given object to the provided writer
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteElementProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            foreach (var pi in info.ElementProperties)
            {
                var value = pi.GetValue(obj, context);
                if (Settings.SerializeDefaultValues || pi.ShouldSerializeValue(obj, value))
                {
                    writer.WriteStartElement(pi.NamespacePrefix, pi.ElementName, pi.Namespace);
                    Serialize(value, writer, pi, pi.ShallCreateInstance, pi.IdentificationMode, context);
                    writer.WriteEndElement();
                }
                if (pi.IsIdentifier)
                {
                    string id = CStr(value);
                    context.RegisterId(id, obj, GetSerializationInfoForInstance(obj, false) ?? info);
                }
            }
        }

        /// <summary>
        /// Writes the elements of the given collection to the provided writer
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        protected virtual void WriteCollectionMembers(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (info.IsCollection)
            {
                IEnumerable coll = obj as IEnumerable;
                foreach (object o in coll)
                {
                    Serialize(o, writer, null, true, XmlIdentificationMode.FullObject, context);
                }
            }
        }

        /// <summary>
        /// Completes the current element for the provided object
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        protected virtual void WriteEndElement(XmlWriter writer, object obj, ITypeSerializationInfo info)
        {
            writer.WriteEndElement();
        }

        /// <summary>
        /// Completes the root element
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="root">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        protected virtual void WriteEndRootElement(XmlWriter writer, object root, ITypeSerializationInfo info)
        {
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the provided identified object
        /// </summary>
        /// <param name="writer">The xml writer to write to</param>
        /// <param name="obj">The element</param>
        /// <param name="info">The serialization information of the objects type</param>
        /// <param name="context">The serialization context</param>
        /// <param name="identificationMode">The identification mode for the current object</param>
        /// <returns>true, if the object could be written as identified object, otherwise false</returns>
        protected virtual bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (!info.IsIdentified) return false;
            string id = CStr(info.IdentifierProperty.GetValue(obj, context));
            if (identificationMode == XmlIdentificationMode.Identifier || (identificationMode == XmlIdentificationMode.AsNeeded && context.ContainsId(id, info)))
            {
                writer.WriteString(id);
                return true;
            }
            else if (identificationMode == XmlIdentificationMode.FullObject && context.ContainsId(id, info))
            {
                writer.WriteStartElement(info.ElementName, info.Namespace);
                if (info.AttributeProperties.Contains(info.IdentifierProperty))
                {
                    writer.WriteAttributeString(info.IdentifierProperty.NamespacePrefix, info.IdentifierProperty.ElementName, info.IdentifierProperty.Namespace,
                        info.IdentifierProperty.ConvertToString(info.IdentifierProperty.GetValue(obj, context)));
                }
                else
                {
                    writer.WriteElementString(info.IdentifierProperty.NamespacePrefix, info.IdentifierProperty.ElementName, info.IdentifierProperty.Namespace,
                        info.IdentifierProperty.ConvertToString(info.IdentifierProperty.GetValue(obj, context)));
                }
                writer.WriteEndElement();
                return true;
            }
            return false;
        }
         
        /// <summary>
        /// Deserializes an Xml-representation of an object back to the corresponding object
        /// </summary>
        /// <param name="path">The path to the Xml file containg the Xml code</param>
        /// <returns>The corresponding object</returns>
        public object Deserialize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Deserialize(fs);
            }
        }

        /// <summary>
        /// Deserializes an Xml-representation of an object back to the corresponding object
        /// </summary>
        /// <param name="stream">The stream containg the Xml code</param>
        /// <returns>The corresponding object</returns>
        public object Deserialize(Stream stream)
        {
            return Deserialize(XmlReader.Create(stream));
        }

        /// <summary>
        /// Deserializes an Xml-representation of an object back to the corresponding object
        /// </summary>
        /// <param name="reader">A TextReader containg the Xml code</param>
        /// <returns>The corresponding object</returns>
        public object Deserialize(TextReader reader)
        {
            return Deserialize(XmlReader.Create(reader));
        }

        /// <summary>
        /// Deserializes an Xml-representation of an object back to the corresponding object
        /// </summary>
        /// <param name="reader">A XmlReader containing the Xml code</param>
        /// <returns>The corresponding object</returns>
        /// <remarks>The function will deserialize the object at the XmlReaders current position</remarks>
        public object Deserialize(XmlReader reader)
        {
            object root = DeserializeRootInternal(reader, out XmlSerializationContext context);
            context.Cleanup();
            return root;
        }
        
        internal object DeserializeInternal(XmlReader reader, IPropertySerializationInfo property, XmlSerializationContext context)
        {
            while (reader.NodeType != XmlNodeType.Element) reader.Read();
            var propertyType = GetElementTypeInfo(reader, property);
            if (propertyType == null) throw new InvalidOperationException($"No information available what the type of {reader.LocalName} is.");
            object deserialized = CreateObject(reader, propertyType, context);
            Initialize(reader, deserialized, context);
            return deserialized;
        }

        internal object DeserializeRootInternal(XmlReader reader, out XmlSerializationContext context)
        {
            object root = CreateRoot(reader);

            context = CreateSerializationContext(root);

            Initialize(reader, root, context);
            return root;
        }

        /// <summary>
        /// Creates the root element
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns>The root element</returns>
        protected object CreateRoot(XmlReader reader)
        {
            while (reader.NodeType != XmlNodeType.Element) reader.Read();
            var rootInfo = GetRootElementTypeInfo(reader);
            object root = CreateObject(reader, rootInfo, null);
            return root;
        }

        /// <summary>
        /// Gets the type information for the current property
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <param name="property">The current property</param>
        /// <returns>The type serialization info that should be used in the remainder</returns>
        protected virtual ITypeSerializationInfo GetElementTypeInfo(XmlReader reader, IPropertySerializationInfo property)
        {
            return GetRootElementTypeInfo(reader);
        }

        /// <summary>
        /// Gets the type information for the root element
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns>The type serialization info for the root element</returns>
        protected virtual ITypeSerializationInfo GetRootElementTypeInfo(XmlReader reader)
        {
            var info = GetTypeInfo(reader.NamespaceURI, reader.LocalName) ?? HandleUnknownType(null, reader.NamespaceURI, reader.LocalName);

            if (info != null)
            {
                return info;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not identify element of type {0} in namespace {1}", reader.LocalName, reader.NamespaceURI));
            }
        }

        private static readonly object[] emptyObjects = { };

        /// <summary>
        /// Creates the object for the current position
        /// </summary>
        /// <param name="reader">the xml reader</param>
        /// <param name="tsi">the type serialization information</param>
        /// <param name="context">the serialization context</param>
        /// <returns>the deserialized object</returns>
        protected virtual object CreateObject(XmlReader reader, ITypeSerializationInfo tsi, XmlSerializationContext context)
        {
            if (tsi.ConstructorProperties == null)
            {
                return tsi.CreateObject(emptyObjects);
            }
            else
            {
                object[] objects = new object[tsi.ConstructorProperties.Length];
                for (int i = 0; i < tsi.ConstructorProperties.Length; i++)
                {
                    IPropertySerializationInfo pi = tsi.ConstructorProperties[i];
                    objects[i] = pi.ConvertFromString(reader.GetAttribute(pi.ElementName, pi.Namespace));
                }
                return tsi.CreateObject(objects);
            }
        }

        /// <summary>
        /// Initialized the property from the reader
        /// </summary>
        /// <param name="reader">the xml reader</param>
        /// <param name="property">the property</param>
        /// <param name="obj">the object</param>
        /// <param name="context">the serialization context</param>
        /// <returns>true, if the initialization was successful, otherwise false</returns>
        protected virtual bool InitializeProperty(XmlReader reader, IPropertySerializationInfo property, object obj, XmlSerializationContext context)
        {
            if (!GoToPropertyContent(reader)) return false;
            if (reader.NodeType == XmlNodeType.Text)
            {
                InitializePropertyFromText(property, obj, reader.Value, context);
            }
            else if (reader.NodeType != XmlNodeType.EndElement)
            {
                object target = DeserializeInternal(reader, property, context);
                if (!property.IsReadOnly && (target == null || property.PropertyType.IsInstanceOf(target)))
                {
                    property.SetValue(obj, target, context);
                }
                else if (property.PropertyType.IsCollection)
                {
                    object collection = property.GetValue(obj, context);
                    property.AddToCollection(collection, target, context);
                }
            }
            else
            {
                //do nothing
            }
            return true;
        }

        /// <summary>
        /// Moves the reader to the content of the property
        /// </summary>
        /// <param name="reader">the Xml reader</param>
        /// <returns>true, if the reader could be moved sucessfully, otherwise false</returns>
        protected virtual bool GoToPropertyContent(XmlReader reader)
        {
            int currentDepth = reader.Depth;
            reader.Read();
            if (reader.Depth <= currentDepth) return false;
            return true;
        }

        /// <summary>
        /// Initializes the given property from the provided text
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="obj">The object</param>
        /// <param name="text">The input text</param>
        /// <param name="context">The serialization context</param>
        protected virtual void InitializePropertyFromText(IPropertySerializationInfo property, object obj, string text, XmlSerializationContext context)
        {
            ITypeSerializationInfo info = property.PropertyType;
            if (property.IsStringConvertible)
            {
                property.SetValue(obj, property.ConvertFromString(text), context);
            }
            else if (info.IsCollection)
            {
                ITypeSerializationInfo itemInfo = info.CollectionItemType;
                var coll = property.GetValue(obj, context);
                if (coll is IList list && !context.IsBlocked(obj, property))
                {
                    list.Clear();
                }
                foreach (var item in text.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (itemInfo.IsStringConvertible)
                    {
                        property.AddToCollection(obj, itemInfo.ConvertFromString(item), context);
                    }
                    else
                    {
                        EnqueueAddToPropertyDelay(property, obj, item, context);
                    }
                }
            }
            else
            {
                EnqueueSetPropertyDelay(property, obj, text, context);
            }
        }

        internal void EnqueueAddToPropertyDelay(IPropertySerializationInfo property, object obj, string text, XmlSerializationContext context)
        {
            context.LostProperties.Enqueue(new XmlAddToPropertyDelay(property) { Target = obj, Identifier = text });
        }

        internal void EnqueueSetPropertyDelay(IPropertySerializationInfo property, object obj, string text, XmlSerializationContext context)
        {
            context.LostProperties.Enqueue(new XmlSetPropertyDelay() { Identifier = text, Target = obj, Property = property });
        }

        /// <summary>
        /// Initializes the given object with the xml code at the current position of the XmlReader
        /// </summary>
        /// <param name="reader">The XmlReader with the Xml code</param>
        /// <param name="obj">The object to initialize</param>
        /// <param name="context">The serialization context</param>
        /// <returns>The initialized object</returns>
        public void Initialize(XmlReader reader, object obj, XmlSerializationContext context)
        {
            if (obj == null) return;
            if (obj is ISupportInitialize initialize) initialize.BeginInit();
            ITypeSerializationInfo info = GetSerializationInfoForInstance(obj, false);
            if (reader.HasAttributes)
            {
                if (info.IsIdentified && info.AttributeProperties.Contains(info.IdentifierProperty))
                {
                    obj = RegisterOrReplace(reader, obj, context, info);
                }
                InitializeAttributeProperties(reader, obj, info, context);
            }
            InitializeElementProperties(reader, ref obj, info, context);
            if (obj is ISupportInitialize init) context.Inits.Enqueue(init);
        }

        private object RegisterOrReplace(XmlReader reader, object obj, XmlSerializationContext context, ITypeSerializationInfo info)
        {
            IPropertySerializationInfo p = info.IdentifierProperty;
            var idValue = reader.GetAttribute(p.ElementName, p.Namespace);
            if (idValue != null)
            {
                string id = CStr(p.ConvertFromString(idValue));
                if (!string.IsNullOrEmpty(id))
                {
                    if (OverrideIdentifiedObject(obj, reader, context))
                    {
                        obj = RegisterOrReplace(obj, context, info, id);
                    }
                    else
                    {
                        context.RegisterId(id, obj, info);
                    }
                }
            }

            return obj;
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
        protected virtual bool OverrideIdentifiedObject(object obj, XmlReader reader, XmlSerializationContext context)
        {
            return true;
        }

        /// <summary>
        /// Initializes the element properties from the xml reader position
        /// </summary>
        /// <param name="reader">the xml reader</param>
        /// <param name="obj">the element</param>
        /// <param name="info">the type serialization information</param>
        /// <param name="context">the serialization context</param>
        protected virtual void InitializeElementProperties(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
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
                    HandleElement(reader, ref obj, info, context);
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
        }

        private void HandleElement(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            bool found = TryInitializeElement(reader, ref obj, info, context);
            if (!found && info.IsCollection)
            {
                object o = DeserializeInternal(reader, null, context);
                info.AddToCollection(obj, o);
            }
            else
            {
                HandleUnknownElement(reader, obj, info, context);
            }
        }

        /// <summary>
        /// Handles an attribute that was not known to the serializer
        /// </summary>
        /// <param name="reader">The current reader position</param>
        /// <param name="obj">The object that is currently deserialized</param>
        /// <param name="info">The type serialization information of the object</param>
        /// <param name="context">The serialization context</param>
        protected virtual void HandleUnknownAttribute(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var e = new UnknownAttributeEventArgs(obj, reader.NamespaceURI, reader.LocalName, reader.Value);
            OnUnknownAttribute(e);
        }

        /// <summary>
        /// Handles an element that was not known to the serializer
        /// </summary>
        /// <param name="reader">The current reader position</param>
        /// <param name="obj">The object that is currently deserialized</param>
        /// <param name="info">The type serialization information of the object</param>
        /// <param name="context">The serialization context</param>
        protected virtual void HandleUnknownElement(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            OnUnknownElement(new UnknownElementEventArgs(obj, reader.ReadOuterXml()));
        }

        private bool TryInitializeElement(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var property = info.ElementProperties.FirstOrDefault(p => IsPropertyElement(reader, p));
            if (property != null) 
            {
                if (property.ShallCreateInstance)
                {
                    if (!InitializeProperty(reader, property, obj, context))
                    {
                        return true;
                    }
                }
                else
                {
                    Initialize(reader, property.GetValue(obj, context), context);
                }

                if (property.IsIdentifier)
                {
                    string str = CStr(property.GetValue(obj, context));
                    if (!string.IsNullOrEmpty(str))
                    {
                        obj = RegisterOrReplace(obj, context, info, str);
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the element at the current reader position refers to the given property
        /// </summary>
        /// <param name="reader">The reader position</param>
        /// <param name="property">The property that should be tested</param>
        /// <returns>true, if the element is about the property, otherwise false</returns>
        protected virtual bool IsPropertyElement(XmlReader reader, IPropertySerializationInfo property)
        {
            return Settings.TreatAsEqual(reader.NamespaceURI, property.Namespace) && Settings.TreatAsEqual(reader.LocalName, property.ElementName);
        }

        /// <summary>
        /// Initializes the attribute properties from the current reader position
        /// </summary>
        /// <param name="reader">the xml reader</param>
        /// <param name="obj">the object</param>
        /// <param name="info">the type serialization information</param>
        /// <param name="context">the serialization context</param>
        protected virtual void InitializeAttributeProperties(XmlReader reader, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var cont = reader.MoveToFirstAttribute();
            while (cont)
            {
                var foundAttribute = false;
                foreach (IPropertySerializationInfo p in info.AttributeProperties)
                {
                    if (IsPropertyElement(reader, p))
                    {
                        InitializePropertyFromText(p, obj, reader.Value, context);
                        foundAttribute = true;
                        break;
                    }
                }
                if (!foundAttribute && Settings.ResolveMissingAttributesAsElements)
                {
                    foreach (var p in info.ElementProperties)
                    {
                        if (IsPropertyElement(reader, p))
                        {
                            InitializePropertyFromText(p, obj, reader.Value, context);
                            foundAttribute = true;
                            break;
                        }
                    }
                }
                if (!foundAttribute)
                {
                    HandleUnknownAttribute(reader, obj, info, context);
                }
                cont = reader.MoveToNextAttribute();
            }
            reader.MoveToElement();
        }
    }


}