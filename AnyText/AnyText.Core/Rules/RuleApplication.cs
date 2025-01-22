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
        private ParsePosition _currentPosition;

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

            _currentPosition = currentPosition;
            Rule = rule;
            Length = length;
            ExaminedTo = ParsePositionDelta.Larger(length, examinedTo);
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
        /// Gets the parsed newPosition under the given context
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <returns>the parsed newPosition</returns>
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
        public ParsePositionDelta Length { get; protected set; }

        /// <summary>
        /// the amount of text that was analyzed to come to the conclusion of this rule application
        /// </summary>
        public ParsePositionDelta ExaminedTo { get; protected set; } 

        /// <summary>
        /// True, if the rule application is part of the current parse tree
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the last position of this rule application
        /// </summary>
        public ParsePosition CurrentPosition
        {
            get => _currentPosition;
        }

        /// <summary>
        /// Sets a new current position
        /// </summary>
        /// <param name="newPosition">the new current position</param>
        /// <param name="updateChildren">true, if the position of child rule applications should be updated as well, otherwise false</param>
        public void EnsurePosition(ParsePosition newPosition, bool updateChildren)
        {
            if (_currentPosition != newPosition)
            {
                if (updateChildren)
                {
                    Shift(new ParsePositionDelta(newPosition.Line - _currentPosition.Line, newPosition.Col - _currentPosition.Col), _currentPosition.Line);
                }
                else
                {
                    _currentPosition = newPosition;
                }
            }
        }

        /// <summary>
        /// Shifts the current rule application by the given position delta
        /// </summary>
        /// <param name="originalLine">the line of the original shoft</param>
        /// <param name="shift"></param>
        public virtual void Shift(ParsePositionDelta shift, int originalLine)
        {
            if (_currentPosition.Line != originalLine)
            {
                _currentPosition = new ParsePosition(_currentPosition.Line + shift.Line, _currentPosition.Col);
            }
            else
            {
                _currentPosition = new ParsePosition(_currentPosition.Line + shift.Line, _currentPosition.Col + shift.Col);
            }
            if (_currentPosition.Line == 4)
            {
                Debugger.Break();
            }
        }

        /// <summary>
        /// Activates the rule application, i.e. marks it as part of the current parse tree
        /// </summary>
        /// <param name="context">the context in which the parse tree exists</param>
        public virtual void Activate(ParseContext context)
        {
            if (!IsActive)
            {
                IsActive = true;
                Rule.OnActivate(this, context);
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
        /// Gets a collection of parse errors represented by this rule application
        /// </summary>
        /// <returns>A collection of parse errors</returns>
        public virtual IEnumerable<ParseError> CreateParseErrors() => Enumerable.Empty<ParseError>();
        
        /// <summary>
        /// Adds all CodeLens information of this <see cref="RuleApplication"/> to the provided collection.
        /// </summary>
        /// <param name="codeLenses">The collection to which the <see cref="CodeLensInfo"/> objects will be added.</param>
        /// <param name="predicate">An optional predicate that filters which rule applications should have their CodeLenses added. Default is <c>true</c> for all.</param>
        public virtual void AddCodeLenses(ICollection<CodeLensInfo> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            predicate ??= _ => true;
            
            if (Rule.SupportedCodeLenses.Any() && predicate.Invoke(this))
            {
                var end = CurrentPosition + Length;
                var ruleCodeLenses = Rule.SupportedCodeLenses.Select(a => new CodeLensInfo()
                {
                    Arguments = a.Arguments,
                    CommandIdentifier = a.CommandIdentifier,
                    Data = a.Data,
                    Title = a.Title,
                    Start = CurrentPosition,
                    End = end,
                });
                foreach (var codeLens in ruleCodeLenses)
                {
                    codeLenses.Add(codeLens);
                }
            }
        }
        
        /// <summary>
        /// Gets called when the newPosition of the given rule application changes
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
        /// <returns>the merged rule application</returns>
        public abstract RuleApplication ApplyTo(RuleApplication other, ParseContext context);

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

        internal virtual RuleApplication MigrateTo(LiteralRuleApplication literal, ParseContext context)
        {
            if (IsActive)
            {
                literal.Activate(context);
                Deactivate(context);
            }
            return literal;
        }

        internal virtual RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
        {
            if (IsActive)
            {
                multiRule.Activate(context);
                Deactivate(context);
            }
            return multiRule;
        }

        internal virtual RuleApplication MigrateTo(SingleRuleApplication singleRule, ParseContext context)
        {
            if (IsActive)
            {
                singleRule.Activate(context);
                Deactivate(context);
            }
            return singleRule;
        }
    }
}
