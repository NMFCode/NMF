namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the type of a message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Indicates an error
        /// </summary>
        Error = 1,

        /// <summary>
        /// Indicates a warning
        /// </summary>
        Warning,

        /// <summary>
        /// Indicates an informational message
        /// </summary>
        Info,

        /// <summary>
        /// Indicates a log message
        /// </summary>
        Log
    }
}
