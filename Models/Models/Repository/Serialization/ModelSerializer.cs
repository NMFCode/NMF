using NMF.Models.Changes;
using NMF.Serialization;
using NMF.Serialization.Xmi;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ModelSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes)
            : base(settings, knownTypes)
        {
        }

        protected override void InitializeElementProperties(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (!(obj is Model model))
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
                        var element = CreateRoot(reader) as IModelElement;
                        Initialize(reader, element, context);
                        if (element != null)
                        {
                            model.RootElements.Add(element);
                        }
                    }
                }
            }
        }

        protected override void WriteElementProperties(XmlWriter writer, object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (!(obj is Model model))
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

        protected override XmlSerializationContext CreateSerializationContext(object root)
        {
            var tempRepository = new ModelRepository();
            tempRepository.Locators.Add(FileLocator.Instance);
            tempRepository.Serializer = this;
            return new ModelSerializationContext(tempRepository, CreateModelForRoot(root));
        }

        protected override bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            if (identificationMode == XmlIdentificationMode.Identifier)
            {
                if (context.Root is Model model && obj is IModelElement modelElement)
                {
                    var uri = model.CreateUriForElement(modelElement);
                    if (uri != null)
                    {
                        writer.WriteString(uri.ConvertToString());
                        return true;
                    }
                }
            }
            return base.WriteIdentifiedObject(writer, obj, identificationMode, info, context);
        }

        public override void Serialize(object obj, XmlWriter writer, IPropertySerializationInfo property, bool writeInstance, XmlIdentificationMode identificationMode, XmlSerializationContext context)
        {
            var useBaseSerialization = true;
            if (obj is IModelElement modelElement)
            {
                if (context is ModelSerializationContext modelSerializationContext && modelSerializationContext.Model != null)
                {
                    useBaseSerialization = !modelSerializationContext.Model.SerializeAsReference(modelElement);
                }
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
                        // for EMF compatibility reasons, we need to render the type of a single-valued reference as well
                        var concreteType = GetSerializationInfo(modelElement.GetType(), true);
                        var sb = new StringBuilder();
                        if (concreteType.NamespacePrefix != null)
                        {
                            sb.Append(concreteType.NamespacePrefix);
                            sb.Append(":");
                        }
                        sb.Append(concreteType.ElementName);
                        sb.Append(" ");
                        sb.Append(uri.ConvertToString());
                        return sb.ToString();
                    }
                }
            }
            return base.GetAttributeValue(value, info, isCollection, context);
        }

        protected override bool IsPropertyElement(XmlReader reader, IPropertySerializationInfo p)
        {
            if (string.IsNullOrEmpty(reader.Prefix))
            {
                if (string.IsNullOrEmpty(p.Namespace) && reader.LocalName == p.ElementName)
                {
                    return true;
                }
            }
            return base.IsPropertyElement(reader, p);
        }

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

        protected override IPropertySerializationInfo IdAttribute
        {
            get
            {
                return ModelIdAttribute.instance;
            }
        }

        public void Serialize(Model model, Stream target)
        {
            base.Serialize(model, target, false);
        }

        public void SerializeFragment(ModelElement element, Stream target)
        {
            base.Serialize(element, target, true);
        }
    }
}
