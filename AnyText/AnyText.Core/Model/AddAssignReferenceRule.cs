using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that adds the value of an inner rule to a collection of the semantic element
    /// </summary>
    /// <typeparam name="TSemanticElement">The type of the context element</typeparam>
    /// <typeparam name="TReference">The type of the property value</typeparam>
    public abstract class AddAssignReferenceRule<TSemanticElement, TReference> : ResolveRule<TSemanticElement, TReference>
    {
        /// <inheritdoc/>
        protected override void Unapply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue)
        {
            GetCollection(contextElement, context).Remove(propertyValue);
        }

        /// <inheritdoc/>
        protected override void Apply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue, bool initial)
        {
            var collection = GetCollection(contextElement, context);
            if (!initial && collection is IList<TReference> list && ruleApplication.Parent != null)
            {
                var index = ruleApplication.Parent.CalculateIndex(ruleApplication);
                if (index >= 0 && index <= list.Count)
                {
                    list.Insert(index, propertyValue);
                }
                else
                {
                    list.Add(propertyValue);
                }
            }
            else
            {
                collection.Add(propertyValue);
            }
        }

        /// <inheritdoc/>
        protected override void Replace(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference oldValue, TReference newValue)
        {
            var collection = GetCollection(contextElement, context);
            if (collection is IList<TReference> list)
            {
                var index = list.IndexOf(oldValue);
                if (index == -1)
                {
                    list.Add(newValue);
                }
                else
                {
                    list[index] = newValue;
                }
            }
            else
            {
                collection.Remove(oldValue);
                collection.Add(newValue);
            }
        }

        /// <summary>
        /// Obtains the child collection
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the parse context in which the collection is obtained</param>
        /// <returns>a collection of values</returns>
        public abstract ICollection<TReference> GetCollection(TSemanticElement semanticElement, ParseContext context);

        /// <summary>
        /// Gets the name of the feature that is assigned
        /// </summary>
        protected abstract string Feature { get; }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, TReference>(Feature, GetCollection, context, out var assigned))
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryConsumeModelToken<TSemanticElement, TReference>(Feature, GetCollection, context, out var assigned))
            {
                return base.Synthesize(GetReferenceString(assigned, parseObject.SemanticElement, context), position, context);
            }
            return new FailedRuleApplication(this, default, $"'{Feature}' of '{semanticElement}' cannot be synthesized");
        }
    }
}
