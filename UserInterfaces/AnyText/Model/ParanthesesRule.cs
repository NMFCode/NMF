using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    public class ParanthesesRule : SequenceRule
    {
        protected override RuleApplication CreateRuleApplication(List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ParanthesesRuleApplication(this, inner, length, examined);
        }

        private class ParanthesesRuleApplication : MultiRuleApplication
        {
            public ParanthesesRuleApplication(Rule rule, List<RuleApplication> inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
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
        }
    }
}
