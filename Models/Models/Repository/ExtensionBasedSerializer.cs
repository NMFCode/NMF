using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a model serializer that chooses the actual serialization process based on the file extension
    /// </summary>
    public class ExtensionBasedSerializer : IModelSerializer, IEnumerable<KeyValuePair<string, IModelSerializer>>
    {
        private readonly IModelSerializer _defaultSerializer;
        private readonly Dictionary<string, IModelSerializer> _serializerByExtension;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="defaultSerializer">the default serializer</param>
        /// <param name="serializerByExtension"></param>
        public ExtensionBasedSerializer(IModelSerializer defaultSerializer, Dictionary<string, IModelSerializer> serializerByExtension)
        {
            if (serializerByExtension == null) throw new ArgumentNullException(nameof(serializerByExtension));

            _defaultSerializer = defaultSerializer ?? MetaRepository.Instance.Serializer;
            _serializerByExtension = serializerByExtension;
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="defaultSerializer">the default serializer</param>
        public ExtensionBasedSerializer(IModelSerializer defaultSerializer) : this(defaultSerializer, new Dictionary<string, IModelSerializer>())
        {
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        public ExtensionBasedSerializer() : this(null, new Dictionary<string, IModelSerializer>())
        {
        }

        /// <summary>
        /// Registers the given serializer for the given extension
        /// </summary>
        /// <param name="extension">the extension (including the leading period)</param>
        /// <param name="serializer">the serializer</param>
        public void Add(string extension, IModelSerializer serializer)
        {
            if (extension == null) throw new ArgumentNullException(nameof(extension));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            _serializerByExtension[extension] = serializer;
        }

        /// <inheritdoc />
        public Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository)
        {
            return SelectSerializer(modelUri).Deserialize(source, modelUri, repository, addToRepository);
        }

        private IModelSerializer SelectSerializer(Uri uri)
        {
            if (uri != null)
            {
                if (uri.IsAbsoluteUri)
                {
                    var path = uri.LocalPath;
                    if (path != null && _serializerByExtension.TryGetValue(Path.GetExtension(path) ?? string.Empty, out var serializer))
                    {
                        return serializer ?? _defaultSerializer;
                    }
                }
                else if (_serializerByExtension.TryGetValue(Path.GetExtension(uri.OriginalString) ?? string.Empty, out var serializer))
                {
                    return serializer ?? _defaultSerializer;
                }
            }
            return _defaultSerializer;
        }

        /// <inheritdoc />
        public void Serialize(Model model, Stream target)
        {
            SelectSerializer(model.ModelUri).Serialize(model, target);
        }

        /// <inheritdoc />
        public void SerializeFragment(ModelElement element, Stream target)
        {
            _defaultSerializer.SerializeFragment(element, target);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, IModelSerializer>> GetEnumerator()
        {
            return _serializerByExtension.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
