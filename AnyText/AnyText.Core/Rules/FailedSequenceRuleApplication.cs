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

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            if (includeFailures)
            {
                foreach (var ruleApplication in _successfulApplications)
                {
                    ruleApplication.IterateLiterals(action, true);
                }
            }
        }

        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            if (Rule is SequenceRule sequence)
            {
                var recoveryOfFailure = _innerFail.Recover(currentRoot, context, out position);
                if (recoveryOfFailure != _innerFail && recoveryOfFailure.IsPositive)
                {
                    var recovered = sequence.Recover(_innerFail.CurrentPosition, _successfulApplications, context);
                    ReplaceWith(recovered);
                    return recovered;
                }
                if (sequence.Rules[sequence.Rules.Length - 1].Rule is LiteralRule stopLiteral)
                {
                    var pos = _innerFail.CurrentPosition;
                    var lineNo = pos.Line;
                    var col = pos.Col;
                    while (lineNo < context.Input.Length)
                    {
                        var startIndex = context.Input[lineNo].IndexOf(stopLiteral.Literal, col, context.StringComparison);
                        if (startIndex != -1)
                        {
                            position = new ParsePosition(lineNo, startIndex);
                            var stopper = context.Matcher.MatchCore(stopLiteral, null, context, ref position);
                            if (stopper is LiteralRuleApplication stopperLiteral)
                            {
                                var recovered = new RecoveredSequenceRuleApplication(Rule, _successfulApplications, _innerFail, stopperLiteral, position - CurrentPosition, position - CurrentPosition);
                                ReplaceWith(recovered);
                                return recovered;
                            }
                        }
                        lineNo++;
                        col = 0;
                    }
                }
            }
            position = CurrentPosition;
            return this;
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            if (includeFailures)
            {
                foreach (var ruleApplication in _successfulApplications)
                {
                    ruleApplication.IterateLiterals(action, parameter, true);
                }
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

        public override void AddParseErrors(ParseContext context)
        {
            if (_furtherFails != null)
            {
                foreach (var fail in _furtherFails)
                {
                    fail.AddParseErrors(context);
                }
            }
            base.AddParseErrors(context);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            if (onlyActive)
            {
                return null;
            }

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
