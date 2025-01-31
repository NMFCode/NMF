namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents the information needed to rename an existing file.
    /// </summary>
    public class RenameFile
    {
        /// <summary>
        ///     The type of file operation (e.g., "rename").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        ///     The URI of the old file name.
        /// </summary>
        public string OldUri { get; set; }

        /// <summary>
        ///     The URI of the new file name.
        /// </summary>
        public string NewUri { get; set; }

        /// <summary>
        ///     File options (e.g., whether to overwrite).
        /// </summary>
        public FileOptions Options { get; set; }

        /// <summary>
        ///     An optional annotation ID related to the file rename.
        /// </summary>
        public string AnnotationId { get; set; }
    }
}