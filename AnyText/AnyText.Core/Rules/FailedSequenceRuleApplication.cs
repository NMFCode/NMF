using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class FailedSequenceRuleApplication : InheritedMultiFailRuleApplication
    {
        private readonly List<RuleApplication> _successfulApplications;

        public FailedSequenceRuleApplication(Rule rule, IEnumerable<RuleApplication> innerFails, List<RuleApplication> innerSuccesses, ParsePosition currentPosition, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, innerFails, currentPosition, length, examinedTo)
        {
            _successfulApplications = innerSuccesses;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            foreach (var ruleApplication in _successfulApplications)
            {
                ruleApplication.IterateLiterals(action);
            }
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            foreach (var ruleApplication in _successfulApplications)
            {
                ruleApplication.IterateLiterals(action, parameter);
            }
        }
        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            foreach (var inner in _successfulApplications)
            {
                if (inner.CurrentPosition > position)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.Length > position)
                {
                    return inner.GetLiteralAt(position);
                }
            }
            return null;
        }
    }
}
