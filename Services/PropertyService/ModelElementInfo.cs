namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// The record for a model element and schema information
    /// </summary>
    /// <param name="Uri">The URI of the model element</param>
    /// <param name="Identifier">The identifier of the model element</param>
    /// <param name="Data">the selected model element</param>
    /// <param name="Schema">the schema</param>
    public record ModelElementInfo(string Uri, string Identifier, IModelElement Data, SchemaElement Schema)
    {
        /// <summary>
        /// Wraps an existing model element into a ModelElementInfo
        /// </summary>
        /// <param name="modelElement">The model element</param>
        /// <returns>The element info</returns>
        public static ModelElementInfo FromModelElement(IModelElement modelElement)
        {
            if (modelElement == null) { return null; }
            return new ModelElementInfo(GetUri(modelElement), GetIdentifierString(modelElement), modelElement, new SchemaElement(modelElement, SchemaWriter.Instance));
        }

        private static string GetUri(IModelElement modelElement)
        {
            var uri = modelElement.AbsoluteUri;
            if (uri != null && uri.IsAbsoluteUri) return uri.AbsoluteUri;
            return modelElement.RelativeUri?.ToString();
        }

        private static string GetIdentifierString(IModelElement modelElement)
        {
            var identifier = modelElement.ToIdentifierString();
            if (identifier == null)
            {
                return modelElement.GetClass().Name;
            }
            else
            {
                return $"{modelElement.GetClass().Name} {identifier}";
            }
        }
    }
}
