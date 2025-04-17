using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class InheritedFailRuleApplication : RuleApplication
    {
        private RuleApplication _innerFail;

        public InheritedFailRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta examinedTo) : base(rule, inner.Length, examinedTo)
        {
            _innerFail = inner;
            inner.Parent = this;
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, context, nextTokenPosition);
            if (_innerFail.CurrentPosition <= nextTokenPosition && _innerFail.CurrentPosition + _innerFail.ExaminedTo >= position
                && _innerFail.SuggestCompletions(position, context, nextTokenPosition) is var innerSuggestions && innerSuggestions != null)
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

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return _innerFail.CreateParseErrors();
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            _innerFail.IterateLiterals(action);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            _innerFail.IterateLiterals(action, parameter);
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return null;
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
