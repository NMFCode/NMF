using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Provides a notifiable associative memory
    /// </summary>
    /// <typeparam name="TKey">The type of the keys</typeparam>
    /// <typeparam name="TValue">The type of the values</typeparam>
    public class ChangeAwareDictionary<TKey, TValue>
    {
        private class Entry : INotifyReversableExpression<TValue>
        {
            private TValue value;

            public bool IsReversable
            {
                get
                {
                    return true;
                }
            }

            public TValue Value
            {
                get
                {
                    return value;
                }

                set
                {
                    if (!EqualityComparer<TValue>.Default.Equals(value, this.value))
                    {
                        var change = new ValueChangedEventArgs(this.value, value);
                        this.value = value;
                        ValueChanged?.Invoke(this, change);
                    }
                }
            }

            bool INotifyExpression.CanBeConstant
            {
                get
                {
                    return false;
                }
            }

            bool INotifyValue<TValue>.IsAttached
            {
                get
                {
                    return true;
                }
            }

            bool INotifyExpression.IsConstant
            {
                get
                {
                    return false;
                }
            }

            bool INotifyExpression.IsParameterFree
            {
                get
                {
                    return true;
                }
            }

            TValue INotifyValue<TValue>.Value
            {
                get
                {
                    return Value;
                }
            }

            object INotifyExpression.ValueObject
            {
                get
                {
                    return Value;
                }
            }

            public event EventHandler<ValueChangedEventArgs> ValueChanged;

            INotifyExpression<TValue> INotifyExpression<TValue>.ApplyParameters(IDictionary<string, object> parameters)
            {
                return this;
            }

            void INotifyValue<TValue>.Attach() { }

            void INotifyValue<TValue>.Detach() { }

            INotifyExpression<TValue> INotifyExpression<TValue>.Reduce()
            {
                return this;
            }

            void INotifyExpression.Refresh()
            {
            }
        }

        private Dictionary<TKey, Entry> inner;

        /// <summary>
        /// Creates a new change-aware dictionary
        /// </summary>
        public ChangeAwareDictionary() : this(null) { }

        /// <summary>
        /// Creates a new change-aware dictionary
        /// </summary>
        /// <param name="comparer">The comparer that should be used to compare dictionary entries</param>
        public ChangeAwareDictionary(IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, Entry>(comparer);
        }

        /// <summary>
        /// Gets or sets a value for the given key
        /// </summary>
        /// <param name="key">The key element</param>
        /// <returns>The current value for the given key</returns>
        public TValue this[TKey key]
        {
            [ObservableProxy(typeof(ChangeAwareDictionary<,>), "AsNotifiable")]
            get
            {
                return AsNotifiable(key).Value;
            }
            set
            {
                AsNotifiable(key).Value = value;
            }
        }

        /// <summary>
        /// Gets an object that tracks the current value for the given key
        /// </summary>
        /// <param name="key">The given key</param>
        /// <returns>An object that tracks the current value and notifies clients when this value changes</returns>
        public INotifyReversableValue<TValue> AsNotifiable(TKey key)
        {
            Entry entry;
            if (!inner.TryGetValue(key, out entry))
            {
                entry = new Entry();
                inner.Add(key, entry);
            }

            return entry;
        }

        /// <summary>
        /// Forgets the notifiable value for the given key
        /// </summary>
        /// <param name="key">The key object</param>
        /// <returns>True, if there was an element for the given key, otherwise false</returns>
        public bool Forget(TKey key)
        {
            return inner.Remove(key);
        }
    }
}
