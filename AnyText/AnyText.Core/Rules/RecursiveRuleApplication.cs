using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class RecursiveRuleApplication : RuleApplication
    {
        private const string ErrorMessageDoNotUse = "Recursive markers must not end up in a parse tree";

        public RecursiveRuleApplication(Rule rule, RecursiveRuleApplication other) : base(rule, other.CurrentPosition, other.Length, other.ExaminedTo)
        {
            Continuations = other.Continuations;
        }

        public RecursiveRuleApplication(Rule rule, ParsePosition currentPosition, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, currentPosition, length, examinedTo)
        {
            Continuations = new List<RecursiveContinuation>();
        }

        public override bool IsPositive => false;

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition) => Enumerable.Empty<CompletionEntry>();

        public List<RecursiveContinuation> Continuations { get; }

        public override object GetValue(ParseContext context)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return null;
        }
    }
}
