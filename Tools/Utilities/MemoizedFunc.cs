using System;
using System.Collections.Generic;

namespace NMF.Utilities
{
    /// <summary>
    /// Denotes a function whose return values are cached
    /// </summary>
    /// <typeparam name="T">the parameter type</typeparam>
    /// <typeparam name="TResult">the result type</typeparam>
    public class MemoizedFunc<T, TResult>
    {
        private readonly Dictionary<T, TResult> savedResults = new Dictionary<T, TResult>();
        private TResult nullResult;
        private bool nullResultSet;
        private readonly Func<T, TResult> func;

        /// <summary>
        /// Creates a new memoized function for the given function
        /// </summary>
        /// <param name="func">the function to memoize</param>
        public MemoizedFunc(Func<T, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the result for the given argument
        /// </summary>
        /// <param name="arg">the argument</param>
        /// <returns>the function result. If possible, the result is taken from the cache, otherwise it is computed and the cache is updated</returns>
        public TResult this[T arg]
        {
            get
            {
                if (arg != null)
                {
                    if (!savedResults.TryGetValue(arg, out TResult result))
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

        /// <summary>
        /// Forgets the saved result for the given argument
        /// </summary>
        /// <param name="arg">the argument for which the result should be forgotten</param>
        public void Forget(T arg)
        {
            if (arg != null)
            {
                savedResults.Remove(arg);
            }
            else
            {
                nullResultSet = false;
                nullResult = default;
            }
        }

        /// <summary>
        /// Determines whether the current memoized function remembers a result for the given argument
        /// </summary>
        /// <param name="arg">the argument</param>
        /// <returns>true, if a result for this argument is available, otherwise false</returns>
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

    /// <summary>
    /// Denotes a function whose return values are cached
    /// </summary>
    /// <typeparam name="T1">the first parameter type</typeparam>
    /// <typeparam name="T2">the second parameter type</typeparam>
    /// <typeparam name="TResult">the result type</typeparam>
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

            public override readonly bool Equals(object obj)
            {
                if (obj is Tuple tuple)
                {
                    return Equals(tuple);
                }
                else
                {
                    return false;
                }
            }

            public override readonly int GetHashCode()
            {
                var hash = 0;
                if (Item1 != null) hash ^= Item1.GetHashCode();
                if (Item2 != null) hash ^= Item2.GetHashCode();
                return hash;
            }

            public readonly bool Equals(Tuple other)
            {
                return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
            }
        }

        private readonly Dictionary<Tuple, TResult> savedResults = new Dictionary<Tuple, TResult>();
        private readonly Func<T1, T2, TResult> func;

        /// <summary>
        /// Creates a new memoized function for the given function
        /// </summary>
        /// <param name="func">the function to memoize</param>
        public MemoizedFunc(Func<T1, T2, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the result for the given argument
        /// </summary>
        /// <param name="arg1">the first argument</param>
        /// <param name="arg2">the second argument</param>
        /// <returns>the function result. If possible, the result is taken from the cache, otherwise it is computed and the cache is updated</returns>
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

        /// <summary>
        /// Forgets the saved result for the given argument
        /// </summary>
        /// <param name="arg1">the argument for which the result should be forgotten</param>
        /// <param name="arg2">the argument for which the result should be forgotten</param>
        public void Forget(T1 arg1, T2 arg2)
        {
            var tuple = new Tuple(arg1, arg2);
            savedResults.Remove(tuple);
        }
    }
}
