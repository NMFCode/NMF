using NMF.AnyText.Rules;
using System.Collections.Generic;

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
        /// <param name="continuations">the continuations possible in this context</param>
        public RecursionContext(ParsePosition position, IReadOnlyCollection<RecursiveContinuation> continuations)
        {
            Position = position;
            Continuations = continuations;
        }

        /// <summary>
        /// Gets the list of possible continuations for the given recursion
        /// </summary>
        public IReadOnlyCollection<RecursiveContinuation> Continuations { get; }

        /// <summary>
        /// The position of this recursion
        /// </summary>
        public ParsePosition Position { get; }
    }
}
