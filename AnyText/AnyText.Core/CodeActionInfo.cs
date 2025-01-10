namespace NMF.AnyText
{
    /// <summary>
    /// Represents the information about a code action.
    /// </summary>
    public class CodeActionInfo
    {
        /// <summary>
        /// The title is typically displayed in the UI to describe the action.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Kind of the code action.
        /// Possible values:
        /// - "quickfix"
        /// - "refactor"
        /// - "refactor.extract"
        /// - "refactor.inline"
        /// - "refactor.rewrite"
        /// - "source"
        /// - "source.organizeImports"
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// This array holds diagnostics for which this action is relevant. If no diagnostics are set, the action may apply generally.
        /// </summary>
        public string[] Diagnostics { get; set; }

        /// <summary>
        /// A value of <c>true</c> indicates that the code action is preferred; otherwise, <c>false</c> or <c>null</c> if there's no preference.
        /// </summary>
        public bool IsPreferred { get; set; }

        /// <summary>
        /// This is the text that describes the command to execute, which can be shown to the user.
        /// </summary>
        public string CommandTitle { get; set; }

        /// <summary>
        /// The command is the identifier or name of the action to execute when the user selects it.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// These are the parameters passed to the command when it is executed.
        /// </summary>
        public object[] Arguments { get; set; }
        /// <summary>
        /// Identifies the Diagnostic that this Action fixes
        /// </summary>
        public string DiagnosticIdentifier  { get; set; }
        /// <summary>
        /// Defines the Workspace changes this action executes
        /// </summary>
        public WorkspaceEdit WorkspaceEdit { get; set; }
           
    }
}