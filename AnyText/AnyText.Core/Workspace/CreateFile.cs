namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents the information needed to create a new file.
    /// </summary>
    public class CreateFile
    {
        /// <summary>
        ///     The type of file creation (e.g., "create").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        ///     The URI of the file to be created.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     File options (e.g., whether to overwrite an existing file).
        /// </summary>
        public FileOptions Options { get; set; }

        /// <summary>
        ///     An optional annotation ID related to the file creation.
        /// </summary>
        public string AnnotationId { get; set; }
    }
}