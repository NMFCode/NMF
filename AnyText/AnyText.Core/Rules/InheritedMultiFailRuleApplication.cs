using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal class InheritedMultiFailRuleApplication : RuleApplication
    {
        private IEnumerable<RuleApplication> _innerFailures;

        public InheritedMultiFailRuleApplication(Rule rule, IEnumerable<RuleApplication> inner, ParsePosition currentPosition, ParsePositionDelta length, ParsePositionDelta examinedTo) : base(rule, currentPosition, length, examinedTo)
        {
            _innerFailures = inner;
            foreach (var innerFail in inner)
            {
                innerFail.Parent = this;
            }
        }

        public override IEnumerable<string> SuggestCompletions(ParsePosition position, ParseContext context, ParsePosition nextTokenPosition)
        {
            var suggestions = base.SuggestCompletions(position, context, nextTokenPosition);
            foreach (var inner in _innerFailures)
            {
                if (inner.CurrentPosition > nextTokenPosition)
                {
                    break;
                }
                if (inner.CurrentPosition + inner.ExaminedTo > position
                    && inner.SuggestCompletions(position, context, nextTokenPosition) is var innerSuggestions && innerSuggestions != null)
                {
                    if(suggestions == null)
                    {
                        suggestions = new List<string>();
                    } 
                    suggestions = suggestions.Concat(innerSuggestions);
                }
            }
            return suggestions;
        }

        /// <inheritdoc />
        public override bool IsPositive => false;

        /// <inheritdoc />
        public override RuleApplication ApplyTo(RuleApplication other, ParseContext context)
        {
            if (other is InheritedMultiFailRuleApplication inn)
            {
                _innerFailures = inn._innerFailures;
            }
            EnsurePosition(CurrentPosition, false);
            return this;
        }

        public override IEnumerable<DiagnosticItem> CreateParseErrors()
        {
            return _innerFailures.SelectMany(e => e.CreateParseErrors());
        }

        public override object GetValue(ParseContext context)
        {
            return null;
        }

        public override void IterateLiterals(Action<LiteralRuleApplication> action)
        {
        }

        public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
        {
        }

        public override void Write(PrettyPrintWriter writer, ParseContext context)
        {
        }

        public override RuleApplication GetLiteralAt(ParsePosition position)
        {
            return null;
        }

        public override RuleApplication PotentialError => this;
    }
}
