using NMF.AnyText.Model;
using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
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

        public MultiRuleApplication(Rule rule, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, endsAt, examinedTo)
        {
            Inner = inner;
            foreach (var innerApp in inner)
            {
                innerApp.Parent = this;
            }
        }

        public List<RuleApplication> Inner { get; }

        public override IEnumerable<RuleApplication> Children => Inner;

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

        public override void Deactivate(ParseContext context)
        {
            foreach (var inner in Inner)
            {
                if (inner.IsActive)
                {
                    inner.Deactivate(context);
                }
                inner.Parent = null;
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

        public override RuleApplication FindChildAt(ParsePosition position, Rule rule)
        {
            return Inner.FirstOrDefault(c => c.CurrentPosition == position && c.Rule == rule);
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            if (IsRecovered)
            {
                return Inner.Where(r => r.IsRecovered).SelectMany(r => r.CreateParseErrors());
            }
            var last = Inner.LastOrDefault();
            if (last != null)
            {
                return last.CreateParseErrors();
            }
            return Enumerable.Empty<DiagnosticItem>();
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

            var removed = new List<RuleApplication>();
            var added = new List<RuleApplication>();
            var tailOffset = multiRule.Inner.Count - Inner.Count;
            int firstDifferentIndex = CalculateFirstDifferentIndex(multiRule);
            int lastDifferentIndex = CalculateLastDifferentIndex(multiRule, tailOffset);

            int offset = 0;
            int index = 0;
            while (index < firstDifferentIndex)
            {
                Inner[index].Parent = this;
                index++;
            }
            while (index <= lastDifferentIndex)
            {
                if (index < multiRule.Inner.Count)
                {
                    if (tailOffset >= 0 || multiRule.Inner[index].CurrentPosition == Inner[index].CurrentPosition)
                    {
                        MigrateChild(multiRule, context, index, offset);
                    }
                    else
                    {
                        RemoveChild(context, removed, index);
                        tailOffset++;
                        continue;
                    }
                }
                else
                {
                    RemoveChild(context, removed, index);
                }
                index++;
            }
            while (index < Inner.Count)
            {
                Inner[index].Parent = this;
                index++;
            }
            while (tailOffset < 0 && lastDifferentIndex + 1 < Inner.Count)
            {
                RemoveChild(context, removed, lastDifferentIndex + 1);
                tailOffset++;
            }
            if (tailOffset > 0)
            {
                for (int i = 1; i <= tailOffset; i++)
                {
                    InsertChild(multiRule, context, added, lastDifferentIndex, i);
                }
            }
            OnMigrate(removed, added);
            
            return this;
        }

        private void InsertChild(MultiRuleApplication multiRule, ParseContext context, List<RuleApplication> added, int lastDifferentIndex, int i)
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

        private void RemoveChild(ParseContext context, List<RuleApplication> removed, int i)
        {
            if (i < Inner.Count)
            {
                var old = Inner[i];
                removed.Add(old);
                old.Deactivate(context);
                old.Parent = null;
                Inner.RemoveAt(i);
            }
        }

        private void MigrateChild(MultiRuleApplication multiRule, ParseContext context, int index, int offset)
        {
            var old = Inner[index];
            var newApp = multiRule.Inner[index + offset].ApplyTo(old, context);
            Inner[index] = newApp;
            if (old != newApp && old.IsActive)
            {
                newApp.Parent = this;
                old.Deactivate(context);
                newApp.Activate(context);
                old.Parent = null;
            }
        }

        protected virtual void OnMigrate(List<RuleApplication> removed, List<RuleApplication> added) { }

        private int CalculateLastDifferentIndex(MultiRuleApplication multiRule, int tailOffset)
        {
            var lastIndex = Inner.Count - 1;
            var min = Math.Max(0, -tailOffset);
            while (lastIndex > min && Inner[lastIndex] == multiRule.Inner[lastIndex + tailOffset])
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
