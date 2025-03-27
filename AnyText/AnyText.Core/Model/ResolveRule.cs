using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes the base class for a rule that resolves elements
    /// </summary>
    public abstract class ResolveRule<TSemanticElement, TReference> : QuoteRule
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
                    Apply(context, contextElement, propertyValue);
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
            if (application.ContextElement is TSemanticElement contextElement && application.SemanticElement is TReference propertyValue)
            {
                context.RemoveReference(application.SemanticElement, application);
                Unapply(context, contextElement, propertyValue);
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                Invalidate(application, context, contextElement);
                return true;
            }
            return false;
        }

        private void Invalidate(RuleApplication application, ParseContext context, TSemanticElement contextElement)
        {
            if (application.SemanticElement != null)
            {
                context.RemoveReference(application.SemanticElement, application);
            }
            var resolveString = RuleHelper.Stringify(application.GetValue(context));
            if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
            {
                Apply(context, contextElement, propertyValue);
                ((ResolveRuleApplication)application).Resolved = propertyValue;
                context.AddReference(propertyValue, application);
            }
            else
            {
                context.EnqueueResolveAction(new ResolveAction(application, resolveString));
            }
        }

        /// <inheritdoc />
        public override void Invalidate(RuleApplication ruleApplication, ParseContext context)
        {
            if (ruleApplication.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(ruleApplication.GetValue(context));
                if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    if (!object.Equals(propertyValue, ruleApplication.SemanticElement))
                    {
                        context.RemoveReference(ruleApplication.SemanticElement, ruleApplication);
                        Apply(context, contextElement, propertyValue);
                    }
                    ((ResolveRuleApplication)ruleApplication).Resolved = propertyValue;
                    context.AddReference(propertyValue, ruleApplication);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(ruleApplication, resolveString));
                }
            }
        }

        /// <summary>
        /// Unapplies the given value to the provided context element
        /// </summary>
        /// <param name="context">the parse context in which this operation is performed</param>
        /// <param name="contextElement">the element to which the value should be unapplied</param>
        /// <param name="propertyValue">the value to unapply</param>
        protected abstract void Unapply(ParseContext context, TSemanticElement contextElement, TReference propertyValue);

        /// <summary>
        /// Applies the given value to the provided context element
        /// </summary>
        /// <param name="context">the parse context in which this operation is performed</param>
        /// <param name="contextElement">the element to which the value should be applied</param>
        /// <param name="propertyValue">the value to apply</param>
        protected abstract void Apply(ParseContext context, TSemanticElement contextElement, TReference propertyValue);

        /// <summary>
        /// Resolves the given input
        /// </summary>
        /// <param name="contextElement">the element in the context of which the string is resolved</param>
        /// <param name="input">the textual reference</param>
        /// <param name="resolved">the resolved reference or the default</param>
        /// <param name="context">the context in which the element is resolved</param>
        /// <returns>true, if the reference could be resolved, otherwise false</returns>
        protected virtual bool TryResolveReference(TSemanticElement contextElement, string input, ParseContext context, out TReference resolved)
        {
            return context.TryResolveReference(contextElement, input, out resolved);
        }

        /// <summary>
        /// Gets the candidates to resolve the given input straing
        /// </summary>
        /// <param name="contextElement">the context element in which the element should be resolved</param>
        /// <param name="input">the input string</param>
        /// <param name="context">the parse context in which candidates are resolved</param>
        /// <returns>A collection of potential references</returns>
        /// <remarks>In case the parse tree was not successful, the context element may be of a different type than <typeparamref name="TSemanticElement"/>.</remarks>
        protected virtual IEnumerable<TReference> GetCandidates(object contextElement, string input, ParseContext context)
        {
            return context.GetPotentialReferences<TReference>(contextElement, input);
        }

        /// <summary>
        /// Gets the printed reference for the given object
        /// </summary>
        /// <param name="reference">the referenced object</param>
        /// <param name="context">the parse context</param>
        /// <param name="contextElement">the semantic context element</param>
        /// <returns>a string representation</returns>
        protected virtual string GetReferenceString(TReference reference, object contextElement, ParseContext context) => reference.ToString();

        /// <inheritdoc />
        public override IEnumerable<CompletionEntry> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, ParsePosition position)
        {
            var restoredContext = context.RestoreContextElement(ruleApplication);
            if (ruleApplication.CurrentPosition.Line != position.Line)
            {
                return null;
            }
            var resolveString = string.Empty;
            if (ruleApplication.CurrentPosition.Col < position.Col && position.Line < context.Input.Length)
            {
                resolveString = context.Input[position.Line].Substring(ruleApplication.CurrentPosition.Col, position.Col - ruleApplication.CurrentPosition.Col);
            }
            return GetCandidates(restoredContext, resolveString, context).Select(r => new CompletionEntry(GetReferenceString(r, restoredContext, context), context.Grammar.GetSymbolKindForType(r.GetType()), position)); 
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(RuleApplication app, ParseContext context)
        {
            return new ResolveRuleApplication(this, app, app.Length, app.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool IsReference => true;

        internal class ResolveRuleApplication : SingleRuleApplication
        {
            public ResolveRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            public TReference Resolved { get; set; }

            public override object SemanticElement => Resolved;

            protected override void OnMigrate(RuleApplication oldValue, RuleApplication newValue, ParseContext context)
            {
                if (oldValue.IsActive)
                {
                    oldValue.Deactivate(context);
                    newValue.Activate(context);
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

        /// <summary>
        /// Gets the delay level
        /// </summary>
        /// <remarks>Reference are resolved in layers, one after the other. </remarks>
        protected virtual byte ResolveDelayLevel => 0;

        /// <summary>
        /// Determines, whether the rule should attempt to resolve references directly when a rule application gets activated
        /// </summary>
        protected virtual bool TryResolveOnActivate => false;

        private sealed class ResolveAction : ParseResolveAction
        {
            public ResolveAction(RuleApplication ruleApplication, string resolveString) : base(ruleApplication, resolveString)
            {
            }

            public override byte ResolveDelayLevel => ((ResolveRule<TSemanticElement, TReference>)(RuleApplication.Rule)).ResolveDelayLevel;

            public override void OnParsingComplete(ParseContext parseContext)
            {
                var contextElement = RuleApplication.ContextElement;
                var parent = (ResolveRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                if (parent.TryResolveReference((TSemanticElement)contextElement, ResolveString, parseContext, out var reference))
                {
                    parent.Apply(parseContext, (TSemanticElement)contextElement, reference);
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
                var parent = (ResolveRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                if (parent.TryResolveReference((TSemanticElement)contextElement, resolveString, context, out var reference))
                {
                    parent.Apply(context, (TSemanticElement)contextElement, reference);
                    context.AddReference(reference, RuleApplication);
                    return false;
                }
                Message = $"Could not resolve '{resolveString}' as {typeof(TReference).Name}";
                return true;
            }
        }
    }
}
