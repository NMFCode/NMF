using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that assigns the resolved value of a child rule to a certain property
    /// </summary>
    /// <typeparam name="TSemanticElement">The type of the context element</typeparam>
    /// <typeparam name="TReference">The type of the property value</typeparam>
    public abstract class AssignReferenceRule<TSemanticElement, TReference> : ResolveRule<TSemanticElement, TReference> where TReference : class
    {
        /// <inheritdoc/>
        protected override void Unapply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue)
        {
            SetValue(contextElement, default, context);
        }

        /// <inheritdoc/>
        protected override void Apply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue)
        {
            SetValue(contextElement, propertyValue, context);
        }

        /// <inheritdoc />
        protected override bool ApplyOverReplace => true;

        /// <summary>
        /// Gets called when the value changes
        /// </summary>
        /// <param name="semanticElement">the context element</param>
        /// <param name="propertyValue">the property value</param>
        /// <param name="context">the parsing context</param>
        protected abstract void SetValue(TSemanticElement semanticElement, TReference propertyValue, ParseContext context);

        /// <summary>
        /// Gets the value of the given property
        /// </summary>
        /// <param name="semanticElement">the context element</param>
        /// <param name="context">the parsing context</param>
        /// <returns>the property value</returns>
        protected abstract TReference GetValue(TSemanticElement semanticElement, ParseContext context);

        /// <summary>
        /// Gets the name of the feature that is assigned
        /// </summary>
        protected abstract string Feature { get; }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, TReference>(Feature, GetValue, context, out var assigned))
            {
                return assigned != null;
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryConsumeModelToken<TSemanticElement, TReference>(Feature, GetValue, context, out var assigned))
            {
                return base.Synthesize(GetReferenceString(assigned, parseObject.SemanticElement, context), position, context);
            }
            return new FailedRuleApplication(this, default, $"'{Feature}' of '{semanticElement}' cannot be synthesized");
        }

        private IEnumerable<SynthesisRequirement> _synthesisRequirements;

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            yield return new AssignReferenceRuleSynthesisRequirement(RuleHelper.GetOrCreateSynthesisRequirements(InnerRule, ref _synthesisRequirements), this);
        }

        private sealed class AssignReferenceRuleSynthesisRequirement : FeatureSynthesisRequirement
        {
            private readonly AssignReferenceRule<TSemanticElement, TReference> _rule;

            public AssignReferenceRuleSynthesisRequirement(IEnumerable<SynthesisRequirement> inner, AssignReferenceRule<TSemanticElement, TReference> rule) : base(inner)
            {
                _rule = rule;
            }

            public override string Feature => _rule.Feature;

            protected override object Peek(ParseObject parseObject)
            {
                if (parseObject.TryPeekModelToken<TSemanticElement, TReference>(_rule.Feature, _rule.GetValue, null, out var assigned))
                {
                    return _rule.GetReferenceString(assigned, parseObject.SemanticElement, null);
                }
                return null;
            }

            internal override void PlaceReservations(ParseObject semanticObject)
            {
                semanticObject.Reserve<TSemanticElement, TReference>(_rule.Feature, _rule.GetValue, null);
            }
        }
    }
}
