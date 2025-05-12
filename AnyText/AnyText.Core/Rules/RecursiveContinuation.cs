using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a recursive continuation
    /// </summary>
    public abstract class RecursiveContinuation
    {

        protected RecursiveContinuation(IEnumerable<Rule> ruleStack)
        {
            RuleStack = ruleStack.ToList();
        }

        public List<Rule> RuleStack { get; }

        /// <summary>
        /// Grows the given base rule application
        /// </summary>
        /// <param name="baseApplication">the base rule application</param>
        /// <param name="parseContext">the parse context in which the continuation is done</param>
        /// <param name="recursionContext">the recursion context in which the continuation is executed</param>
        /// <param name="position">the position of the parser</param>
        /// <returns>the grown rule application</returns>
        public virtual RuleApplication ResolveRecursion(RuleApplication baseApplication, ParseContext parseContext, RecursionContext recursionContext, ref ParsePosition position)
        {
            return baseApplication;
        }
    }
}
