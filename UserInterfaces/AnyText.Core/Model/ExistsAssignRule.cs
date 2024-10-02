using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Assign(semanticElement, application.IsPositive, context);
            }
        }

        /// <inheritdoc />
        protected internal override void OnDeactivate(RuleApplication application, ParseContext context)
        {
            if (application.ContextElement is TSemanticElement semanticElement)
            {
                Assign(semanticElement, false, context);
            }
        }

        /// <summary>
        /// Assigns the value to the given semantic element
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="value">the value to assign</param>
        /// <param name="context">the parse context</param>
        protected abstract void Assign(TSemanticElement semanticElement, bool value, ParseContext context);
    }
}
