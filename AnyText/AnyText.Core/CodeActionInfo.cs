using System;
using System.Collections.Generic;
using NMF.AnyText.Rules;
using NMF.AnyText.Workspace;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents the information about a code action.
    /// </summary>
    public abstract class CodeActionInfo : ActionInfo
    {
        /// <summary>
        /// The title is typically displayed in the UI to describe the action.
        /// </summary>
        public string Title { get; init; }

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
        public string Kind { get; init; }

        /// <summary>
        /// This array holds diagnostics for which this action is relevant. If no diagnostics are set, the action may apply generally.
        /// </summary>
        public string[] Diagnostics { get; init; }

        /// <summary>
        /// A value of <c>true</c> indicates that the code action is preferred; otherwise, <c>false</c> or <c>null</c> if there's no preference.
        /// </summary>
        public bool IsPreferred { get; init; }

        /// <summary>
        /// This is the text that describes the command to execute, which can be shown to the user.
        /// </summary>
        public string CommandTitle { get; init; }

        /// <summary>
        /// The command is the identifier or name of the action to execute when the user selects it.
        /// </summary>
        public string CommandIdentifier { get; init; }

        /// <summary>
        /// These are the parameters passed to the command when it is executed.
        /// </summary>
        public Dictionary<string, object> Arguments { get; init; }
        
        /// <summary>
        /// Identifies the Diagnostic that this Action fixes
        /// </summary>
        public string DiagnosticIdentifier  { get; init; }

        /// <summary>
        /// Creates the workspace edit calculated for the given arguments
        /// </summary>
        /// <param name="arguments">the arguments for which to calculate the workspace edit</param>
        /// <returns>The workspace edit</returns>
        public abstract WorkspaceEdit CreateWorkspaceEdit(ExecuteCommandArguments arguments);
    }

    /// <summary>
    /// Represents the information about a code action.
    /// </summary>
    /// <typeparam name="T">The semantic type of elements for which this action is executed</typeparam>
    public class CodeActionInfo<T> : CodeActionInfo
    {
        /// <summary>
        /// The actual action that is executed
        /// </summary>
        public Action<T, ExecuteCommandArguments> Action { get; init; }

        /// <summary>
        /// Defines the how the WorkspaceEdit Object of this CodeAction is created
        /// </summary>
        public Func<T, ExecuteCommandArguments, WorkspaceEdit> WorkspaceEdit { get; init; }

        /// <inheritdoc/>
        public override void Invoke(ExecuteCommandArguments arguments)
        {
            if (Action != null && arguments.RuleApplication.ContextElement is T typedElement)
            {
                Action.Invoke(typedElement, arguments);
            }
        }

        /// <inheritdoc />
        public override WorkspaceEdit CreateWorkspaceEdit(ExecuteCommandArguments arguments)
        {
            if (WorkspaceEdit != null && arguments.RuleApplication.ContextElement is T typedElement)
            {
                return WorkspaceEdit.Invoke(typedElement, arguments);
            }
            return null;
        }
    }
}