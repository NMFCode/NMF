using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes the application of a rule
    /// </summary>
    [DebuggerDisplay("{Description}")]
    public abstract class RuleApplication
    {
        /// <summary>
        /// Gets the debugger description for this rule application
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Description => (IsPositive ? "Successful" : "Failed") + " application of " + Rule.GetType().Name + " at " + CurrentPosition.ToString();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule that was matched</param>
        /// <param name="currentPosition">the current position of this rule application</param>
        /// <param name="length">the length of the rule application</param>
        /// <param name="examinedTo">the amount of text that was analyzed to come to the conclusion of this rule application</param>
        /// <exception cref="InvalidOperationException"></exception>
        protected RuleApplication(Rule rule, ParsePosition currentPosition, ParsePositionDelta length, ParsePositionDelta examinedTo)
        {
            if (length.Line < 0)
            {
                throw new InvalidOperationException();
            }

            CurrentPosition = currentPosition;
            Rule = rule;
            Length = length;
            ExaminedTo = examinedTo;
        }

        /// <summary>
        /// True, if the rule application was successful, otherwise false
        /// </summary>
        public virtual bool IsPositive => true;

        /// <summary>
        /// Gets the element that denotes the context for this rule application
        /// </summary>
        public virtual object ContextElement => Parent?.ContextElement;

        /// <summary>
        /// Gets the parsed value under the given context
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <returns>the parsed value</returns>
        public abstract object GetValue(ParseContext context);

        /// <summary>
        /// The rule that was matched
        /// </summary>
        public Rule Rule { get; }

        /// <summary>
        /// Gets a collection of comments for this rule application
        /// </summary>
        public List<RuleApplication> Comments { get; internal set; }

        /// <summary>
        /// the length of the rule application
        /// </summary>
        public ParsePositionDelta Length { get; }

        /// <summary>
        /// the amount of text that was analyzed to come to the conclusion of this rule application
        /// </summary>
        public ParsePositionDelta ExaminedTo { get; } 

        /// <summary>
        /// True, if the rule application is part of the current parse tree
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the last position of this rule application
        /// </summary>
        public ParsePosition CurrentPosition { get; set; }

        /// <summary>
        /// Activates the rule application, i.e. marks it as part of the current parse tree
        /// </summary>
        /// <param name="context">the context in which the parse tree exists</param>
        /// <param name="position">the position at which the rule is activated</param>
        public virtual void Activate(ParseContext context, ParsePosition position)
        {
            if (!IsActive)
            {
                IsActive = true;
                Rule.OnActivate(this, position, context);
            }
        }

        /// <summary>
        /// Deactivates the rule application, i.e. unmarks it as part of the parse tree
        /// </summary>
        /// <param name="context">the context in which the parse tree exists</param>
        public virtual void Deactivate(ParseContext context)
        {
            if (IsActive)
            {
                IsActive = false;
                Rule.OnDeactivate(this, context);
            }
        }

        /// <summary>
        /// Gets the parent rule application in the parse tree
        /// </summary>
        public RuleApplication Parent { get; internal set; }

        /// <summary>
        /// Gets the position of the error
        /// </summary>
        public virtual ParsePosition ErrorPosition => default;

        /// <summary>
        /// Gets the message to indicate why the rule application failed
        /// </summary>
        public virtual string Message => null;

        /// <summary>
        /// Gets called when the value of the given rule application changes
        /// </summary>
        /// <param name="changedChild">the changed rule application (either this or a child in the parse tree)</param>
        /// <param name="context">the parse context</param>
        protected internal virtual void OnValueChange(RuleApplication changedChild, ParseContext context)
        {
            if (!Rule.OnValueChange(this, context))
            {
                Parent?.OnValueChange(this, context);
            }
        }

        /// <summary>
        /// Applies the structure of the current rule application to the given other rule application
        /// </summary>
        /// <param name="other">the rule application to which the rule should be applied</param>
        /// <param name="context">the parse context</param>
        /// <param name="position">the position at which the rule is activated</param>
        /// <returns>the merged rule application</returns>
        public abstract RuleApplication ApplyTo(RuleApplication other, ParsePosition position, ParseContext context);

        /// <summary>
        /// Iterate over all literals
        /// </summary>
        /// <param name="action">the action that should be performed for all literals</param>
        public abstract void IterateLiterals(Action<LiteralRuleApplication> action);

        /// <summary>
        /// Iterate over all literals
        /// </summary>
        /// <typeparam name="T">the parameter type</typeparam>
        /// <param name="action">the action that should be performed for all literals</param>
        /// <param name="parameter">the parameter</param>
        public abstract void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter);

        /// <summary>
        /// Writes the given rule application to the provided text writer
        /// </summary>
        /// <param name="writer">the writer to which the rule application should be written</param>
        /// <param name="context">the parse context</param>
        /// <returns>true, if any content has been written, otherwise false</returns>
        public abstract void Write(PrettyPrintWriter writer, ParseContext context);

        /// <summary>
        /// Applies all formatting instructions
        /// </summary>
        /// <param name="writer">the pretty print writer</param>
        protected void ApplyFormattingInstructions(PrettyPrintWriter writer)
        {
            if (Rule.FormattingInstructions != null)
            {
                foreach (var instruction in Rule.FormattingInstructions)
                {
                    instruction.Apply(writer);
                }
            }
        }

        internal virtual RuleApplication MigrateTo(LiteralRuleApplication literal, ParsePosition position, ParseContext context)
        {
            if (IsActive)
            {
                literal.Activate(context, position);
                Deactivate(context);
            }
            return literal;
        }

        internal virtual RuleApplication MigrateTo(MultiRuleApplication multiRule, ParsePosition position, ParseContext context)
        {
            if (IsActive)
            {
                multiRule.Activate(context, position);
                Deactivate(context);
            }
            return multiRule;
        }

        internal virtual RuleApplication MigrateTo(SingleRuleApplication singleRule, ParsePosition position, ParseContext context)
        {
            if (IsActive)
            {
                singleRule.Activate(context, position);
                Deactivate(context);
            }
            return singleRule;
        }
    }
}
