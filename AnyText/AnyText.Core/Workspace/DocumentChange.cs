namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents a change to a document, including text edits, file creation, renaming, or deletion.
    /// </summary>
    public class DocumentChange
    {
        /// <summary>
        ///     Text document edits (e.g., line insertions, deletions).
        /// </summary>
        public TextDocumentEdit TextDocumentEdit { get; set; }

        /// <summary>
        ///     Information for creating a new file.
        /// </summary>
        public CreateFile CreateFile { get; set; }

        /// <summary>
        ///     Information for renaming an existing file.
        /// </summary>
        public RenameFile RenameFile { get; set; }

        /// <summary>
        ///     Information for deleting an existing file.
        /// </summary>
        public DeleteFile DeleteFile { get; set; }
    }
}