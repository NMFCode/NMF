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
    public abstract class ResolveRule<T> : QuoteRule
    {

        /// <summary>
        /// Resolves the given input
        /// </summary>
        /// <param name="contextElement">the element in the context of which the string is resolved</param>
        /// <param name="input">the textual reference</param>
        /// <param name="resolved">the resolved reference or the default</param>
        /// <param name="context">the context in which the element is resolved</param>
        /// <returns>true, if the reference could be resolved, otherwise false</returns>
        protected virtual bool TryResolveReference(object contextElement, string input, ParseContext context, out T resolved)
        {
            return context.TryResolveReference(contextElement, input, out resolved);
        }

        protected virtual byte ResolveDelayLevel => 0;

        protected virtual bool TryResolveOnActivate => false;
    }
}
