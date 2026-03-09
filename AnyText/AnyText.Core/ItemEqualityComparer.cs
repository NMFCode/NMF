using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class ItemEqualityComparer<T> : IEqualityComparer<T[]>
    {
        private readonly IEqualityComparer<T> _comparer;

        public ItemEqualityComparer() : this(EqualityComparer<T>.Default) { }

        public ItemEqualityComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer; 
        }

        public bool Equals(T[] x, T[] y)
        {
            if (x == null)
            {
                return y == null;
            }
            if (y == null || y.Length != x.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (!_comparer.Equals(x[i], y[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode([DisallowNull] T[] obj)
        {
            unchecked
            {
                var hash = obj.Length.GetHashCode();
                for (int i = 0; i < obj.Length; i++)
                {
                    hash ^= 23 * hash + obj[i].GetHashCode();
                }
                return hash;
            }
        }
    }
}
