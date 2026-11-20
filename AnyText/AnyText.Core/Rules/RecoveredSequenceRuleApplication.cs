using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;

namespace NMF.AnyText.Rules
{
    internal class RecoveredSequenceRuleApplication : RuleApplication
    {
        private readonly List<RuleApplication> _successfulApplications;
        private readonly RuleApplication _innerFail;
        private readonly LiteralRuleApplication _stopper;

        public override ParsePositionDelta SuccessfulLength
        {
            get
            {
                if (_successfulApplications.Count == 0)
                {
                    return default;
                }

                var last = _successfulApplications[_successfulApplications.Count - 1];
                return last.CurrentPosition + last.Length - CurrentPosition;
            }
        }

        public RecoveredSequenceRuleApplication(Rule rule, List<RuleApplication> successfulApplications, RuleApplication innerFail, LiteralRuleApplication stopper, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            _successfulApplications = successfulApplications;
            _stopper = stopper;
            _innerFail = innerFail;

            SetRecovered(true);
        }

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            return other;
        }

        public override void AddParseErrors(ParseContext context)
        {
            if (_successfulApplications.Count > 0)
            {
                var last = _successfulApplications[_successfulApplications.Count - 1];
                last.AddParseErrors(context);
                if (last.CurrentPosition + last.SuccessfulLength <= _innerFail.CurrentPosition)
                {
                    _innerFail.AddParseErrors(context);
                }
            }
            else
            {
                _innerFail.AddParseErrors(context);
            }
        }

        internal override void AddDocumentSymbols(ParseContext context, ICollection<DocumentSymbol> result)
        {
            if (Rule.PassAlongDocumentSymbols)
            {
                foreach (var innerRuleApplication in _successfulApplications)
                {
                    innerRuleApplication.AddDocumentSymbols(context, result);
                }
                return;
            }

            if (Rule.SymbolKind == SymbolKind.Null) return;

            var children = new List<DocumentSymbol>();
            foreach (var innerRuleApplication in _successfulApplications)
            {
                innerRuleApplication.AddDocumentSymbols(context, children);
            }

            AddDocumentSymbol(context, result, children);
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, ParsePosition endPosition, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition);
            foreach (var inner in _successfulApplications)
            {
                if (inner.CurrentPosition >= nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo >= position)
                {
                    suggestions = suggestions.NullsafeConcat(inner.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition));
                }
            }
            if (_innerFail.CurrentPosition <= position && _innerFail.CurrentPosition + _innerFail.ExaminedTo >= position)
            {
                suggestions = suggestions.NullsafeConcat(_innerFail.SuggestCompletions(position, endPosition, fragment, context, nextTokenPosition));
            }
            return suggestions;
        }

        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                var first = _successfulApplications[i].GetFirstInnerLiteral();
                if (first != null)
                {
                    return first;
                }
            }
            return _stopper;
        }

        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            return _stopper;
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                var literal = _successfulApplications[i].GetLiteralAt(position, onlyActive);
                if (literal != null)
                {
                    return literal;
                }
            }
            return null;
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                _successfulApplications[i].IterateLiterals(action, includeFailures);
            }
            action.Invoke(_stopper);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                _successfulApplications[i].IterateLiterals(action, parameter, includeFailures);
            }
            action.Invoke(_stopper, parameter);
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action, ParsePosition from, ParsePosition to, bool includeFailures)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                var app = _successfulApplications[i];
                if (app.CurrentPosition >= from && app.CurrentPosition + app.Length <= to)
                {
                    app.IterateLiterals(action, from, to, includeFailures);
                }
            }
            action.Invoke(_stopper);
        }

        public override bool IterateLiterals(Func<LiteralRuleApplication, bool> action, bool includeFailures)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                if (!_successfulApplications[i].IterateLiterals(action, includeFailures))
                {
                    return false;
                }
            }
            return action.Invoke(_stopper);
        }

        public override bool IterateLiterals(Func<LiteralRuleApplication, bool> action, ParsePosition from, bool includeFailures)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                if (_successfulApplications[i].CurrentPosition >= from && !_successfulApplications[i].IterateLiterals(action, includeFailures))
                {
                    return false;
                }
            }
            return action.Invoke(_stopper);
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            var lastPos = CurrentPosition;
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                _successfulApplications[i].Write(writer, context);
                lastPos = _successfulApplications[i].CurrentPosition + _successfulApplications[i].Length;
            }

            var lineNo = lastPos.Line;
            var col = lastPos.Col;

            while (lineNo < _stopper.CurrentPosition.Line)
            {
                writer.WriteRaw(context.Input[lineNo].Substring(col));
                writer.WriteNewLine();
                lineNo++;
                col = 0;
            }

            writer.WriteRaw(context.Input[lineNo].Substring(0, _stopper.CurrentPosition.Col));

            _stopper.Write(writer, context);
        }

        public override bool ContinueToNextToken(RuleApplication childRuleApplication, PrettyPrintWriter writer, ParseContext context)
        {
            writer.WriteToken(string.Empty, false);
            return true;
        }
    }
}
