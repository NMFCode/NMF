using NMF.AnyText.Rules;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that catches whether a given rule matched
    /// </summary>
    /// <typeparam name="TSemanticElement"></typeparam>
    public abstract class ExistsAssignRule<TSemanticElement> : QuoteRule
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement semanticElement)
            {
                SetValue(semanticElement, application.IsPositive, context);
            }
            else
            {
                context.AddDiagnosticItem(new DiagnosticItem(DiagnosticSources.Grammar, application, $"Element is not of expected type {typeof(TSemanticElement).Name}"));
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement semanticElement)
            {
                SetValue(semanticElement, false, context);
            }
        }

        /// <summary>
        /// Assigns the value to the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="value">the value to assign</param>
        /// <param name="context">the parse context</param>
        protected abstract void SetValue(TSemanticElement semanticElement, bool value, ParseContext context);

        /// <summary>
        /// Gets the value of the given property
        /// </summary>
        /// <param name="semanticElement">the context element</param>
        /// <param name="context">the parsing context</param>
        /// <returns>the property value</returns>
        protected abstract bool GetValue(TSemanticElement semanticElement, ParseContext context);

        /// <summary>
        /// Gets the name of the feature that is assigned
        /// </summary>
        protected abstract string Feature { get; }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, bool>(Feature, GetValue, context, out var assigned))
            {
                return assigned;
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryConsumeModelToken<TSemanticElement, bool>(Feature, GetValue, context, out var assigned) && assigned)
            {
                return base.Synthesize(semanticElement, position, context);
            }
            return new FailedRuleApplication(this, default, $"'{Feature}' of '{semanticElement}' cannot be synthesized");
        }
    }
}
