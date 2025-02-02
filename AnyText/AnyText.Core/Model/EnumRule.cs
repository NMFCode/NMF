﻿using NMF.AnyText.Rules;
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
        public override bool CanSynthesize(object semanticElement)
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
                    var inner = Alternatives[index].Synthesize(en, position, context);
                    if (inner.IsPositive)
                    {
                        return CreateRuleApplication(inner, default);
                    }
                }
            }
            return new FailedRuleApplication(this, position, default, position, "Not a valid enum");
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
                var index = Array.IndexOf(rule.Alternatives, Inner.Rule);
                return rule.Values[index];
            }
        }
    }
}
