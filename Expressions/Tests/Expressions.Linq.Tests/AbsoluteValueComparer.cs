using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq.Tests
{
    public class AbsoluteValueComparer : IEqualityComparer<int>, IComparer<int>
    {
        public bool Equals(int x, int y)
        {
            return Math.Abs(x) == Math.Abs(y);
        }

        public int GetHashCode(int obj)
        {
            return Math.Abs(obj).GetHashCode();
        }

        public int Compare(int x, int y)
        {
            return Math.Abs(x).CompareTo(Math.Abs(y));
        }
    }
}
