using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Serialization;
using NMF.Serialization.Json;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JsonSerializer = NMF.Serialization.Json.JsonSerializer;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes a serializer that serializes models to JSON
    /// </summary>
    public class JsonModelSerializer : JsonSerializer, IModelSerializer
    {
        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        public JsonModelSerializer() : this(XmlSerializationSettings.Default) { }

        /// <summary>
        /// Creates a new model serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        public JsonModelSerializer(XmlSerializationSettings settings) : this(settings, null) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public JsonModelSerializer(Serializer parent) : base(parent) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public JsonModelSerializer(Serializer parent, XmlSerializationSettings settings) : base(parent, settings) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="knownTypes">A collection of known types</param>
        public JsonModelSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes)
            : base(settings, knownTypes)
        {
        }

        /// <inheritdoc />
        protected override XmlSerializationContext CreateSerializationContext(object root)
        {
            var tempRepository = new ModelRepository();
            tempRepository.Locators.Add(FileLocator.Instance);
            tempRepository.Serializer = this;
            return new ModelSerializationContext(tempRepository, CreateModelForRoot(root));
        }

        /// <summary>
        /// Deserializes the contents from the given reader
        /// </summary>
        /// <param name="reader">the JSON reader to read from</param>
        /// <param name="repository">the repository in the scope of which the fragment is resolved</param>
        /// <param name="resolveModel">the model in which relative Uris are resolved</param>
        /// <returns>the object contained in the JSON format</returns>
        public object DeserializeFragment(ref Utf8JsonStreamReader reader, IModelRepository repository, Model resolveModel)
        {
            ITypeSerializationInfo tsi = null;
            var root = CreateObject(ref reader, ref tsi);
            var context = new ModelSerializationContext(repository, resolveModel);
            Initialize(ref reader, context, root, tsi);
            context.Cleanup();
            return root;
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
        protected override string GetAttributeValue(object value, ITypeSerializationInfo info, bool isCollection, XmlSerializationContext context)
        {
            if (value is ModelElement modelElement && context.Root is Model model)
            {
                Uri uri = model.CreateUriForElement(modelElement);
                if (uri != null)
                {
                    return uri.ConvertToString();
                }
            }
            return base.GetAttributeValue(value, info, isCollection, context);
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
            var reader = new Utf8JsonStreamReader(source, 2048);

            ITypeSerializationInfo tsi = null;
            while (reader.TokenType == JsonTokenType.None) reader.Read();
            var root = CreateObject(ref reader, ref tsi);
            var model = CreateModelForRoot(root);
            var context = new ModelSerializationContext(repository, model);

            Initialize(ref reader, context, root, tsi);

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

        /// <inheritdoc />
        public void Serialize(Model model, Stream target)
        {
            var writer = new Utf8JsonWriter(target);
            Serialize(model, writer);
            writer.Dispose();
        }

        /// <inheritdoc />
        public void SerializeFragment(ModelElement element, Stream target)
        {
            var writer = new Utf8JsonWriter(target);
            Serialize(element, writer, fragment: true);
            writer.Dispose();
        }
    }
}
