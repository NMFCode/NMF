namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents the information needed to delete an existing file.
    /// </summary>
    public class DeleteFile
    {
        /// <summary>
        ///     The type of file operation (e.g., "delete").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        ///     The URI of the file to be deleted.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     File deletion options (e.g., whether to delete recursively).
        /// </summary>
        public DeleteFileOptions Options { get; set; }

        /// <summary>
        ///     An optional annotation ID related to the file deletion.
        /// </summary>
        public string AnnotationId { get; set; }
    }
}