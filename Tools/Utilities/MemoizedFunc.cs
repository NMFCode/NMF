using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Utilities
{
    public class MemoizedFunc<T, TResult>
    {
        private readonly Dictionary<T, TResult> savedResults = new Dictionary<T, TResult>();
        private TResult nullResult;
        private bool nullResultSet;
        private readonly Func<T, TResult> func;

        public MemoizedFunc(Func<T, TResult> func)
        {
            this.func = func;
        }

        public TResult this[T arg]
        {
            get
            {
                if (arg != null)
                {
                    TResult result;
                    if (!savedResults.TryGetValue(arg, out result))
                    {
                        result = func(arg);
                        savedResults.Add(arg, result);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    if (!nullResultSet)
                    {
                        nullResult = func(arg);
                        nullResultSet = true;
                    }
                    return nullResult;
                }
            }
        }

        public void Forget(T arg)
        {
            if (arg != null)
            {
                savedResults.Remove(arg);
            }
            else
            {
                nullResultSet = false;
                nullResult = default(TResult);
            }
        }

        public bool IsMemoized(T arg)
        {
            if (arg != null)
            {
                return savedResults.ContainsKey(arg);
            }
            else
            {
                return nullResultSet;
            }
        }
    }

    public class MemoizedFunc<T1, T2, TResult>
    {
        private struct Tuple : IEquatable<Tuple>
        {
            public T1 Item1 { get; set; }
            public T2 Item2 { get; set; }

            public Tuple(T1 item1, T2 item2) : this()
            {
                Item1 = item1;
                Item2 = item2;
            }

            public override bool Equals(object obj)
            {
                if (obj is Tuple)
                {
                    return Equals((Tuple)obj);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (Item1 != null) hash ^= Item1.GetHashCode();
                if (Item2 != null) hash ^= Item2.GetHashCode();
                return hash;
            }

            public bool Equals(Tuple other)
            {
                return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
            }
        }

        private readonly Dictionary<Tuple, TResult> savedResults = new Dictionary<Tuple, TResult>();
        private readonly Func<T1, T2, TResult> func;

        public MemoizedFunc(Func<T1, T2, TResult> func)
        {
            this.func = func;
        }

        public TResult this[T1 arg1, T2 arg2]
        {
            get
            {
                var tuple = new Tuple(arg1, arg2);
                TResult result;
                if (savedResults.TryGetValue(tuple, out result))
                {
                    return result;
                }
                else
                {
                    result = func(arg1, arg2);
                    savedResults.Add(tuple, result);
                    return result;
                }
            }
        }

        public void Forget(T1 arg1, T2 arg2)
        {
            var tuple = new Tuple(arg1, arg2);
            savedResults.Remove(tuple);
        }
    }
}
