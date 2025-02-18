namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents the text document edit instructions, including the document and the edits.
    /// </summary>
    public class TextDocumentEdit
    {
        /// <summary>
        ///     Identifies the text document to edit, including optional version information.
        /// </summary>
        public OptionalVersionedTextDocumentIdentifier TextDocument { get; set; }

        /// <summary>
        ///     An Array of edits to perform on the document (e.g., insertions, deletions).
        /// </summary>
        public TextEdit[] Edits { get; set; }
    }
}