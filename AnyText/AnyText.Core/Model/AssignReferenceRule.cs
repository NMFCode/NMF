﻿using NMF.AnyText.Rules;
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
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveOnActivate && TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    var ruleApplication = (ResolveRuleApplication)application;
                    ruleApplication.Resolved = propertyValue;
                    SetValue(contextElement, propertyValue, context);
                    context.AddReference(propertyValue, application);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString));
                }
            }
            else
            {
                context.AddDiagnosticItem(new DiagnosticItem(DiagnosticSources.Grammar, application, $"Element is not of expected type {typeof(TSemanticElement).Name}"));
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                context.RemoveReference(contextElement, application);
                SetValue(contextElement, default, context);
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    SetValue(contextElement, propertyValue, context);
                    ((ResolveRuleApplication)application).Resolved = propertyValue;
                    context.AddReference(propertyValue, application);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString));
                }
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
        public override bool CanSynthesize(object semanticElement, ParseContext context)
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
            return new FailedRuleApplication(this, position, default, $"'{Feature}' of '{semanticElement}' cannot be synthesized");
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

        private sealed class ResolveAction : ParseResolveAction
        {
            public ResolveAction(RuleApplication ruleApplication, string resolveString) : base(ruleApplication, resolveString)
            {
            }

            public override byte ResolveDelayLevel => ((AssignReferenceRule<TSemanticElement, TReference>)(RuleApplication.Rule)).ResolveDelayLevel;

            public override void OnParsingComplete(ParseContext parseContext)
            {
                var contextElement = RuleApplication.ContextElement;
                var parent = (AssignReferenceRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                if (parent.TryResolveReference((TSemanticElement)contextElement, ResolveString, parseContext, out var reference))
                {
                    parent.SetValue((TSemanticElement)contextElement, reference, parseContext);
                    ((ResolveRuleApplication)RuleApplication).Resolved = reference;
                    parseContext.AddReference(reference, RuleApplication);
                }
                else
                {
                    var existingError = parseContext.Errors.OfType<ResolveError>().FirstOrDefault(e => e.RuleApplication.CurrentPosition == RuleApplication.CurrentPosition && e.RuleApplication.Rule == RuleApplication.Rule);
                    if (existingError != null)
                    {
                        existingError.UpdateMessage($"Could not resolve '{ResolveString}' as {typeof(TReference).Name}");
                    }
                    else
                    {
                        parseContext.AddDiagnosticItem(new ResolveError(DiagnosticSources.ResolveReferences, RuleApplication, $"Could not resolve '{ResolveString}' as {typeof(TReference).Name}"));
                    }
                }
            }
        }

        private sealed class ResolveError : DiagnosticItem
        {
            public void UpdateMessage(string message)
            {
                Message = message;
            }

            public ResolveError(string source, RuleApplication ruleApplication, string message) : base(source, ruleApplication, message)
            {
            }

            public override bool CheckIfStillExist(ParseContext context)
            {
                var contextElement = RuleApplication.ContextElement;
                var resolveString = RuleApplication.GetValue(context) as string;
                var parent = (AssignReferenceRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                if (parent.TryResolveReference((TSemanticElement)contextElement, resolveString, context, out var reference))
                {
                    parent.SetValue((TSemanticElement)contextElement, reference, context);
                    return false;
                }
                Message = $"Could not resolve '{resolveString}' as {typeof(TReference).Name}";
                return true;
            }
        }
    }
}
