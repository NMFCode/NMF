using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class RecoveredSequenceRuleApplication : RuleApplication
    {
        private readonly List<RuleApplication> _successfulApplications;
        private readonly RuleApplication _innerFail;
        private readonly LiteralRuleApplication _stopper;

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

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return _innerFail.CreateParseErrors().Select(AdjustError);
        }

        private DiagnosticItem AdjustError(DiagnosticItem error)
        {
            var length = _stopper.CurrentPosition - _innerFail.CurrentPosition;
            return new CustomLengthDiagnosticItem(error.Source, _innerFail, length, $"Failed to parse content: {error.Message}", error.Severity);
        }

        private class CustomLengthDiagnosticItem : DiagnosticItem
        {
            public CustomLengthDiagnosticItem(string source, RuleApplication ruleApplication, ParsePositionDelta length, string message, DiagnosticSeverity severity = DiagnosticSeverity.Error) : base(source, ruleApplication, message, severity)
            {
                _length = length;
            }

            private readonly ParsePositionDelta _length;

            public override ParsePositionDelta Length => _length;
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

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                var literal = _successfulApplications[i].GetLiteralAt(position);
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

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                _successfulApplications[i].IterateLiterals(action);
            }
            action?.Invoke(_stopper);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            for (int i = 0; i < _successfulApplications.Count; i++)
            {
                _successfulApplications[i].IterateLiterals(action, parameter);
            }
            action?.Invoke(_stopper, parameter);
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

    }
}
