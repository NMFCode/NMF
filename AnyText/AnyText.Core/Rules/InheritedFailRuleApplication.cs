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

        public InheritedFailRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta examinedTo) : base(rule, inner.CurrentPosition, inner.Length, examinedTo)
        {
            _innerFail = inner;
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            EnsurePosition(CurrentPosition, false);
            if (other is InheritedFailRuleApplication inn)
            {
                _innerFail = inn._innerFail;
            }
            return this;
        }

        public override IEnumerable<ParseError> CreateParseErrors()
        {
            return _innerFail.CreateParseErrors();
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
            //SuggestCompletion anfügen
            if (_innerFail is FailedRuleApplication failedRuleApplication)
            {
                yield return failedRuleApplication.FailedLiteral;
            }
        }
    }
}
