using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> baseComparer;

        public ReverseComparer(IComparer<T> baseComparer)
        {
            this.baseComparer = baseComparer ?? Comparer<T>.Default;
        }

        public int Compare(T x, T y)
        {
            return baseComparer.Compare(y, x);
        }
    }
}
