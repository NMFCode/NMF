namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Options for creating or renaming files, such as overwrite behavior.
    /// </summary>
    public class FileOptions
    {
        /// <summary>
        ///     If true, overwrite an existing file.
        /// </summary>
        public bool? Overwrite { get; set; }

        /// <summary>
        ///     If true, ignore the operation if the file already exists.
        /// </summary>
        public bool? IgnoreIfExists { get; set; }
    }
}