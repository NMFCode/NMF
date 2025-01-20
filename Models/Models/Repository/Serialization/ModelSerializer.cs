using NMF.Serialization;
using NMF.Serialization.Xmi;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace NMF.Models.Repository.Serialization
{
    /// <summary>
    /// Denotes the standard model serializer
    /// </summary>
    public class ModelSerializer : XmiSerializer, IModelSerializer
    {
        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        public ModelSerializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        public ModelSerializer(XmlSerializationSettings settings) : this(settings, null) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public ModelSerializer(XmlSerializer parent) : base(parent) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public ModelSerializer(XmlSerializer parent, XmlSerializationSettings settings) : base(parent, settings) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="knownTypes">A collection of known types</param>
        public ModelSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes)
            : base(settings, knownTypes)
        {
        }

        /// <inheritdoc />
        protected override void InitializeElementProperties(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (obj is not Model model)
            {
                base.InitializeElementProperties(reader, ref obj, info, context);
            }
            else
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
                        AddRootElement(reader, context, model);
                    }
                }
            }
        }

        private void AddRootElement(XmlReader reader, XmlSerializationContext context, Model model)
        {
            var element = CreateRoot(reader) as IModelElement;
            Initialize(reader, element, context);
            if (element != null)
            {
                model.RootElements.Add(element);
            }
        }

        /// <inheritdoc />
        protected override void WriteElementProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (obj is not Model model)
            {
                base.WriteElementProperties(writer, obj, info, context);
            }
            else
            {
                foreach (var element in model.RootElements)
                {
                    if (element == null) continue;
                    var typeInfo = GetSerializationInfo(element.GetType(), true);
                    writer.WriteStartElement(typeInfo.NamespacePrefix ?? RootPrefix, typeInfo.ElementName, typeInfo.Namespace);
                    Serialize(element, writer, null, false, XmlIdentificationMode.FullObject, context);
                    writer.WriteEndElement();
                }
            }
        }

        /// <inheritdoc />
        protected override XmlSerializationContext CreateSerializationContext(object root)
        {
            var tempRepository = new ModelRepository();
            tempRepository.Locators.Add(FileLocator.Instance);
            tempRepository.Serializer = this;
            return new ModelSerializationContext(tempRepository, CreateModelForRoot(root));
        }

        /// <inheritdoc />
        protected override bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (identificationMode == XmlIdentificationMode.Identifier && context.Root is Model model && obj is IModelElement modelElement)
            {
                var uri = model.CreateUriForElement(modelElement);
                if (uri != null)
                {
                    writer.WriteString(uri.ConvertToString());
                    return true;
                }
            }
            return base.WriteIdentifiedObject(writer, obj, identificationMode, info, context);
        }

        /// <inheritdoc />
        public override void Serialize(object obj, XmlWriter writer, IPropertySerializationInfo property, bool writeInstance, XmlIdentificationMode identificationMode, XmlSerializationContext context)
        {
            var useBaseSerialization = true;
            if (obj is IModelElement modelElement && context is ModelSerializationContext modelSerializationContext && modelSerializationContext.Model != null)
            {
                useBaseSerialization = !modelSerializationContext.Model.SerializeAsReference(modelElement);
            }
            if (useBaseSerialization)
            {
                base.Serialize(obj, writer, property, writeInstance, identificationMode, context);
            }
            else
            {
                writer.WriteStartAttribute("href");
                WriteIdentifiedObject(writer, obj, XmlIdentificationMode.Identifier, GetSerializationInfo(obj.GetType(), false), context);
                writer.WriteEndAttribute();
            }
        }

        /// <inheritdoc />
        protected override string GetAttributeValue(object value, ITypeSerializationInfo info, bool isCollection, XmlSerializationContext context)
        {
            if (value is ModelElement modelElement && context.Root is Model model)
            {
                Uri uri = model.CreateUriForElement(modelElement);
                if (uri != null)
                {
                    if (isCollection || GetSerializationInfoForInstance(modelElement, true) == info)
                    {
                        return uri.ConvertToString();
                    }
                    else
                    {
                        return ElementUriAndType(modelElement, uri);
                    }
                }
            }
            return base.GetAttributeValue(value, info, isCollection, context);
        }

        private string ElementUriAndType(ModelElement modelElement, Uri uri)
        {
            // for EMF compatibility reasons, we need to render the type of a single-valued reference as well
            var concreteType = GetSerializationInfo(modelElement.GetType(), true);
            var sb = new StringBuilder();
            if (concreteType.NamespacePrefix != null)
            {
                sb.Append(concreteType.NamespacePrefix);
                sb.Append(':');
            }
            sb.Append(concreteType.ElementName);
            sb.Append(' ');
            sb.Append(uri.ConvertToString());
            return sb.ToString();
        }

        /// <inheritdoc />
        protected override bool IsPropertyElement(XmlReader reader, IPropertySerializationInfo property)
        {
            if (string.IsNullOrEmpty(reader.Prefix) && string.IsNullOrEmpty(property.Namespace) && reader.LocalName == property.ElementName)
            {
                return true;
            }
            return base.IsPropertyElement(reader, property);
        }

        /// <summary>
        /// Creates the model for the given root element
        /// </summary>
        /// <param name="root">The root element</param>
        /// <returns>The model instance</returns>
        /// <exception cref="InvalidOperationException">Thrown if root is not a model element</exception>
        protected virtual Model CreateModelForRoot(object root)
        {
            if (root is ModelElement modelElement)
            {
                var model = modelElement.Model;
                if (model == null)
                {
                    model = new Model();
                    model.RootElements.Add(modelElement);
                }
                return model;
            }
            else
            {
                throw new InvalidOperationException("The deserialized object graph is not a valid model.");
            }
        }

        /// <inheritdoc />
        public Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository)
        {
            var reader = XmlReader.Create(source);
            var root = CreateRoot(reader);

            var model = CreateModelForRoot(root);
            model.ModelUri = modelUri;
            if (addToRepository) repository.Models.Add(modelUri, model);
            var context = new ModelSerializationContext(repository, model);

            Initialize(reader, root, context);
            
            foreach (var pair in context.IDs)
            {
                if (pair.Value is ModelElement modelElement)
                {
                    model.RegisterId(pair.Key, modelElement);
                }
            }

            context.Cleanup();

            return model;
        }

        /// <summary>
        /// Gets the serialization root element
        /// </summary>
        /// <param name="graph">The base element that should be serialized</param>
        /// <param name="fragment">A value indicating whether only a fragment should be written</param>
        /// <returns>The root element for serialization</returns>
        protected override object SelectRoot(object graph, bool fragment)
        {
            if (fragment) return graph;
            if (graph is IModelElement modelElement)
            {
                var model = modelElement.Model;
                if (model == null) return graph;
                if (model.RootElements.Count == 1 && Model.PromoteSingleRootElement) return model.RootElements[0];
                return model;
            }
            else
            {
                return graph;
            }
        }

        /// <inheritdoc />
        protected override IPropertySerializationInfo IdAttribute
        {
            get
            {
                return ModelIdAttribute.instance;
            }
        }

        /// <inheritdoc />
        public void Serialize(Model model, Stream target)
        {
            base.Serialize(model, target, false);
        }

        /// <inheritdoc />
        public void SerializeFragment(ModelElement element, Stream target)
        {
            base.Serialize(element, target, true);
        }
    }
}
