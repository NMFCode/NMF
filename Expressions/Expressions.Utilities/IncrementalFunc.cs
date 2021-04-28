using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;

            public Key(T1 arg1)
            {
                this.arg1 = arg1;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, TResult>> func) : this(ObservingFunc<T1, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;

            public Key(T1 arg1, T2 arg2)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key key)
                {
                    return Equals(key);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, TResult>> func) : this(ObservingFunc<T1, T2, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;

            public Key(T1 arg1, T2 arg2, T3 arg3)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, TResult>> func) : this(ObservingFunc<T1, T2, T3, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="T6">The type of argument 6</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, T6, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;
            private readonly T6 arg6;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
                this.arg6 = arg6;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5) && EqualityComparer<T6>.Default.Equals(arg6, other.arg6);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                if (arg6 != null) hash ^= arg6.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, T6, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, T6, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <param name="arg6">Input argument 6</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <param name="arg6">Argument 6</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="T6">The type of argument 6</typeparam>
    /// <typeparam name="T7">The type of argument 7</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;
            private readonly T6 arg6;
            private readonly T7 arg7;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
                this.arg6 = arg6;
                this.arg7 = arg7;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5) && EqualityComparer<T6>.Default.Equals(arg6, other.arg6) && EqualityComparer<T7>.Default.Equals(arg7, other.arg7);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                if (arg6 != null) hash ^= arg6.GetHashCode();
                if (arg7 != null) hash ^= arg7.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <param name="arg6">Input argument 6</param>
        /// <param name="arg7">Input argument 7</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <param name="arg6">Argument 6</param>
        /// <param name="arg7">Argument 7</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="T6">The type of argument 6</typeparam>
    /// <typeparam name="T7">The type of argument 7</typeparam>
    /// <typeparam name="T8">The type of argument 8</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;
            private readonly T6 arg6;
            private readonly T7 arg7;
            private readonly T8 arg8;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
                this.arg6 = arg6;
                this.arg7 = arg7;
                this.arg8 = arg8;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5) && EqualityComparer<T6>.Default.Equals(arg6, other.arg6) && EqualityComparer<T7>.Default.Equals(arg7, other.arg7) && EqualityComparer<T8>.Default.Equals(arg8, other.arg8);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                if (arg6 != null) hash ^= arg6.GetHashCode();
                if (arg7 != null) hash ^= arg7.GetHashCode();
                if (arg8 != null) hash ^= arg8.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <param name="arg6">Input argument 6</param>
        /// <param name="arg7">Input argument 7</param>
        /// <param name="arg8">Input argument 8</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <param name="arg6">Argument 6</param>
        /// <param name="arg7">Argument 7</param>
        /// <param name="arg8">Argument 8</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="T6">The type of argument 6</typeparam>
    /// <typeparam name="T7">The type of argument 7</typeparam>
    /// <typeparam name="T8">The type of argument 8</typeparam>
    /// <typeparam name="T9">The type of argument 9</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;
            private readonly T6 arg6;
            private readonly T7 arg7;
            private readonly T8 arg8;
            private readonly T9 arg9;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
                this.arg6 = arg6;
                this.arg7 = arg7;
                this.arg8 = arg8;
                this.arg9 = arg9;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5) && EqualityComparer<T6>.Default.Equals(arg6, other.arg6) && EqualityComparer<T7>.Default.Equals(arg7, other.arg7) && EqualityComparer<T8>.Default.Equals(arg8, other.arg8) && EqualityComparer<T9>.Default.Equals(arg9, other.arg9);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                if (arg6 != null) hash ^= arg6.GetHashCode();
                if (arg7 != null) hash ^= arg7.GetHashCode();
                if (arg8 != null) hash ^= arg8.GetHashCode();
                if (arg9 != null) hash ^= arg9.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <param name="arg6">Input argument 6</param>
        /// <param name="arg7">Input argument 7</param>
        /// <param name="arg8">Input argument 8</param>
        /// <param name="arg9">Input argument 9</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <param name="arg6">Argument 6</param>
        /// <param name="arg7">Argument 7</param>
        /// <param name="arg8">Argument 8</param>
        /// <param name="arg9">Argument 9</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
    /// <summary>
    /// Represents a function class which tracks any calls
    /// </summary>
    /// <typeparam name="T1">The type of argument 1</typeparam>
    /// <typeparam name="T2">The type of argument 2</typeparam>
    /// <typeparam name="T3">The type of argument 3</typeparam>
    /// <typeparam name="T4">The type of argument 4</typeparam>
    /// <typeparam name="T5">The type of argument 5</typeparam>
    /// <typeparam name="T6">The type of argument 6</typeparam>
    /// <typeparam name="T7">The type of argument 7</typeparam>
    /// <typeparam name="T8">The type of argument 8</typeparam>
    /// <typeparam name="T9">The type of argument 9</typeparam>
    /// <typeparam name="T10">The type of argument 10</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class IncrementalFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        /// <summary>
        /// This structure serves as a helper in order to save multiple items at once, since there are no real tuple types in .NET 4
        /// </summary>
        private struct Key : IEquatable<Key>
        {
            private readonly T1 arg1;
            private readonly T2 arg2;
            private readonly T3 arg3;
            private readonly T4 arg4;
            private readonly T5 arg5;
            private readonly T6 arg6;
            private readonly T7 arg7;
            private readonly T8 arg8;
            private readonly T9 arg9;
            private readonly T10 arg10;

            public Key(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.arg5 = arg5;
                this.arg6 = arg6;
                this.arg7 = arg7;
                this.arg8 = arg8;
                this.arg9 = arg9;
                this.arg10 = arg10;
            }

            public bool Equals(Key other)
            {
                return EqualityComparer<T1>.Default.Equals(arg1, other.arg1) && EqualityComparer<T2>.Default.Equals(arg2, other.arg2) && EqualityComparer<T3>.Default.Equals(arg3, other.arg3) && EqualityComparer<T4>.Default.Equals(arg4, other.arg4) && EqualityComparer<T5>.Default.Equals(arg5, other.arg5) && EqualityComparer<T6>.Default.Equals(arg6, other.arg6) && EqualityComparer<T7>.Default.Equals(arg7, other.arg7) && EqualityComparer<T8>.Default.Equals(arg8, other.arg8) && EqualityComparer<T9>.Default.Equals(arg9, other.arg9) && EqualityComparer<T10>.Default.Equals(arg10, other.arg10);
            }

            public override bool Equals(object obj)
            {
                if (obj is Key)
                {
                    return Equals((Key)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                var hash = 0;
                if (arg1 != null) hash ^= arg1.GetHashCode();
                if (arg2 != null) hash ^= arg2.GetHashCode();
                if (arg3 != null) hash ^= arg3.GetHashCode();
                if (arg4 != null) hash ^= arg4.GetHashCode();
                if (arg5 != null) hash ^= arg5.GetHashCode();
                if (arg6 != null) hash ^= arg6.GetHashCode();
                if (arg7 != null) hash ^= arg7.GetHashCode();
                if (arg8 != null) hash ^= arg8.GetHashCode();
                if (arg9 != null) hash ^= arg9.GetHashCode();
                if (arg10 != null) hash ^= arg10.GetHashCode();
                return hash;
            }
        }

        private readonly ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func;
        private readonly Dictionary<Key, INotifyValue<TResult>> savedArgs = new Dictionary<Key, INotifyValue<TResult>>();

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> func) : this(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.FromExpression(func)) { }

        /// <summary>
        /// Create an incremental func for the given expression
        /// </summary>
        /// <param name="func">The expression that should be observed</param>
        public IncrementalFunc(ObservingFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Gets the function value for the given arguments
        /// </summary>
        /// <param name="arg1">Input argument 1</param>
        /// <param name="arg2">Input argument 2</param>
        /// <param name="arg3">Input argument 3</param>
        /// <param name="arg4">Input argument 4</param>
        /// <param name="arg5">Input argument 5</param>
        /// <param name="arg6">Input argument 6</param>
        /// <param name="arg7">Input argument 7</param>
        /// <param name="arg8">Input argument 8</param>
        /// <param name="arg9">Input argument 9</param>
        /// <param name="arg10">Input argument 10</param>
        /// <returns>The current function valur for the provided argument</returns>
        public TResult this[T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10]
        {
            [ObservableProxy(typeof(IncrementalFunc<,>), "GetNotifyValue")]
            get
            {
                INotifyValue<TResult> saved;
                Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                if (!savedArgs.TryGetValue(key, out saved))
                {
                    saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                    savedArgs.Add(key, saved);
                }
                return saved.Value;
            }
        }

        /// <summary>
        /// Gets the changable value for the given arguments
        /// </summary>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <param name="arg3">Argument 3</param>
        /// <param name="arg4">Argument 4</param>
        /// <param name="arg5">Argument 5</param>
        /// <param name="arg6">Argument 6</param>
        /// <param name="arg7">Argument 7</param>
        /// <param name="arg8">Argument 8</param>
        /// <param name="arg9">Argument 9</param>
        /// <param name="arg10">Argument 10</param>
        /// <returns>A changable function value</returns>
        public INotifyValue<TResult> GetNotifyValue(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            INotifyValue<TResult> saved;
            Key key = new Key(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            if (!savedArgs.TryGetValue(key, out saved))
            {
                saved = func.Observe(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                savedArgs.Add(key, saved);
            }
			return saved;
        }
    }
}
