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
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParsePosition position, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                OnChangeValue(contextElement, propertyValue, context);
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
            if (application.ContextElement is TSemanticElement contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                OnChangeValue(contextElement, propertyValue, context);
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
        protected abstract void OnChangeValue(TSemanticElement semanticElement, TProperty propertyValue, ParseContext context);

    }
}
