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

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, bool ignoreStartPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, context, ignoreStartPosition);
            if (Stopper.CurrentPosition <= position && Stopper.CurrentPosition + Stopper.ExaminedTo >= position)
            {
                return SuggestCompletions(position, context, ignoreStartPosition, baseSuggestions);
            }
            else if (context.Matcher.IsWhiteSpaceTo(position, Stopper.CurrentPosition) && Stopper.CurrentPosition + Stopper.ExaminedTo >= position)
            {
                return SuggestCompletions(position, context, true, baseSuggestions);
            }
            return baseSuggestions;
        }

        private IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, bool ignoreStartPosition, IEnumerable<string> baseSuggestions)
        {
            var stopperSuggestions = Stopper.SuggestCompletions(position, context, ignoreStartPosition);
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
