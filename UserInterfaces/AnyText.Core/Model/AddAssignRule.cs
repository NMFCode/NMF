﻿using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that adds the value of an inner rule to a collection of the semantic element
    /// </summary>
    /// <typeparam name="TSemanticElement">The type of the context element</typeparam>
    /// <typeparam name="TProperty">The type of the property value</typeparam>
    public abstract class AddAssignRule<TSemanticElement, TProperty> : QuoteRule
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParsePosition position, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                GetCollection(contextElement, context).Add(propertyValue);
            }
            else
            {
                context.Errors.Add(new ParseError(ParseErrorSources.Grammar, position, application.Length, $"Element is not of expected type {typeof(TSemanticElement).Name}"));
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                GetCollection(contextElement, context).Remove(propertyValue);
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context)
        {
            // value change already handled in rule application
            return application.ContextElement is TSemanticElement;
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(RuleApplication app, ParseContext context)
        {
            return new Application(this, app, app.Length, app.ExaminedTo);
        }

        /// <summary>
        /// Gets the name of the feature that is assigned
        /// </summary>
        protected abstract string Feature { get; }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, TProperty>(Feature, GetCollection, null, out var assigned))
            {
                return !EqualityComparer<TProperty>.Default.Equals(assigned, default);
            }
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryConsumeModelToken<TSemanticElement, TProperty>(Feature, GetCollection, context, out var assigned))
            {
                return base.Synthesize(assigned, position, context);
            }
            return new FailedRuleApplication(this, position, default, position, Feature);
        }


        /// <summary>
        /// Obtains the child collection
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the parse context in which the collection is obtained</param>
        /// <returns>a collection of values</returns>
        public abstract ICollection<TProperty> GetCollection(TSemanticElement semanticElement, ParseContext context);

        private sealed class Application : SingleRuleApplication
        {
            public Application(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            protected override void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParsePosition position, ParseContext context)
            {
                if (oldValue.IsActive)
                {
                    oldValue.Deactivate(context);
                    newValue.Activate(context, position);
                    if (Rule is AddAssignRule<TSemanticElement, TProperty> addAssignRule && ContextElement is TSemanticElement contextElement)
                    {
                        var collection = addAssignRule.GetCollection(contextElement, context);
                        if (oldValue.GetValue(context) is TProperty oldProperty)
                        {
                            collection.Remove(oldProperty);
                        }
                        if (newValue.GetValue(context) is TProperty newProperty)
                        {
                            collection.Add(newProperty);
                        }
                    }
                    else
                    {
                        Rule.OnValueChange(this, context);
                    }
                }
            }
        }
    }
}