using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class InheritedMultiFailRuleApplication : RuleApplication
    {
        private IEnumerable<RuleApplication> _innerFailures;

        public InheritedMultiFailRuleApplication(Rule rule, IEnumerable<RuleApplication> inner, ParsePosition currentPosition, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, currentPosition, length, examinedTo)
        {
            _innerFailures = inner;
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            if (other is InheritedMultiFailRuleApplication inn)
            {
                _innerFailures = inn._innerFailures;
            }
            EnsurePosition(CurrentPosition, false);
            return this;
        }

        public override IEnumerable<ParseError> CreateParseErrors()
        {
            return _innerFailures.SelectMany(e => e.CreateParseErrors());
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<string> SuggestCompletions(ParseContext context, ParsePosition position)
        {
            //TODO: SuggestCompletion anfügen
            foreach (var innerFail in _innerFailures)
            {
                if (innerFail is FailedRuleApplication failedRuleApplication)
                    yield return failedRuleApplication.FailedLiteral;
            }
        }
    }
}
