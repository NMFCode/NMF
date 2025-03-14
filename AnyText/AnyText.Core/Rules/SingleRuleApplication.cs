﻿using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class SingleRuleApplication : RuleApplication
    {
        public SingleRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, (inner?.CurrentPosition).GetValueOrDefault(), endsAt, examinedTo)
        {
            Inner = inner;
            if (inner != null)
            {
                inner.Parent = this;
            }
        }

        public override void Validate(ParseContext context)
        {
            Inner.Validate(context);
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return Inner.CreateParseErrors();
        }

        public RuleApplication Inner { get; private set; }

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, context, nextTokenPosition);
            if (Inner.CurrentPosition <= nextTokenPosition && Inner.CurrentPosition + Inner.ExaminedTo >= position
                && Inner.SuggestCompletions(position, context, nextTokenPosition) is var innerSuggestions && innerSuggestions != null)
            {
                if (suggestions == null)
                {
                    return innerSuggestions;
                }
                else
                {
                    return suggestions.Concat(innerSuggestions);
                }
            }
            return suggestions;
        }

        public override void Activate(ParseContext context)
        {
            if (Inner != null && !Inner.IsActive)
            {
                Inner.Parent = this;
                Inner.Activate(context);
            }
            base.Activate(context);
        }

        public override RuleApplication FindChildAt(ParsePosition position, Rule rule)
        {
            return position == Inner.CurrentPosition && rule == Inner.Rule ? Inner : null;
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other.MigrateTo(this, context);
        }

        public override RuleApplication PotentialError => Inner?.PotentialError;

        public override void Shift(ParsePositionDelta shift, int originalLine)
        {
            base.Shift(shift, originalLine);
            Inner?.Shift(shift, originalLine);
        }

        public override void Deactivate(ParseContext context)
        {
            if (Inner != null && Inner.IsActive)
            {
                Inner.Parent = null;
                Inner.Deactivate(context);
            }
            base.Deactivate(context);
        }

        /// <inheritdoc />
        public override void AddCodeLenses(ICollection<CodeLensApplication> codeLenses, Predicate<RuleApplication> predicate = null)
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
            EnsurePosition(singleRule.CurrentPosition, false);
            Length = singleRule.Length;
            ExaminedTo = singleRule.ExaminedTo;
            Comments = singleRule.Comments;

            if (old.Rule == singleRule.Inner.Rule)
            {
                Inner = singleRule.Inner.ApplyTo(Inner, context);
            }
            else
            {
                Inner = singleRule.Inner;
            }
            if (old != Inner)
            {
                Inner.Parent = this;
                old.Parent = null;
                OnMigrate(old, Inner, context);
            }

            return this;
        }

        protected virtual void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParseContext context)
        {
            if (oldValue.IsActive)
            {
                oldValue.Deactivate(context);
                newValue.Activate(context);
                OnValueChange(this, context);
            }
        }

        public override object GetValue(ParseContext context)
        {
            return Inner?.GetValue(context);
        }

        /// <inheritdoc />
        public override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            Inner.AddDocumentSymbols(context, result);
        }

        /// <inheritdoc />
        public override void AddFoldingRanges(ICollection<FoldingRange> result)
        {
            base.AddFoldingRanges(result);
            Inner.AddFoldingRanges(result);
        }

        /// <inheritdoc />
        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            Inner.IterateLiterals(action);
        }

        /// <inheritdoc />
        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            Inner.IterateLiterals(action, parameter);
        }

        /// <inheritdoc />
        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            Rule.Write(writer, context, this);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return Inner.GetLiteralAt(position);
        }
    }
}
