using NMF.AnyText.Model;
using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class MultiRuleApplication : RuleApplication
    {

        public MultiRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, endsAt, examinedTo)
        {
            Inner = inner;
            foreach (var innerApp in inner)
            {
                innerApp.Parent = this;
            }
        }

        public List<RuleApplication> Inner { get; }

        public override void Activate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                inner.Parent = this;
                if (!inner.IsActive)
                {
                    inner.Activate(context);
                }
            }
            base.Activate(context);
        }

        public override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, context, nextTokenPosition);
            foreach (var inner in Inner)
            {
                if (inner.CurrentPosition > nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo >  position
                    && inner.SuggestCompletions(position, context, nextTokenPosition) is var innerSuggestions && innerSuggestions != null)
                {
                    if(suggestions == null)
                    {
                        suggestions = Enumerable.Empty<CompletionEntry>();
                    }
                    suggestions = suggestions.Concat(innerSuggestions);
                }
            }
            return suggestions;
        }

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

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        public override void Shift(ParsePositionDelta shift, int originalLine)
        {
            base.Shift(shift, originalLine);
            foreach (var inner in Inner)
            {
                inner.Shift(shift, originalLine);
            }
        }

        public override void Deactivate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                inner.Parent = null;
                if (inner.IsActive)
                {
                    inner.Deactivate(context);
                }
            }
            base.Deactivate(context);
        }
        
        /// <inheritdoc />
        public override void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            foreach (var ruleApplication in Inner)
            {
                ruleApplication.AddCodeLenses(codeLenses, predicate);
            }
            base.AddCodeLenses(codeLenses, predicate);
        }

        public override RuleApplication FindChildAt(ParsePosition position, Rule rule)
        {
            return Inner.FirstOrDefault(c => c.CurrentPosition == position && c.Rule == rule);
        }

        internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
        {
            if (multiRule.Rule != Rule)
            {
                return base.MigrateTo(multiRule, context);
            }

            EnsurePosition(multiRule.CurrentPosition, false);
            Length = multiRule.Length;
            ExaminedTo = multiRule.ExaminedTo;
            Comments = multiRule.Comments;

            var removed = new List<RuleApplication>();
            var added = new List<RuleApplication>();
            var tailOffset = multiRule.Inner.Count - Inner.Count;
            int firstDifferentIndex = CalculateFirstDifferentIndex(multiRule);
            int lastDifferentIndex = CalculateLastDifferentIndex(multiRule, tailOffset);

            for (int i = firstDifferentIndex; i <= lastDifferentIndex; i++)
            {
                if (i < multiRule.Inner.Count)
                {
                    var old = Inner[i];
                    var newApp = multiRule.Inner[i].ApplyTo(old, context);
                    Inner[i] = newApp;
                    if (old != newApp && old.IsActive)
                    {
                        newApp.Parent = this;
                        old.Deactivate(context);
                        newApp.Activate(context);
                        old.Parent = null;
                    }
                }
                else
                {
                    var old = Inner[i];
                    removed.Add(old);
                    old.Deactivate(context);
                    old.Parent = null;
                    Inner.RemoveAt(i);
                }
            }
            for (int i = 1; i <= tailOffset; i++)
            {
                var item = multiRule.Inner[lastDifferentIndex + i];
                added.Add(item);
                if (IsActive)
                {
                    item.Parent = this;
                    item.Activate(context);
                }
                Inner.Insert(lastDifferentIndex + i, item);
            }
            OnMigrate(removed, added);
            
            return this;
        }

        protected virtual void OnMigrate(List<RuleApplication> removed, List<RuleApplication> added) { }

        private int CalculateLastDifferentIndex(MultiRuleApplication multiRule, int tailOffset)
        {
            var lastIndex = Inner.Count - 1;
            while (lastIndex > 0 && Inner[lastIndex] == multiRule.Inner[lastIndex + tailOffset])
            {
                lastIndex--;
            }

            return lastIndex;
        }

        private int CalculateFirstDifferentIndex(MultiRuleApplication multiRule)
        {
            var index = 0;
            while (index < Inner.Count && index < multiRule.Inner.Count && Inner[index] == multiRule.Inner[index])
            {
                index++;
            }

            return index;
        }

        public override object GetValue(ParseContext context)
        {
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

        /// <inheritdoc />
        public override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
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
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            foreach (var item in Inner)
            {
                item.IterateLiterals(action);
            }
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            foreach(var item in Inner)
            {
                item.IterateLiterals(action, parameter);
            }
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Rule.Write(writer, context, this);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            foreach (var inner in Inner)
            {
                if (inner.CurrentPosition > position)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.Length > position)
                {
                    return inner.GetLiteralAt(position);
                }
            }
            return null;
        }

        /// <inheritdoc />
        internal override void AddInlayEntries(ParseRange range, List<InlayEntry> inlayEntries)
        {
            CheckForInlayEntry(range, inlayEntries);
            foreach (var item in Inner)
            {
                item.AddInlayEntries(range, inlayEntries);
            }
        }
    }
}
