using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class FailedSequenceRuleApplication : InheritedFailRuleApplication
    {
        private readonly List<RuleApplication> _successfulApplications;
        private readonly List<RuleApplication> _furtherFails;

        public FailedSequenceRuleApplication(Rule rule, RuleApplication fail, List<RuleApplication> furtherFails, List<RuleApplication> innerSuccesses, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, fail, examinedTo)
        {
            _successfulApplications = innerSuccesses;
            _furtherFails = furtherFails;
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

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            foreach (var inner in _successfulApplications.NullsafeConcat(_furtherFails))
            {
                if (inner.CurrentPosition > nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ScopeLength >= position)
                {
                    suggestions = suggestions.NullsafeConcat(inner.SuggestCompletions(position, fragment, context, nextTokenPosition));
                }
            }
            return suggestions;
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            if (_furtherFails != null)
            {
                return base.CreateParseErrors().Concat(_furtherFails.SelectMany(fail => fail.CreateParseErrors()));
            }
            return base.CreateParseErrors();
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            RuleApplication failedLiteral = null;
            foreach (var inner in _successfulApplications)
            {
                if (inner.CurrentPosition > position)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo > position)
                {
                    var lit = inner.GetLiteralAt(position);
                    if (lit != null)
                    {
                        if (lit.IsPositive)
                        {
                            return lit;
                        }
                        failedLiteral = lit;
                    }
                }
            }
            return base.GetLiteralAt(position) ?? failedLiteral;
        }
    }
}
