using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that assigns the value of a child rule to a certain property
    /// </summary>
    /// <typeparam name="TSemanticElement">The type of the context element</typeparam>
    /// <typeparam name="TProperty">The type of the property value</typeparam>
    public abstract class AssignRule<TSemanticElement, TProperty> : QuoteRule
    {
        private static readonly bool NeedsNullCheck = RuleHelper.CanBeNull(typeof(TProperty));

        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                SetValue(contextElement, propertyValue, context);
            }
            else
            {
                context.Errors.Add(new DiagnosticItem(DiagnosticSources.Grammar, application, $"Element is not of expected type {typeof(TSemanticElement).Name}"));
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                SetValue(contextElement, default, context);
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                SetValue(contextElement, propertyValue, context);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets called when the value changes
        /// </summary>
        /// <param name="semanticElement">the context element</param>
        /// <param name="propertyValue">the property value</param>
        /// <param name="context">the parsing context</param>
        protected abstract void SetValue(TSemanticElement semanticElement, TProperty propertyValue, ParseContext context);

        /// <summary>
        /// Gets the value of the given property
        /// </summary>
        /// <param name="semanticElement">the context element</param>
        /// <param name="context">the parsing context</param>
        /// <returns>the property value</returns>
        protected abstract TProperty GetValue(TSemanticElement semanticElement, ParseContext context);

        /// <summary>
        /// Gets the name of the feature that is assigned
        /// </summary>
        protected abstract string Feature { get; }

        private IEnumerable<SynthesisRequirement> _synthesisRequirements;

        /// <inheritdoc />
        public sealed override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, TProperty>(Feature, GetValue, context, out var assigned))
            {
                return CanSynthesize((TSemanticElement)parseObject.SemanticElement, assigned)
                    && RuleHelper.GetOrCreateSynthesisRequirements(InnerRule, ref _synthesisRequirements).All(r => r.Matches(assigned));
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current rule can synthesize rule applications for the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="propertyValue">the assigned property value</param>
        /// <returns>true, if a rule application can be synthesized, otherwise false</returns>
        protected virtual bool CanSynthesize(TSemanticElement semanticElement, TProperty propertyValue)
        {
            return !NeedsNullCheck || !EqualityComparer<TProperty>.Default.Equals(propertyValue, default);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryConsumeModelToken<TSemanticElement, TProperty>(Feature, GetValue, context, out var assigned))
            {
                return base.Synthesize(assigned, position, context);
            }
            return new FailedRuleApplication(this, position, default, $"'{Feature}' of '{semanticElement}' cannot be synthesized");
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            yield return new AssignRuleSynthesisRequirement( RuleHelper.GetOrCreateSynthesisRequirements(InnerRule, ref _synthesisRequirements), this);
        }

        private sealed class AssignRuleSynthesisRequirement : FeatureSynthesisRequirement
        {
            private readonly AssignRule<TSemanticElement, TProperty> _rule;

            public AssignRuleSynthesisRequirement(IEnumerable<SynthesisRequirement> inner, AssignRule<TSemanticElement, TProperty> rule) : base(inner)
            {
                _rule = rule;
            }

            public override string Feature => _rule.Feature;

            protected override object Peek(ParseObject parseObject)
            {
                if (parseObject.TryPeekModelToken<TSemanticElement, TProperty>(_rule.Feature, _rule.GetValue, null, out var assigned))
                {
                    return assigned;
                }
                return null;
            }

            internal override void PlaceReservations(ParseObject semanticObject)
            {
                semanticObject.Reserve<TSemanticElement, TProperty>(_rule.Feature, _rule.GetValue, null);
            }
        }

    }
}
