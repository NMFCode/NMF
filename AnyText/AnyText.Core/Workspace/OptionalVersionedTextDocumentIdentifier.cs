namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Identifies a text document with optional versioning.
    /// </summary>
    public class OptionalVersionedTextDocumentIdentifier
    {
        /// <summary>
        ///     The URI of the text document.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     An optional version number for the document, if versioning is supported.
        /// </summary>
        public int? Version { get; set; }
    }
}