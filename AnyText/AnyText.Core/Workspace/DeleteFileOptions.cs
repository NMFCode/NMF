namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Options for deleting files, such as recursive deletion and handling missing files.
    /// </summary>
    public class DeleteFileOptions
    {
        /// <summary>
        ///     If true, delete directories recursively.
        /// </summary>
        public bool? Recursive { get; set; }

        /// <summary>
        ///     If true, ignore the operation if the file does not exist.
        /// </summary>
        public bool? IgnoreIfNotExists { get; set; }
    }
}