using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class StarRuleApplication : MultiRuleApplication
    {
        public RuleApplication Stopper { get; }

        public StarRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
        {
            Stopper = stopper;
            stopper.Parent = this;
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            if (Stopper.CurrentPosition <= nextTokenPosition && Stopper.CurrentPosition + Stopper.ScopeLength >= position)
            {
                return baseSuggestions.NullsafeConcat(Stopper.SuggestCompletions(position, fragment, context, nextTokenPosition));
            }
            return baseSuggestions;
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            var stopperErrors = Stopper.CreateParseErrors().ToList();
            if (stopperErrors.Count > 0)
            {
                return stopperErrors;
            }
            return base.CreateParseErrors();
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            var lit = base.GetLiteralAt(position);
            if (lit != null)
            {
                return lit;
            }
            return Stopper.GetLiteralAt(position);
        }

        public override RuleApplication PotentialError => Stopper;

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            base.IterateLiterals(action);
            Stopper.IterateLiterals(action);
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            base.IterateLiterals(action, parameter);
            Stopper.IterateLiterals(action, parameter);
        }
    }
}
