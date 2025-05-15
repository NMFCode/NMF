using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.AnyText.Rules;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.UniversalVariability
{
    partial class UniversalVariabilityGrammar
    {
        partial class FeatureConstraintFeatureFeatureRule
        {
            protected override bool TryResolveReference(IFeatureConstraint contextElement, string input, ParseContext context, out IFeature? resolved)
            {
                resolved = null;
                var featureModel = contextElement.Ancestors().OfType<FeatureModel>().FirstOrDefault();
                if (featureModel != null)
                {
                    resolved = featureModel.Descendants().OfType<Feature>().FirstOrDefault(f => f.Name == input);
                    return resolved != null;
                }
                return false;
            }
        }

        protected override ParseContext CreateParseContext()
        {
            return new UniversalVariabilityParseContext(this);
        }
    }

    public class UniversalVariabilityParseContext : ModelParseContext
    {
        public UniversalVariabilityParseContext(Grammar grammar, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) : base(grammar, stringComparison)
        {
        }

        protected override bool AcceptOneOrMoreAdd(OneOrMoreRule rule, RuleApplication toAdd, List<RuleApplication> added)
        {
            var sameColumn = added.Count == 0 || toAdd.Length == default || toAdd.CurrentPosition.Col == added[added.Count - 1].CurrentPosition.Col;
            if (!sameColumn)
            {

            }
            return sameColumn;
        }

        protected override bool AcceptZeroOrMoreAdd(ZeroOrMoreRule star, RuleApplication toAdd, List<RuleApplication> added)
        {
            var sameColumn = added.Count == 0 || toAdd.Length == default || toAdd.CurrentPosition.Col == added[added.Count - 1].CurrentPosition.Col;
            if (!sameColumn)
            {

            }
            return sameColumn;
        }

        protected override bool AcceptSequenceAdd(SequenceRule sequence, ref RuleApplication toAdd, List<RuleApplication> added)
        {
            var result = added.Count == 0 || toAdd.Length == default || toAdd.CurrentPosition.Col >= added[0].CurrentPosition.Col;
            if (!result && toAdd.Rule.IsEpsilonAllowed())
            {
                var app = toAdd.Rule.CreateEpsilonRuleApplication(toAdd);
                toAdd = app;
            }
            return true;
        }
    }
}