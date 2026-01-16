using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText.Rules
{
    internal class SingleRuleApplication : RuleApplication
    {
        public SingleRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            Inner = inner;
            if (inner != null)
            {
                IsRecovered = inner.IsRecovered;
            }
        }

        public override void Validate(ParseContext context)
        {
            Inner.Validate(context);
        }

        public override void AddParseErrors(ParseContext context)
        {
            Inner?.AddParseErrors(context);
        }

        public RuleApplication Inner { get; private set; }

        public override IEnumerable<RuleApplication> Children => Enumerable.Repeat(Inner, Inner != null ? 1 : 0);

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            if (Inner.CurrentPosition <= nextTokenPosition && Inner.CurrentPosition + Inner.ScopeLength >= position)
            {
                return suggestions.NullsafeConcat(Inner.SuggestCompletions(position, fragment, context, nextTokenPosition));
            }
            return suggestions;
        }

        public override void Activate(ParseContext context, bool initial)
        {
            if (Inner != null && !Inner.IsActive)
            {
                Inner.ChangeParent(this, context);
                Inner.Activate(context, initial);
            }
            base.Activate(context, initial);
        }
        
        public override RuleApplication FindChildAt(ParsePosition position, Rule rule)
        {
            return position == Inner.CurrentPosition && rule == Inner.Rule ? Inner : null;
        }

        public override void ReplaceChild(RuleApplication childApplication, RuleApplication newChild, ParseContext context)
        {
            if (Inner == childApplication)
            {
                Inner = newChild;
                newChild.ChangeParent(this, context);
            } 
            else
            {
                base.ReplaceChild(childApplication, newChild, context);
            }
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        public override RuleApplication PotentialError => Inner?.PotentialError;

        public override void Deactivate(ParseContext context)
        {
            if (Inner != null && Inner.IsActive && Inner.Parent == this)
            {
                Inner.Deactivate(context);
                Inner.ChangeParent(null, context);
            }
            base.Deactivate(context);
        }

        internal override void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
        {
            if (Inner != null)
            {
                Inner.AddCodeLenses(codeLenses, predicate);
            }
            base.AddCodeLenses(codeLenses, predicate);
        }
        internal override RuleApplication MigrateTo(SingleRuleApplication singleRule, ParseContext context)
        {
            if (singleRule.Rule != Rule)
            {
                return base.MigrateTo(singleRule, context);
            }
            var old = Inner;
            Length = singleRule.Length;
            ExaminedTo = singleRule.ExaminedTo;
            Comments = singleRule.Comments;
            singleRule.ReplaceWith(this);
            if (old.Rule == singleRule.Inner.Rule)
            {   
                var singleRuleInner = singleRule.Inner;
                Inner = singleRuleInner.ApplyTo(Inner, context);
            }
            else
            {
                Inner = singleRule.Inner;
            }
            if (old != Inner)
            {
                Inner.ChangeParent(this, context);
                OnMigrate(old, Inner, context);
            }

            return this;
        }
        protected virtual void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParseContext context)
        {
            if (oldValue.IsActive && oldValue.Parent == this)
            {
                oldValue.Deactivate(context);
                oldValue.ChangeParent(null, context);
                newValue.Activate(context, false);
            }
            if(newValue.IsPositive)
                OnValueChange(this, context, oldValue);
        }

        public override object GetValue(ParseContext context)
        {
            return Inner?.GetValue(context);
        }

        internal override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            Inner.AddDocumentSymbols(context, result);
        }

        internal override void AddInlayEntries(ParseRange range, List<InlayEntry> inlayEntries, ParseContext context)
        {
            CheckForInlayEntry(range, inlayEntries, context);
            Inner.AddInlayEntries(range, inlayEntries, context);
        }

        internal override void AddFoldingRanges(ICollection<FoldingRange> result)
        {
            base.AddFoldingRanges(result);
            Inner.AddFoldingRanges(result);
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            Inner.IterateLiterals(action, includeFailures);
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            Inner.IterateLiterals(action, parameter, includeFailures);
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Rule.Write(writer, context, this);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            return Inner.GetLiteralAt(position, onlyActive);
        }

        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            return Inner.GetFirstInnerLiteral();
        }

        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            return Inner.GetLastInnerLiteral();
        }
    }
}
