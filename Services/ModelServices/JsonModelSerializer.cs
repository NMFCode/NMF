using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Serialization;
using NMF.Serialization.Json;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using JsonSerializer = NMF.Serialization.Json.JsonSerializer;

namespace NMF.Models.Services
{
    internal class JsonModelSerializer : JsonSerializer, IModelSerializer
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
        public JsonModelSerializer(XmlSerializer parent) : base(parent) { }

        /// <summary>
        /// Creates a new serializer and copies settings and known types from the given serializer
        /// </summary>
        /// <param name="settings">The serialization settings</param>
        /// <param name="parent">An XML serializer to copy settings and known type information from</param>
        public JsonModelSerializer(XmlSerializer parent, XmlSerializationSettings settings) : base(parent, settings) { }

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

        public Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Model model, Stream target)
        {
            throw new NotImplementedException();
        }

        public void SerializeFragment(ModelElement element, Stream target)
        {
            throw new NotImplementedException();
        }
    }
}
