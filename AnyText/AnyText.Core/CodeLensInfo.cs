using System;
using System.Collections.Generic;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents a CodeLens item used for a Language Server Protocol (LSP) server.
    /// CodeLens provides information or actions associated with specific locations in a text document.
    /// </summary>
    public class CodeLensInfo
    {
        /// <summary>
        /// RuleApplication of the Lens
        /// </summary>
        public RuleApplication RuleApplication {get;set;}
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
        /// Gets or sets additional data associated with this CodeLens, which can be used for custom functionality.
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// The actual execution of this CodeLens
        /// </summary>
        public Action<ExecuteCommandArguments> Action { get; set; }
    }
}