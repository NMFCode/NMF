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
    public abstract class AssignReferenceRule<TSemanticElement, TReference> : ResolveRule<TReference>
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParsePosition position, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement)
            {
                var resolveString = RuleHelper.Stringify(application.GetValue(context));
                if (TryResolveReference(contextElement, resolveString, context, out var propertyValue))
                {
                    OnChangeValue(contextElement, propertyValue, context);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString, default));
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
                OnChangeValue(contextElement, default, context);
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
                    OnChangeValue(contextElement, propertyValue, context);
                }
                else
                {
                    context.EnqueueResolveAction(new ResolveAction(application, resolveString, default));
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
        protected abstract void OnChangeValue(TSemanticElement semanticElement, TReference propertyValue, ParseContext context);

        private sealed class ResolveAction : ParseResolveAction
        {
            public ResolveAction(RuleApplication ruleApplication, string resolveString, ParsePosition position) : base(ruleApplication, resolveString, position)
            {
            }

            public override void OnParsingComplete(ParseContext parseContext)
            {
                var contextElement = RuleApplication.ContextElement;
                if (parseContext.TryResolveReference<TReference>(contextElement, ResolveString, out var reference))
                {
                    var parent = (AssignReferenceRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                    parent.OnChangeValue((TSemanticElement)contextElement, reference, parseContext);
                }
                else
                {
                    parseContext.Errors.Add(new ParseError(ParseErrorSources.ResolveReferences, Position, RuleApplication.Length, $"Could not resolve '{ResolveString}' as {typeof(TReference).Name}"));
                }
            }
        }
    }
}
