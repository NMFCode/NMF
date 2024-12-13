using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a model serializer that chooses the actual serialization process based on the file extension
    /// </summary>
    public class ExtensionBasedSerializer : IModelSerializer
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
    }
}
