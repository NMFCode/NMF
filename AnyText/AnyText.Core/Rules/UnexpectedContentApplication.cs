using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class UnexpectedContentApplication : FailedRuleApplication
    {
        private readonly RuleApplication _inner;

        public UnexpectedContentApplication(Rule rule, RuleApplication inner, ParsePosition currentPosition, ParsePositionDelta examinedTo, string message) : base(rule, currentPosition, examinedTo, message)
        {
            _inner = inner;
        }

        internal override bool IsUnexpectedContent => true;

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            return _inner.SuggestCompletions(position, context, nextTokenPosition);
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            _inner.IterateLiterals(action);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            _inner.IterateLiterals(action, parameter);
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return _inner.CreateParseErrors().Concat(base.CreateParseErrors());
        }
    }
}
