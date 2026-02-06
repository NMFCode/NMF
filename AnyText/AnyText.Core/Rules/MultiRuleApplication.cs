using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Represents a rule application with multiple inner rule applications
    /// </summary>
    public class MultiRuleApplication : RuleApplication
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">the rule referenced by this rule application</param>
        /// <param name="inner">the inner rule applications</param>
        /// <param name="length">the length of the rule application</param>
        /// <param name="examinedTo">the span that was used to conclude the rule application</param>
        public MultiRuleApplication(Rule rule, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            Inner = inner;
        }

        /// <summary>
        /// Gets a list of inner rule applications
        /// </summary>
        public List<RuleApplication> Inner { get; }

        /// <inheritdoc/>
        public override IEnumerable<RuleApplication> Children => Inner;

        /// <inheritdoc/>
        public override void Activate(ParseContext context, bool initial)
        {
            foreach (var inner in Inner)
            {
                inner.ChangeParent(this, context);
                if (!inner.IsActive)
                {
                    inner.Activate(context, initial);
                }
            }
            base.Activate(context, initial);
        }
        
        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            foreach (var inner in Inner)
            {
                if (inner.CurrentPosition > nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo >=  position)
                {
                    suggestions = suggestions.NullsafeConcat(inner.SuggestCompletions(position, fragment, context, nextTokenPosition));
                }
            }
            return suggestions;
        }

        internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
        {
            if (multiRule.Rule != Rule)
            {
                return base.MigrateTo(multiRule, context);
            }

            Length = multiRule.Length;
            ExaminedTo = multiRule.ExaminedTo;
            Comments = multiRule.Comments;
            multiRule.ReplaceWith(this);

            for (int index = 0; index < Inner.Count; index++)
            {
                var old = Inner[index];
                var newApp = multiRule.Inner[index];
                if (old != newApp)
                {
                    Inner[index] = newApp = newApp.ApplyTo(old, context);
                    if (newApp != old && IsActive)
                    {
                        newApp.ChangeParent(this, context);
                        old.Deactivate(context);
                        newApp.Activate(context, false);
                    }
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public override void Validate(ParseContext context)
        {
            foreach (var item in Inner)
            {
                item.Validate(context);
            }
        }

        /// <inheritdoc />
        public override RuleApplication GetIdentifier()
        {
            var result = base.GetIdentifier();
            for (var i = 0; result == null && i < Inner.Count; i++)
            {
                result = Inner[i].GetIdentifier();
            }
            return result;
        }

        /// <inheritdoc/>
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        /// <inheritdoc/>
        public override void Deactivate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                if (inner.IsActive && inner.Parent == this)
                {
                    inner.Deactivate(context);
                    inner.ChangeParent(null, context);
                }
            }
            base.Deactivate(context);
        }

        internal override void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            foreach (var ruleApplication in Inner)
            {
                ruleApplication.AddCodeLenses(codeLenses, predicate);
            }
            base.AddCodeLenses(codeLenses, predicate);
        }

        /// <inheritdoc/>
        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            if (CurrentPosition + Length == currentRoot.CurrentPosition + currentRoot.Length && Inner.Count > 0)
            {
                var lastInner = Inner[Inner.Count - 1];
                var recovered = lastInner.Recover(currentRoot, context, out position);
                if (recovered.Length > lastInner.Length)
                {
                    Inner[Inner.Count - 1] = recovered;
                    SetRecovered(true);
                    return this;
                }
            }
            return base.Recover(currentRoot, context, out position);
        }

        /// <inheritdoc/>
        public override RuleApplication FindChildAt(ParsePosition position, Rule rule)
        {
            return Inner.FirstOrDefault(c => c.CurrentPosition == position && c.Rule == rule);
        }

        /// <inheritdoc/>
        public override void ReplaceChild(RuleApplication childApplication, RuleApplication newChild, ParseContext context)
        {
            var index = Inner.IndexOf(childApplication);
            if (index >= 0)
            {
                Inner[index] = newChild;
                newChild.ChangeParent(this, context);
            }
            else
            {
                base.ReplaceChild(childApplication, newChild, context);
            }
        }

        /// <inheritdoc/>
        public override void AddParseErrors(ParseContext context)
        {
            if (IsRecovered)
            {
                foreach (var app in Inner.Where(r => r.IsRecovered))
                {
                    app.AddParseErrors(context);
                }
            }
            var last = Inner.LastOrDefault();
            if (last != null && !last.IsRecovered)
            {
                last.AddParseErrors(context);
            }
        }

        /// <inheritdoc/>
        public override int CalculateIndex(RuleApplication ruleApplication)
        {
            return Inner.IndexOf(ruleApplication);
        }

        /// <inheritdoc/>
        public override int CalculateIndex(RuleApplication ruleApplication, Stack<Rule> ruleStack)
        {
            var index = 0;
            foreach (var item in Inner)
            {
                if (item.IsStack(ruleStack))
                {
                    index++;
                }
                if (item == ruleApplication)
                {
                    return index;
                }
            }
            return -1;
        }

        /// <inheritdoc/>
        public override bool IsStack(Stack<Rule> ruleStack)
        {
            return false;
        }

        /// <inheritdoc/>
        protected virtual void OnMigrate(List<RuleApplication> removed, List<RuleApplication> added) { }


        /// <inheritdoc/>
        public override object GetValue(ParseContext context)
        {
            if (context == null)
            {
                // this is used for debugging purposes
                return string.Join(' ', Inner.Select(r => r.GetValue(context)));
            }
            // by default, the value of a sequence is the string representation of its contents
            if (Length.Line <= 0)
            {
                return context.Input[CurrentPosition.Line].Substring(CurrentPosition.Col, Length.Col);
            }
            else
            {
                var builder = new StringBuilder();
                var lineNo = CurrentPosition.Line;
                builder.AppendLine(context.Input[lineNo].Substring(CurrentPosition.Col));
                for (var i = 1; i < Length.Line; i++)
                {
                    builder.AppendLine(context.Input[lineNo + i]);
                }
                return builder.ToString();
            }
        }

        internal override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            if (Rule.PassAlongDocumentSymbols)
            {
                foreach (var innerRuleApplication in Inner)
                {
                    innerRuleApplication.AddDocumentSymbols(context, result);
                }
                return;
            }

            if (Rule.SymbolKind == SymbolKind.Null) return;
            
            var children = new List<DocumentSymbol>();
            foreach (var innerRuleApplication in Inner)
            {
                innerRuleApplication.AddDocumentSymbols(context, children);
            }

            AddDocumentSymbol(context, result, children);
        }

        /// <inheritdoc />
        internal override void AddFoldingRanges(ICollection<FoldingRange> result)
        {
            base.AddFoldingRanges(result);

            if (Rule.HasFoldingKind(out var kind))
            {
                AddFoldingRange(kind, result);
            }

            foreach (var innerRuleApplication in Inner)
            {
                innerRuleApplication.AddFoldingRanges(result);
            }
        }

        private void AddFoldingRange(string kind, ICollection<FoldingRange> result)
        {
            if (Inner.Count < 2) return;

            var first = Inner[0];
            var last = Inner[Inner.Count - 1];
            var endPosition = last.CurrentPosition + last.Length;

            var foldingRange = new FoldingRange()
            {
                StartLine = (uint)first.CurrentPosition.Line,
                StartCharacter = (uint)first.CurrentPosition.Col,
                EndLine = (uint)endPosition.Line,
                EndCharacter = (uint)endPosition.Col,
                Kind = kind
            };

            result.Add(foldingRange);
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            for (int i = 0; i < Inner.Count; i++)
            {
                Inner[i].IterateLiterals(action, includeFailures && i == Inner.Count - 1);
            }
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            for (int i = 0; i < Inner.Count; i++)
            {
                Inner[i].IterateLiterals(action, parameter, includeFailures && i == Inner.Count - 1);
            }
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Rule.Write(writer, context, this);
        }

        /// <inheritdoc/>
        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            foreach (var inner in Inner)
            {
                if (inner.CurrentPosition > position)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo > position)
                {
                    var lit = inner.GetLiteralAt(position, onlyActive);
                    if (lit != null && lit.IsPositive && (!onlyActive || lit.IsActive))
                    {
                        return lit;
                    }
                }
            }
            return null;
        }

        /// <inheritdoc />
        internal override void AddInlayEntries(ParseRange range, List<InlayEntry> inlayEntries, ParseContext context)
        {
            CheckForInlayEntry(range, inlayEntries, context);
            foreach (var item in Inner)
            {
                item.AddInlayEntries(range, inlayEntries, context);
            }
        }

        /// <inheritdoc />
        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            for (var i = 0; i < Inner.Count; i++)
            {
                var innerFirst = Inner[i].GetFirstInnerLiteral();
                if (innerFirst != null)
                {
                    return innerFirst;
                }
            }
            return null;
        }

        /// <inheritdoc />
        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            for (int i = Inner.Count - 1; i >= 0; i--)
            {
                var innerLast = Inner[i].GetLastInnerLiteral();
                if ( innerLast != null )
                {
                    return innerLast;
                }
            }
            return null;
        }
    }
}
