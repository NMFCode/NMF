﻿using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private sealed class ParanthesesRuleApplication : MultiRuleApplication
        {
            public ParanthesesRuleApplication(Rule rule, ParsePosition currentPosition, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, currentPosition, inner, endsAt, examinedTo)
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

            internal override RuleApplication MigrateTo(MultiRuleApplication multiRule, ParsePosition position, ParseContext context)
            {
                if (Inner.Count <= 0) return this;
                Inner[0] = multiRule.Inner[0].ApplyTo(Inner[0], position, context);
                position += Inner[0].Length;

                if (Inner.Count == 1) return this;
                var current = Inner[1];
                var newValue = multiRule.Inner[1].ApplyTo(current, position, context);
                if (current != newValue)
                {
                    Inner[1] = newValue;
                    Parent.OnValueChange(this, context);
                }
                position += newValue.Length;


                if (Inner.Count == 2) return this;
                Inner[2] = multiRule.Inner[2].ApplyTo(Inner[2], position, context);
                return this;
            }
        }
    }
}