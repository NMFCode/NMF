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

        public RecursiveRuleApplication(Rule rule, RecursiveRuleApplication other) : base(rule, other.Length, other.ExaminedTo)
        {
            Continuations = other.Continuations;
            Dependents = other.Dependents;
            Dependents.Add(this);
        }

        public RecursiveRuleApplication(Rule rule, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, length, examinedTo)
        {
            Continuations = new List<RecursiveContinuation>();
            Dependents = new List<RuleApplication>();
        }

        public override bool IsPositive => false;

        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition) => Enumerable.Empty<CompletionEntry>();

        public List<RecursiveContinuation> Continuations { get; }
        public List<RuleApplication> Dependents { get; }

        public void Resolve(ParseContext context, ParsePosition position)
        {
            foreach (var dep in Dependents)
            {
                var positionCopy = position;
                var newDep = dep.Rule.Match(context, ref positionCopy);
                dep.ReplaceWith(newDep);
            }
        }

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

        public override LiteralRuleApplication GetFirstInnerLiteral()
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }

        public override LiteralRuleApplication GetLastInnerLiteral()
        {
            throw new InvalidOperationException(ErrorMessageDoNotUse);
        }
    }
}
