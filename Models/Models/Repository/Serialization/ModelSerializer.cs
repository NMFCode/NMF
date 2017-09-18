﻿using NMF.Models.Changes;
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
    public class ModelSerializer : XmiSerializer, IModelSerializer
    {
        public ModelSerializer() : this(XmlSerializationSettings.Default) { }

        public ModelSerializer(XmlSerializationSettings settings) : this(settings, null) { }

        public ModelSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes)
            : base(settings, knownTypes)
        {

        }

        protected override void InitializeElementProperties(XmlReader reader, ref object obj, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var model = obj as Model;
            if (model == null)
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
            var model = obj as Model;
            if (model == null)
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
                var model = context.Root as Model;
                if (model != null)
                {
                    var modelElement = obj as IModelElement;
                    if (modelElement != null)
                    {
                        Uri uri = model.CreateUriForElement(modelElement);
                        if (uri != null)
                        {
                            writer.WriteString(uri.ConvertToString());
                            return true;
                        }
                    }
                }
            }
            return base.WriteIdentifiedObject(writer, obj, identificationMode, info, context);
        }

        public override void Serialize(object obj, XmlWriter writer, IPropertySerializationInfo property, bool writeInstance, XmlIdentificationMode identificationMode, XmlSerializationContext context)
        {
            var modelElement = obj as IModelElement;
            var useBaseSerialization = true;
            if (modelElement != null)
            {
                var modelSerializationContext = context as ModelSerializationContext;
                if (modelSerializationContext != null && modelSerializationContext.Model != null)
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

        protected override string GetAttributeValue(object value, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var model = context.Root as Model;
            var modelElement = value as ModelElement;
            if (modelElement != null && model != null)
            {
                Uri uri = model.CreateUriForElement(modelElement);
                if (uri != null)
                {
                    return uri.ConvertToString();
                }
            }
            return base.GetAttributeValue(value, info, context);
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
            var modelElement = root as ModelElement;
            if (modelElement != null)
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
                var modelElement = pair.Value as ModelElement;
                if (modelElement != null)
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
            var modelElement = graph as IModelElement;
            if (modelElement != null)
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
