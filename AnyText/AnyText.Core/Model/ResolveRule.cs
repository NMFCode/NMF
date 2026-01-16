using NMF.AnyText.Rules;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes the base class for a rule that resolves elements
    /// </summary>
    public abstract class ResolveRule<TSemanticElement, TReference> : QuoteRule
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context, bool initial)
        {
            if (!context.IsExecutingModelChanges)
            {
                if (application is ResolveRuleApplication ruleApplication)
                {
                    context.AddReference(ruleApplication.Resolved, ruleApplication);
                }
                return;
            }
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveOnActivate && TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    var ruleApplication = (ResolveRuleApplication)application;
                    ruleApplication.Resolved = propertyValue;
                    Apply(ruleApplication, context, contextElement, propertyValue, initial);
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
                if (context.IsExecutingModelChanges)
                {
                    Unapply(application, context, contextElement, propertyValue);
                }
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context, RuleApplication oldRuleApplication)
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
            var resolveString = RuleHelper.Stringify(application.GetValue(context));
            var resolveApplication = (ResolveRuleApplication)application;
            if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
            {
                if (!EqualityComparer<TReference>.Default.Equals(resolveApplication.Resolved, propertyValue))
                {
                    if (resolveApplication.SemanticElement != null)
                    {
                        context.RemoveReference(resolveApplication.Resolved, application);
                    }
                    if (context.IsExecutingModelChanges)
                    {
                        if (ApplyOverReplace)
                        {
                            Apply(application, context, contextElement, propertyValue, false);
                        }
                        else
                        {
                            Replace(application, context, contextElement, resolveApplication.Resolved, propertyValue);
                        }
                    }
                    resolveApplication.Resolved = propertyValue;
                    context.AddReference(propertyValue, application);
                }
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
                if (!InvalidateCore(ruleApplication, context, contextElement, resolveString))
                {
                    context.EnqueueResolveAction(new ResolveAction(ruleApplication, resolveString));
                }
            }
        }

        private bool InvalidateCore(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, string resolveString)
        {
            if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
            {
                if (!object.Equals(propertyValue, ruleApplication.SemanticElement))
                {
                    context.RemoveReference(ruleApplication.SemanticElement, ruleApplication);
                    if (context.IsExecutingModelChanges)
                    {
                        if (ApplyOverReplace)
                        {
                            Apply(ruleApplication, context, contextElement, propertyValue, false);
                        }
                        else
                        {
                            Replace(ruleApplication, context, contextElement, (TReference)ruleApplication.SemanticElement, propertyValue);
                        }
                    }
                }
                ((ResolveRuleApplication)ruleApplication).Resolved = propertyValue;
                context.AddReference(propertyValue, ruleApplication);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Unapplies the given value to the provided context element
        /// </summary>
        /// <param name="ruleApplication">the rule application which should be applied</param>
        /// <param name="context">the parse context in which this operation is performed</param>
        /// <param name="contextElement">the element to which the value should be unapplied</param>
        /// <param name="propertyValue">the value to unapply</param>
        protected abstract void Unapply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue);

        /// <summary>
        /// Applies the given value to the provided context element
        /// </summary>
        /// <param name="ruleApplication">the rule application which should be applied</param>
        /// <param name="context">the parse context in which this operation is performed</param>
        /// <param name="contextElement">the element to which the value should be applied</param>
        /// <param name="propertyValue">the value to apply</param>
        /// <param name="initial">A flag indicating whether the apply was called due to initial parse</param>
        protected abstract void Apply(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference propertyValue, bool initial);

        /// <summary>
        /// Replaces the provided old element with the provided new element
        /// </summary>
        /// <param name="ruleApplication">the rule application which should be applied</param>
        /// <param name="context">the parse context in which this operation is performed</param>
        /// <param name="contextElement">the element to which the value should be applied</param>
        /// <param name="oldValue">the old value</param>
        /// <param name="newValue">the value to apply</param>
        protected virtual void Replace(RuleApplication ruleApplication, ParseContext context, TSemanticElement contextElement, TReference oldValue, TReference newValue)
        {
            if (context.IsExecutingModelChanges)
            {
                Unapply(ruleApplication, context, contextElement, oldValue);
                Apply(ruleApplication, context, contextElement, newValue, false);
            }
        }

        /// <summary>
        /// Indicates whether a replace rather than apply is required on update
        /// </summary>
        protected virtual bool ApplyOverReplace => false;

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
        public override string GetHoverText(RuleApplication ruleApplication, Parser document, ParsePosition position)
        {
            if (ruleApplication is ResolveRuleApplication resolveRuleApplication)
            {
                if (resolveRuleApplication.Resolved == null)
                {
                    return "(failed to resolve)";
                }
                return resolveRuleApplication.Resolved.ToString();
            }
            return base.GetHoverText(ruleApplication, document, position);
        }

        /// <inheritdoc />
        public override IEnumerable<CompletionEntry> SuggestCompletions(ParseContext context, RuleApplication ruleApplication, string fragment, ParsePosition position)
        {
            var restoredContext = context.RestoreContextElement(ruleApplication);
            if (ruleApplication.CurrentPosition.Line != position.Line)
            {
                return null;
            }
            return GetCandidates(restoredContext, fragment, context).Select(r => CreateCompletionEntry(r, restoredContext, ruleApplication, position, context)); 
        }

        /// <summary>
        /// Creates the completion entry for the given reference
        /// </summary>
        /// <param name="reference">the referenced element</param>
        /// <param name="semanticContext">the semantic context</param>
        /// <param name="ruleApplication">the rule application for which to calculate the completion entry</param>
        /// <param name="position">the position of the request</param>
        /// <param name="context">the parse context in which the completion is requested</param>
        /// <returns></returns>
        protected virtual CompletionEntry CreateCompletionEntry(TReference reference, object semanticContext, RuleApplication ruleApplication, ParsePosition position, ParseContext context)
        {
            return new CompletionEntry(GetReferenceString(reference, semanticContext, context), context.Grammar.GetSymbolKindForType(reference.GetType()), ruleApplication.CurrentPosition, position);
        }

        /// <inheritdoc />
        protected override RuleApplication CreateRuleApplication(RuleApplication app, ParseContext context, object semanticElement = null)
        {
            return new ResolveRuleApplication(this, app, app.Length, app.ExaminedTo, semanticElement);
        }

        /// <inheritdoc />
        public override bool IsReference => true;

        internal class ResolveRuleApplication : SingleRuleApplication
        {
            public ResolveRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo, object semanticElement) : base(rule, inner, endsAt, examinedTo)
            {
                if (semanticElement is TReference resolved)
                {
                    Resolved = resolved;
                }
            }

            public TReference Resolved { get; set; }

            public override object SemanticElement => Resolved;
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
                if (!parent.InvalidateCore(RuleApplication, parseContext, (TSemanticElement)contextElement, ResolveString))
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
                if (parent.InvalidateCore(RuleApplication, context, (TSemanticElement)contextElement, resolveString))
                {
                    return false;
                }
                Message = $"Could not resolve '{resolveString}' as {typeof(TReference).Name}";
                return true;
            }
        }
    }
}
