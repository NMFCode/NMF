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

        public StarRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, inner, endsAt, examinedTo)
        {
            Stopper = stopper;
            stopper.Parent = this;
        }

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, context, nextTokenPosition);
            if (Stopper.CurrentPosition <= nextTokenPosition && Stopper.CurrentPosition + Stopper.ExaminedTo >= position)
            {
                return SuggestCompletions(position, context, nextTokenPosition, baseSuggestions);
            }
            return baseSuggestions;
        }

        private IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition, IEnumerable<string> baseSuggestions)
        {
            var stopperSuggestions = Stopper.SuggestCompletions(position, context, nextTokenPosition);
            if (stopperSuggestions == null)
            {
                return baseSuggestions;
            }
            if (baseSuggestions != null)
            {
                return baseSuggestions.Concat(stopperSuggestions);
            }
            else
            {
                return stopperSuggestions;
            }
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

        public override RuleApplication PotentialError => Stopper;
    }
}
