using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;

namespace NMF.AnyText.Rules
{
    internal class InheritedFailRuleApplication : RuleApplication
    {
        protected RuleApplication _innerFail;

        public InheritedFailRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta examinedTo) : base(rule, inner.Length, examinedTo)
        {
            _innerFail = inner;
            inner.Parent = this;
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            if (_innerFail.CurrentPosition <= nextTokenPosition && _innerFail.CurrentPosition + _innerFail.ExaminedTo >= position)
            {
                return suggestions.NullsafeConcat(_innerFail.SuggestCompletions(position, fragment, context, nextTokenPosition));
            }
            return suggestions;
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            other.ReplaceWith(this);
            if (other is InheritedFailRuleApplication inn)
            {
                _innerFail = inn._innerFail;
            }
            return this;
        }

        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            var recovered = Rule.Recover(this, _innerFail, currentRoot, context, out position);
            if (recovered.IsPositive)
            {
                recovered.SetRecovered(true);
                ReplaceWith(recovered);
            }
            return recovered;
        }

        public override void AddParseErrors(ParseContext context)
        {
            _innerFail.AddParseErrors(context);
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            if (includeFailures)
            {
                _innerFail.IterateLiterals(action, true);
            }
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            if (includeFailures)
            {
                _innerFail.IterateLiterals(action, parameter, true);
            }
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            return _innerFail.GetLiteralAt(position, onlyActive);
        }

        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            return null;
        }

        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            return null;
        }

        public override RuleApplication PotentialError => this;
    }
}
