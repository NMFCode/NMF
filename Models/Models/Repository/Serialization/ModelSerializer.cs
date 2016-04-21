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
                    WriteBeginRootElement(writer, element, typeInfo);
                    Serialize(element, writer, null, false, XmlIdentificationMode.FullObject, context);
                    WriteEndRootElement(writer, element, typeInfo);
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
                        Uri uri;
                        if (modelElement.Model == model)
                        {
                            uri = modelElement.RelativeUri;
                        }
                        else
                        {
                            uri = MakeShortUri(modelElement.AbsoluteUri, model.ModelUri);
                        }
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

        private Uri MakeShortUri(Uri target, Uri current)
        {
            if (target == null) return null;
            if (!target.IsFile) return target;
            if (target.Scheme != current.Scheme) return target;
            if (target.Host != current.Host) return target;
            for (int i = 0; i < target.Segments.Length; i++)
            {
                if (i >= current.Segments.Length || target.Segments[i] != current.Segments[i])
                {
                    var relative = Path.Combine(Enumerable.Repeat("..", current.Segments.Length - i - 1).Concat(target.Segments.Skip(i)).ToArray());
                    return new Uri(relative + target.Fragment, UriKind.Relative);
                }
            }
            return target;
        }

        public override void Serialize(object obj, XmlWriter writer, IPropertySerializationInfo property, bool writeInstance, XmlIdentificationMode identificationMode, XmlSerializationContext context)
        {
            var modelElement = obj as IModelElement;
            if (modelElement == null)
            {
                base.Serialize(obj, writer, property, writeInstance, identificationMode, context);
            }
            else
            {
                var model = modelElement.Model;
                var modelContext = context as ModelSerializationContext;
                if (modelContext == null || model == modelContext.Model)
                {
                    base.Serialize(obj, writer, property, writeInstance, identificationMode, context);
                }
                else
                {
                    writer.WriteStartAttribute("href");
                    WriteIdentifiedObject(writer, obj, property.IdentificationMode, GetSerializationInfo(obj.GetType(), false), context);
                    writer.WriteEndAttribute();
                }
            }
        }

        protected override string GetAttributeValue(object value, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            var model = context.Root as Model;
            var modelElement = value as ModelElement;
            if (modelElement != null && model != null)
            {
                Uri uri;
                if (modelElement.Model == model)
                {
                    uri = modelElement.RelativeUri;
                }
                else
                {
                    uri = MakeShortUri(modelElement.AbsoluteUri, model.ModelUri);
                }
                if (uri != null)
                {
                    return uri.ConvertToString();
                }
            }
            return base.GetAttributeValue(value, info, context);
        }

        protected virtual XmlModel CreateModelForRoot(object root)
        {
            var modelElement = root as ModelElement;
            if (modelElement != null)
            {
                var model = modelElement.Model as XmlModel;
                if (model == null)
                {
                    model = new XmlModel();
                    var oldModel = modelElement.Model;
                    if (oldModel != null)
                    {
                        model.ModelUri = oldModel.ModelUri;
                    }
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

            var idStore = new Dictionary<string, IModelElement>();
            foreach (var pair in context.IDs)
            {
                var modelElement = pair.Value as IModelElement;
                if (modelElement != null && !idStore.ContainsKey(pair.Key))
                {
                    idStore.Add(pair.Key, modelElement);
                }
            }
            if (idStore.Count > 0)
            {
                model.IdStore = idStore;
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
                if (model.RootElements.Count == 1) return model.RootElements[0];
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

        public void SerializeFragment(IModelElement element, Stream target)
        {
            base.Serialize(element, target, true);
        }
    }
}
