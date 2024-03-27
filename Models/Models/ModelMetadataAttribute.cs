using System;

namespace NMF.Models
{
    /// <summary>
    /// Declares that the assembly includes code for a given metamodel
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ModelMetadataAttribute : Attribute
    {
        /// <summary>
        /// The model uri
        /// </summary>
        public Uri ModelUri { get; private set; }

        /// <summary>
        /// the name of the resource
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// Defines that the assembly contains a model at the given embedded resource name
        /// </summary>
        /// <param name="modelUri">The URI of the model</param>
        /// <param name="resourceName">The resource name or name suffix</param>
        public ModelMetadataAttribute(string modelUri, string resourceName)
        {
            Uri parsedModelUri;
            if (Uri.TryCreate(modelUri, UriKind.Absolute, out parsedModelUri))
            {
                ModelUri = parsedModelUri;
            }
            else
            {
                ModelUri = new Uri(modelUri, UriKind.Relative);
            }
            ResourceName = resourceName;
        }
    }
}
