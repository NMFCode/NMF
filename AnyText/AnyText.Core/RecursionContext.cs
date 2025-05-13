using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the context in which a left recursion is resolved
    /// </summary>
    public class RecursionContext
    {
        /// <summary>
        /// Creates a new recursion context at the given position
        /// </summary>
        /// <param name="position">the position of the recursion context</param>
        public RecursionContext(ParsePosition position)
        {
            Position = position;
            AddContinuations = true;
            Continuations = new List<RecursiveContinuation>();
            RuleStack = new Stack<Rule>();
        }

        /// <summary>
        /// Gets the stack of rules that participate in the recursion
        /// </summary>
        public Stack<Rule> RuleStack { get; }

        /// <summary>
        /// Gets the list of possible continuations for the given recursion
        /// </summary>
        public List<RecursiveContinuation> Continuations { get; }

        /// <summary>
        /// The position of this recursion
        /// </summary>
        public ParsePosition Position { get; }

        /// <summary>
        /// True, if rules shall register continuations, otherwise false
        /// </summary>
        public bool AddContinuations { get; private set; }

        internal void StopAddingContinuations()
        {
            AddContinuations = false;
        }
    }
}
