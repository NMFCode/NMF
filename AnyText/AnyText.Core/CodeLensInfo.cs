using System.Collections.Generic;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents a CodeLens item used for a Language Server Protocol (LSP) server.
    /// CodeLens provides information or actions associated with specific locations in a text document.
    /// </summary>
    public class CodeLensInfo
    {
        /// <summary>
        /// Gets or sets the title of the CodeLens item, typically a label displayed in the editor.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier for the command to be executed when the CodeLens is activated.
        /// </summary>
        public string CommandIdentifier { get; set; }
        
        /// <summary>
        /// Gets or sets the dictionary of arguments to be passed along with the command when invoked.
        /// </summary>
        public Dictionary<string, object> Arguments { get; set; }
        
        /// <summary>
        /// Gets or sets the start position of the text range that this CodeLens is associated with.
        /// </summary>
        public ParsePosition Start { get; set; }
        
        /// <summary>
        /// Gets or sets the end position of the text range that this CodeLens is associated with.
        /// </summary>
        public ParsePosition End { get; set; }
        
        /// <summary>
        /// Gets or sets additional data associated with this CodeLens, which can be used for custom functionality.
        /// </summary>
        public object Data { get; set; }
    }
}