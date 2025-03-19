using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private List<DiagnosticItem> _diagnosticItems;

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

        internal virtual bool IsUnexpectedContent => false;

        /// <summary>
        /// True, if the rule application was successful, otherwise false
        /// </summary>
        public virtual bool IsPositive => true;

        /// <summary>
        /// Gets the element that denotes the context for this rule application
        /// </summary>
        public virtual object ContextElement => Parent?.ContextElement;

        /// <summary>
        /// Gets the semantic element of the rule application
        /// </summary>
        public virtual object SemanticElement => null;

        internal void AddDiagnosticItem(DiagnosticItem diagnosticItem)
        {
            if (_diagnosticItems == null)
            {
                _diagnosticItems = new List<DiagnosticItem>();
            }
            _diagnosticItems.Add(diagnosticItem);
        }

        internal bool RemoveDiagnosticItem(DiagnosticItem diagnosticItem)
        {
            return _diagnosticItems != null && _diagnosticItems.Remove(diagnosticItem);
        }

        /// <summary>
        /// Gets a collection of diagnostic items related to this rule application
        /// </summary>
        public IEnumerable<DiagnosticItem> DiagnosticItems => _diagnosticItems ?? Enumerable.Empty<DiagnosticItem>();

        /// <summary>
        /// Gets the child rule application at the given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="rule">The expected rule of the child</param>
        /// <returns>The child at the given position or null</returns>
        public virtual RuleApplication FindChildAt(ParsePosition position, Rule rule) => null;

        /// <summary>
        /// Validates the rule semantically
        /// </summary>
        /// <param name="context">the parse context in which the rule is validated</param>
        public virtual void Validate(ParseContext context) { }

        public virtual IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition) => Rule.SuggestCompletions(context, this, position);

        /// <summary>
        /// Gets the parsed newPosition under the given context
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <returns>the parsed newPosition</returns>
        public abstract object GetValue(ParseContext context);

        /// <summary>
        /// Adds document symbols to a list
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <param name="result">the list to add document symbols to</param>
        public virtual void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            if (Rule.SymbolKind == SymbolKind.Null) return;
            AddDocumentSymbol(context, result, null);
        }

        /// <summary>
        /// Adds a document symbol to a list
        /// </summary>
        /// <param name="context">the parse context</param>
        /// <param name="result">the list to add the document symbol to</param>
        /// <param name="children">the children symbols of the document symbol</param>
        public virtual void AddDocumentSymbol(ParseContext context, ICollection<DocumentSymbol> result, IEnumerable<DocumentSymbol> children)
        {
            var identifier = GetIdentifier();
            if (identifier != null)
            {
                result.Add(new DocumentSymbol()
                {
                    Name = (string)identifier.GetValue(context),
                    Detail = null,
                    Kind = Rule.SymbolKind,
                    Tags = Rule.SymbolTags(this),
                    Range = new ParseRange(CurrentPosition, CurrentPosition + Length),
                    SelectionRange = new ParseRange(identifier.CurrentPosition, identifier.CurrentPosition + identifier.Length),
                    Children = children != null && children.Any() ? children : null
                });
            }
        }

        /// <summary>
        /// Gets the folding ranges present in the rule application
        /// </summary>
        /// <param name="result">The IEnumerable to hold the folding ranges</param>
        public virtual void AddFoldingRanges(ICollection<FoldingRange> result)
        {
            if (Comments != null)
            {
                AddCommentFoldingRanges(result);
            }
        }

        private void AddCommentFoldingRanges(ICollection<FoldingRange> result)
        {
            for (var i = 0; i < Comments.Count; i++)
            {
                var commentRuleApplication = Comments[i];

                RuleApplication endCommentRuleApplication;
                do
                {
                    endCommentRuleApplication = Comments[i++];
                }
                while (endCommentRuleApplication.CurrentPosition.Col == commentRuleApplication.CurrentPosition.Col && i < Comments.Count);

                if (commentRuleApplication.CurrentPosition.Line == endCommentRuleApplication.CurrentPosition.Line + endCommentRuleApplication.Length.Line) continue;

                var commentsFoldingRange = new FoldingRange()
                {
                    StartLine = (uint)commentRuleApplication.CurrentPosition.Line,
                    StartCharacter = (uint)commentRuleApplication.CurrentPosition.Col,
                    EndLine = (uint)(endCommentRuleApplication.CurrentPosition.Line + endCommentRuleApplication.Length.Line),
                    EndCharacter = (uint)(endCommentRuleApplication.CurrentPosition.Col + endCommentRuleApplication.Length.Col),
                    Kind = "comment"
                };

                result.Add(commentsFoldingRange);
            }
        }

        /// <summary>
        /// Gets the first reference or definition rule in the upward parse tree starting from this rule application
        /// </summary>
        /// <returns>The rule application of the reference or definition rule</returns>
        public RuleApplication GetFirstReferenceOrDefinition()
        {
            if (Rule.IsReference || Rule.IsDefinition)
            {
                return this;
            }
            return Parent?.GetFirstReferenceOrDefinition() ?? null;
        }

        /// <summary>
        /// Gets the first contained rule application that represents an identifier
        /// </summary>
        /// <returns>The rule application for the literal rule representing the identifier</returns>
        public virtual RuleApplication GetIdentifier()
        {
            if (Rule.IsIdentifier)
            {
                return this;
            }
            return null;
        }

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
        public virtual IEnumerable<DiagnosticItem> CreateParseErrors() => Enumerable.Empty<DiagnosticItem>();

        /// <summary>
        /// Denotes a potential error to improve error reporting
        /// </summary>
        public virtual RuleApplication PotentialError => null;
        
        /// <summary>
        /// Adds all CodeLens information of this <see cref="RuleApplication"/> to the provided collection.
        /// </summary>
        /// <param name="codeLenses">The collection to which the <see cref="CodeLensInfo"/> objects will be added.</param>
        /// <param name="predicate">An optional predicate that filters which rule applications should have their CodeLenses added. Default is <c>true</c> for all.</param>
        public virtual void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            predicate ??= _ => true;
            
            if (Rule.SupportedCodeLenses.Any() && predicate.Invoke(this))
            {
                var ruleCodeLenses = Rule.SupportedCodeLenses.Select(a => new CodeLensApplication(a, this));
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
        /// Gets the literal at the given position
        /// </summary>
        /// <param name="position">the position</param>
        /// <returns>the literal rule application or null, if there is no literal there</returns>
        public abstract RuleApplication GetLiteralAt(ParsePosition position);

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
