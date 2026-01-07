using System;
using System.Collections.Generic;

namespace NMF.AnyText.Rules
{
    internal class StarRuleApplication : MultiRuleApplication
    {
        public RuleApplication Stopper { get; set; }

        public StarRuleApplication(Rule rule, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, inner, length, examinedTo)
        {
            Stopper = stopper;
            if (stopper != null && !stopper.IsPositive)
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

        public override void AddParseErrors(ParseContext context)
        {
            Stopper?.AddParseErrors(context);
            base.AddParseErrors(context);
        }

        public override RuleApplication GetLiteralAt(ParsePosition position, bool onlyActive = false)
        {
            var lit = base.GetLiteralAt(position, onlyActive);
            if (lit != null || Stopper == null)
            {
                return lit;
            }
            return Stopper.GetLiteralAt(position, onlyActive);
        }

        public override RuleApplication PotentialError => Stopper;

        public override void IterateLiterals(Action<LiteralRuleApplication> action, bool includeFailures)
        {
            base.IterateLiterals(action, true);
            if (Stopper != null && includeFailures)
            {
                Stopper.IterateLiterals(action, true);
            }
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter, bool includeFailures)
        {
            base.IterateLiterals(action, parameter, includeFailures);
            if (Stopper != null && includeFailures)
            {
                Stopper.IterateLiterals(action, parameter, true);
            }
        }

        public override RuleApplication Recover(RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            var recovered = Stopper.Recover(currentRoot, context, out position);
            if (recovered.IsPositive)
            {
                position = Stopper.CurrentPosition;
                var examined = ExaminedTo;
                var applications = new List<RuleApplication>(Inner);
                var isRecovered = true;
                var newStop = RuleHelper.Star(context, null, Stopper.Rule, applications, CurrentPosition, (_,_,_) => true, ref position, ref examined, ref isRecovered);
                var recovery = new StarRuleApplication(Rule, applications, newStop, position - CurrentPosition, examined).SetRecovered(true);
                ReplaceWith(recovery);
                return recovery;
            }
            position = CurrentPosition;
            return this;
        }
    }
}
