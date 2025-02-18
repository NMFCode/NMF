using System;
using System.Collections.Generic;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents a CodeLens item used for a Language Server Protocol (LSP) server.
    /// CodeLens provides information or actions associated with specific locations in a text document.
    /// </summary>
    public abstract class CodeLensInfo : ActionInfo
    {
        /// <summary>
        /// Gets or sets the title of the CodeLens item, typically a label displayed in the editor.
        /// </summary>
        public string Title { get; init; }
        
        /// <summary>
        /// Gets or sets the identifier for the command to be executed when the CodeLens is activated.
        /// </summary>
        public string CommandIdentifier { get; init; }
        
        /// <summary>
        /// Gets or sets the dictionary of arguments to be passed along with the command when invoked.
        /// </summary>
        public Dictionary<string, object> Arguments { get; init; }
        
        /// <summary>
        /// Gets or sets additional data associated with this CodeLens, which can be used for custom functionality.
        /// </summary>
        public object Data { get; init; }

        /// <summary>
        /// Calculates the title of the code lens for the given rule application
        /// </summary>
        /// <param name="ruleApplication"></param>
        /// <returns>The title of the code lens</returns>
        public virtual string GetTitleForRuleApplication(RuleApplication ruleApplication) => Title;
    }

    /// <summary>
    /// Represents a CodeLens item used for a Language Server Protocol (LSP) server.
    /// CodeLens provides information or actions associated with specific locations in a text document.
    /// </summary>
    /// <typeparam name="T">The semantic type of elements for which this action is executed</typeparam>
    public class CodeLensInfo<T> : CodeLensInfo
    {
        /// <summary>
        /// The actual execution of this CodeLens
        /// </summary>
        public Action<T, ExecuteCommandArguments> Action { get; init; }

        /// <summary>
        /// A function to calculate the title from the semantic element
        /// </summary>
        public Func<T, string> TitleFunc { get; init; }

        /// <inheritdoc/>
        public override void Invoke(ExecuteCommandArguments arguments)
        {
            if (Action != null && arguments.RuleApplication.ContextElement is T typedElement)
            {
                Action.Invoke(typedElement, arguments);
            }
        }

        /// <inheritdoc />
        public override string GetTitleForRuleApplication(RuleApplication ruleApplication)
        {
            if (TitleFunc != null && ruleApplication.ContextElement is T typedElement)
            {
                return TitleFunc(typedElement) ?? Title;
            }
            return Title;
        }

    }
}