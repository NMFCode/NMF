using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText.Rules
{
    internal class FailedSequenceRuleApplication : InheritedFailRuleApplication
    {
        private readonly List<RuleApplication> _successfulApplications;

        public FailedSequenceRuleApplication(Rule rule, RuleApplication fail, List<RuleApplication> innerSuccesses, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, fail, examinedTo)
        {
            _successfulApplications = innerSuccesses;
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

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, ParsePosition endPosition, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            if (_successfulApplications.Count > 0)
            {
                var last = _successfulApplications.Last();
                if (position > last.CurrentPosition)
                {
                    if (last.CurrentPosition + last.SuccessfulLength > _innerFail.CurrentPosition)
                    {
                        return last.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition);
                    }
                    else if (position > last.CurrentPosition + last.ExaminedTo)
                    {
                        return _innerFail.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition);
                    }
                    return _innerFail.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition)
                        .NullsafeConcat(last.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition));
                }
            }
            var suggestions = base.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition);
            foreach (var inner in _successfulApplications)
            {
                if (inner.CurrentPosition >= nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ScopeLength >= position)
                {
                    suggestions = suggestions.NullsafeConcat(inner.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition));
                }
            }
            return suggestions;
        }

        public override void AddParseErrors(ParseContext context)
        {
            if (_successfulApplications.Count > 0)
            {
                _successfulApplications.Last().AddParseErrors(context);
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

        public override ParsePositionDelta SuccessfulLength
        {
            get
            {
                var pos = CurrentPosition;
                var max = pos;
                if (_successfulApplications.Count > 0)
                {
                    var last = _successfulApplications.Last();
                    max = last.CurrentPosition + last.Length;
                }
                var successBeforeFail = _innerFail.CurrentPosition + _innerFail.SuccessfulLength;
                if (successBeforeFail > max)
                {
                    max = successBeforeFail;
                }
                return max - pos;
            }
        }
    }
}
