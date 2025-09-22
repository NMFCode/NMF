using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a special sequence rule whose value is the value of the second child
    /// </summary>
    public class ParanthesesRule : SequenceRule
    {
        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ParanthesesRuleApplication(this, currentPosition, inner, length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            synthesisPlan?.BlockRecursion(this, semanticElement);
            return base.CanSynthesize(semanticElement, context, synthesisPlan);
        }

        /// <inheritdoc />
        public override bool HasFoldingKind(out string kind)
        {
            kind = null;
            return true;
        }

        /// <inheritdoc />
        protected internal override void AddLeftRecursionRules(List<Rule> trace, List<RecursiveContinuation> continuations)
        {
        }

        private sealed class ParanthesesRuleApplication : MultiRuleApplication
        {
            public ParanthesesRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            public override object GetValue(ParseContext context)
            {
                if (Inner.Count < 3)
                {
                    return null;
                }
                return Inner[1].GetValue(context);
            }

            internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParseContext context)
            {
                if (multiRule.Rule != Rule)
                {
                    return multiRule;
                }

                Length = multiRule.Length;
                ExaminedTo = multiRule.ExaminedTo;
                Comments = multiRule.Comments;
                multiRule.ReplaceWith(this);

                if (Inner.Count <= 0) return this;
                Inner[0] = multiRule.Inner[0].ApplyTo(Inner[0], context);
                Inner[0].Parent = this;

                if (Inner.Count == 1) return this;
                var current = Inner[1];
                var newValue = multiRule.Inner[1].ApplyTo(current, context);
                if (current != newValue)
                {
                    Inner[1] = newValue;
                    Inner[1].Parent = this;
                    Parent.OnValueChange(this, context, current);
                }


                if (Inner.Count == 2) return this;
                Inner[2] = multiRule.Inner[2].ApplyTo(Inner[2], context);
                Inner[2].Parent = this;
                return this;
            }
        }
    }
}
