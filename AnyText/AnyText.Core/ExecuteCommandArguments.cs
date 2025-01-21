using System.Collections.Generic;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the arguments for executing a command on a document.
    /// </summary>
    public class ExecuteCommandArguments
    {
        
        /// <summary>
        /// URI of the document.
        /// </summary>
        public string DocumentUri { get; set; }
        
        /// <summary>
        /// Starting position of the Range.
        /// </summary>
        public ParsePosition Start { get; set; }
        
        /// <summary>
        /// Ending position of the Range.
        /// </summary>
        public ParsePosition End { get; set; }
        
        /// <summary>
        /// Additional options for the command execution.
        /// </summary>
        public Dictionary<string, object> OtherOptions { get; set; }
    }
}