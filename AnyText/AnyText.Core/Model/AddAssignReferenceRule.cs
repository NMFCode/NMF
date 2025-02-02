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
    /// <typeparam name="TReference">The type of the property value</typeparam>
    public abstract class AddAssignReferenceRule<TSemanticElement, TReference> : ResolveRule<TReference>
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParsePosition position, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    GetCollection(contextElement, context).Add(propertyValue);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString, position, true));
                }
            }
            else
            {
                context.Errors.Add(new ParseError(ParseErrorSources.Grammar, position, application.Length, $"Element is not of expected type {typeof(TSemanticElement).Name}"));
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    GetCollection(contextElement, context).Remove(propertyValue);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString, default, false));
                }
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

        /// <summary>
        /// Gets the printed reference for the given object
        /// </summary>
        /// <param name="reference">the referenced object</param>
        /// <param name="context">the parse context</param>
        /// <returns>a string representation</returns>
        protected abstract string GetReferenceString(TReference reference, ParseContext context);

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            if (semanticElement is ParseObject parseObject && parseObject.TryPeekModelToken<TSemanticElement, TReference>(Feature, GetCollection, null, out var assigned))
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
                return base.Synthesize(GetReferenceString(assigned, context), position, context);
            }
            return new FailedRuleApplication(this, position, default, position, Feature);
        }

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
                    if (Rule is AddAssignRule<TSemanticElement, TReference> addAssignRule && ContextElement is TSemanticElement contextElement)
                    {
                        var collection = addAssignRule.GetCollection(contextElement, context);
                        if (oldValue.GetValue(context) is TReference oldProperty)
                        {
                            collection.Remove(oldProperty);
                        }
                        if (newValue.GetValue(context) is TReference newProperty)
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


        private sealed class ResolveAction : ParseResolveAction
        {
            private readonly bool _isAdd;

            public ResolveAction(RuleApplication ruleApplication, string resolveString, ParsePosition position, bool isAdd) : base(ruleApplication, resolveString, position)
            {
                _isAdd = isAdd;
            }

            public override void OnParsingComplete(ParseContext parseContext)
            {
                var contextElement = RuleApplication.ContextElement;
                if (parseContext.TryResolveReference<TReference>(contextElement, ResolveString, out var reference))
                {
                    var parent = (AddAssignReferenceRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                    var collection = parent.GetCollection((TSemanticElement)contextElement, parseContext);
                    if (_isAdd)
                    {
                        collection.Add(reference);
                    }
                    else
                    {
                        collection.Remove(reference);
                    }
                }
                else
                {
                    parseContext.Errors.Add(new ParseError(ParseErrorSources.ResolveReferences, Position, RuleApplication.Length, $"Could not resolve '{ResolveString}' as {typeof(TReference).Name}"));
                }
            }
        }
    }
}
