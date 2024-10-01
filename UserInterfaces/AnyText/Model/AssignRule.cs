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
    /// <typeparam name="TContext">The type of the context element</typeparam>
    /// <typeparam name="TProperty">The type of the property value</typeparam>
    public abstract class AssignRule<TContext, TProperty> : QuoteRule
    {
        /// <inheritdoc />
        protected internal override void OnActivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TContext contextElement && application.GetValue(context) is TProperty propertyValue)
            {
                OnChangeValue(contextElement, propertyValue, context);
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TContext contextElement)
            {
                OnChangeValue(contextElement, default, context);
            }
        }

        /// <inheritdoc />
        protected internal override bool OnValueChange(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TContext contextElement && application.GetValue(context) is TProperty propertyValue)
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
        protected abstract void OnChangeValue(TContext semanticElement, TProperty propertyValue, ParseContext context);

    }
}
