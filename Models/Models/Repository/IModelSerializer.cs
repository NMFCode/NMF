using System;
using System.IO;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a serializer component for model elements
    /// </summary>
    public interface IModelSerializer
    {
        /// <summary>
        /// Serializes the given model to the given target stream
        /// </summary>
        /// <param name="model">the model to serialize</param>
        /// <param name="target">the target stream</param>
        void Serialize(Model model, Stream target);

        /// <summary>
        /// Serialize the given model element as a fragment to the given stream
        /// </summary>
        /// <param name="element">the model element to serialize</param>
        /// <param name="target">the target stream</param>
        void SerializeFragment(ModelElement element, Stream target);

        /// <summary>
        /// Deserializes the given source into a model
        /// </summary>
        /// <param name="source">the source stream</param>
        /// <param name="modelUri">the URI of the model</param>
        /// <param name="repository">the repository in the context of which the model is deserialized</param>
        /// <param name="addToRepository">true, if the model should be added to the repository, otherwise false</param>
        /// <returns>the deserialized model</returns>
        Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository);

    }
}
