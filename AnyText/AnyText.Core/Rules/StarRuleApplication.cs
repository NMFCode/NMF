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

        public StarRuleApplication(Rule rule, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, inner, length, examinedTo)
        {
            Stopper = stopper;
            if (stopper != null)
            {
                stopper.Parent = this;
            }
        }

        internal override IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, string fragment, ParseContext context, ParsePosition nextTokenPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, fragment, context, nextTokenPosition);
            if (Stopper != null && Stopper.CurrentPosition <= nextTokenPosition && Stopper.CurrentPosition + Stopper.ScopeLength >= position)
            {
                return baseSuggestions.NullsafeConcat(Stopper.SuggestCompletions(position, fragment, context, nextTokenPosition));
            }
            return baseSuggestions;
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            if (Stopper != null)
            {
                var stopperErrors = Stopper.CreateParseErrors().ToList();
                if (stopperErrors.Count > 0)
                {
                    return stopperErrors;
                }
            }
            return base.CreateParseErrors();
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            var lit = base.GetLiteralAt(position);
            if (lit != null || Stopper == null)
            {
                return lit;
            }
            return Stopper.GetLiteralAt(position);
        }

        public override RuleApplication PotentialError => Stopper;

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
            base.IterateLiterals(action);
            if (Stopper != null)
            {
                Stopper.IterateLiterals(action);
            }
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
            base.IterateLiterals(action, parameter);
            if (Stopper != null)
            {
                Stopper.IterateLiterals(action, parameter);
            }
        }
    }
}
