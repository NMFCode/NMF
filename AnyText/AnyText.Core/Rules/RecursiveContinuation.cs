using System.Collections.Generic;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a recursive continuation
    /// </summary>
    public abstract class RecursiveContinuation
    {

        /// <summary>
        /// Creates a new continuation
        /// </summary>
        /// <param name="affectedRules">the rules that need to be invalidated when performing the recursion</param>
        protected RecursiveContinuation(IReadOnlyList<Rule> affectedRules)
        {
            AffectedRules = affectedRules;
        }

        /// <summary>
        /// Gets the rules needed to invalidate before executing the recursion
        /// </summary>
        public IReadOnlyList<Rule> AffectedRules { get; }

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
