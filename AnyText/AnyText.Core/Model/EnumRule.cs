using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes an enumeration rule
    /// </summary>
    /// <typeparam name="TEnum">the type of the enumeration</typeparam>
    public class EnumRule<TEnum> : ChoiceRule
    {
        /// <summary>
        /// Gets the enum values assigned to the alternatives
        /// </summary>
        public TEnum[] Values { get; set; }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return semanticElement is TEnum en && Values.Contains(en);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is TEnum en)
            {
                var index = Array.IndexOf(Values, en);
                if (index != -1)
                {
                    var inner = Alternatives[index].Rule.Synthesize(en, position, context);
                    if (inner.IsPositive)
                    {
                        return CreateRuleApplication(inner, default);
                    }
                }
            }
            return new FailedRuleApplication(this, position, default, $"{semanticElement} is not a valid enum. Allowed values are {string.Join(", ", Values)}");
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            yield return new EnumSynthesisRequirement(Values);
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(RuleApplication match, ParsePositionDelta examined)
        {
            return new EnumRuleApplication(this, match, match.Length, examined);
        }

        private sealed class EnumRuleApplication : SingleRuleApplication
        {
            public EnumRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            public override object GetValue(ParseContext context)
            {
                var rule = (EnumRule<TEnum>)Rule;
                for (int i = 0; i < rule.Alternatives.Length; i++)
                {
                    if (rule.Alternatives[i].Rule == Inner.Rule)
                    {
                        return rule.Values[i];
                    }
                }
                return null;
            }
        }

        private sealed class EnumSynthesisRequirement : SynthesisRequirement
        {
            private readonly TEnum[] _values;

            public EnumSynthesisRequirement(TEnum[] values)
            {
                _values = values;
            }

            public override bool Matches(object semanticObject)
            {
                return semanticObject is TEnum en && _values.Contains(en);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<string> SuggestCompletions()
        {
            if (Alternatives == null || Alternatives.Length == 0 || Values == null || Values.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            if (Alternatives.Length != Values.Length)
            {
                throw new InvalidOperationException("The number of alternatives and enum values must match.");
            }

            var completions = new List<string>();
            for (int i = 0; i < Alternatives.Length; i++)
            {
                var ruleCompletions = Alternatives[i]?.SuggestCompletions() ?? Enumerable.Empty<string>();
                foreach (var suggestion in ruleCompletions)
                {
                    completions.Add($"{Values[i]}: {suggestion}");
                }
            }

            return completions.Distinct();
        }
    }
}
