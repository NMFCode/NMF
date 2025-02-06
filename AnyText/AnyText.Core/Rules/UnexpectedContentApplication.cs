using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class UnexpectedContentApplication : FailedRuleApplication
    {
        private RuleApplication _inner;

        public UnexpectedContentApplication(Rule rule, RuleApplication inner, ParsePosition currentPosition, ParsePositionDelta examinedTo, string message) : base(rule, currentPosition, examinedTo, message)
        {
            _inner = inner;
        }

        internal override bool IsUnexpectedContent => true;

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            _inner.IterateLiterals(action);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            _inner.IterateLiterals(action, parameter);
        }
    }
}
