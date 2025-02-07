using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class StarRuleApplication : MultiRuleApplication
    {
        private readonly RuleApplication _stopper;

        public StarRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, RuleApplication stopper, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, inner, endsAt, examinedTo)
        {
            _stopper = stopper;
            stopper.Parent = this;
        }

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, bool ignoreStartPosition)
        {
            var baseSuggestions = base.SuggestCompletions(position, context, ignoreStartPosition);
            if (_stopper.CurrentPosition <= position && _stopper.CurrentPosition + _stopper.ExaminedTo >= position)
            {
                return SuggestCompletions(position, context, ignoreStartPosition, baseSuggestions);
            }
            else if (context.Matcher.IsWhiteSpaceTo(position, _stopper.CurrentPosition) && _stopper.CurrentPosition + _stopper.ExaminedTo >= position)
            {
                return SuggestCompletions(position, context, true, baseSuggestions);
            }
            return baseSuggestions;
        }

        private IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, bool ignoreStartPosition, IEnumerable<string> baseSuggestions)
        {
            var stopperSuggestions = _stopper.SuggestCompletions(position, context, ignoreStartPosition);
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
    }
}
