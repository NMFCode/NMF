using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    public class ModelElementRule<T> : SequenceRule
    {
        protected virtual T CreateElement(IEnumerable<RuleApplication> inner)
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        protected override RuleApplication CreateRuleApplication(List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new ModelElementRuleApplication(this, inner, CreateElement(inner), length, examined);
        }

        private class ModelElementRuleApplication : MultiRuleApplication
        {
            private readonly object _semanticElement;

            public ModelElementRuleApplication(Rule rule, List<RuleApplication> inner, object semanticElement, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
                _semanticElement = semanticElement;
            }

            public override object ContextElement => _semanticElement;

            public override object GetValue(ParseContext context)
            {
                return _semanticElement;
            }
        }
    }
}
