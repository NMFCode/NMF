using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class FailedChoiceRuleApplication : RuleApplication
    {
        private IEnumerable<RuleApplication> _innerFailures;

        public FailedChoiceRuleApplication(Rule rule, IEnumerable<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            _innerFailures = inner;
            foreach (var innerFail in inner)
            {
                innerFail.Parent = this;
            }
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            foreach (var inner in _innerFailures)
            {
                if (inner.CurrentPosition > nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo >= position)
                {
                    suggestions = suggestions.NullsafeConcat(inner.SuggestCompletions(position, fragment, context, nextTokenPosition));
                }
            }
            return suggestions;
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            if (other is FailedChoiceRuleApplication inn)
            {
                _innerFailures = inn._innerFailures;
            }
            other.ReplaceWith(this);
            return this;
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return GetRuleApplicationWithFarestExaminationLength()?.CreateParseErrors() ?? Enumerable.Empty<DiagnosticItem>();
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        private RuleApplication GetRuleApplicationWithFarestExaminationLength()
        {
            return _innerFailures.Aggregate(default(RuleApplication), (acc, r) => acc == null || r.ExaminedTo > acc.ExaminedTo ? r : acc);
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            GetRuleApplicationWithFarestExaminationLength()?.IterateLiterals(action);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            GetRuleApplicationWithFarestExaminationLength().IterateLiterals(action, parameter);
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return GetRuleApplicationWithFarestExaminationLength()?.GetLiteralAt(position);
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
