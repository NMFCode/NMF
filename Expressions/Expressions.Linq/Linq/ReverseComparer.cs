using System.Collections.Generic;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a comparer that reverses another comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> baseComparer;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="baseComparer">The inner comparer or null</param>
        public ReverseComparer(IComparer<T> baseComparer)
        {
            this.baseComparer = baseComparer ?? Comparer<T>.Default;
        }

        /// <inheritdoc />
        public int Compare(T x, T y)
        {
#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            return baseComparer.Compare(y, x);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
        }
    }
}
