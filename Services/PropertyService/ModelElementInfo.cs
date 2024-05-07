namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// The record for a model element and schema information
    /// </summary>
    /// <param name="ModelElement">the selected model element</param>
    /// <param name="Schema">the schema</param>
    public record ModelElementInfo(IModelElement ModelElement, SchemaElement Schema);
}
