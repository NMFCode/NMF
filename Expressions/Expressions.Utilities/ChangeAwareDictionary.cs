using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        private class Entry : INotifyReversableExpression<TValue>, INotifyPropertyChanged, IValueChangedNotificationResult<TValue>
        {
            private readonly PropertyChangeListener listener;
            private TValue value;
            private TValue oldValue;

            public Entry()
            {
                listener = new PropertyChangeListener(this);
                var successors = new MultiSuccessorList();
                successors.Attached += (obj, e) => listener.Subscribe(this, "Value");
                successors.Detached += (obj, e) => listener.Unsubscribe();
                Successors = successors;
            }

            public IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    return Enumerable.Empty<INotifiable>();
                }
            }

            public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

            public bool IsReversable
            {
                get
                {
                    return true;
                }
            }

            public ISuccessorList Successors { get; }

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
                        oldValue = this.value;
                        this.value = value;
                        ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, value));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
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

            public event PropertyChangedEventHandler PropertyChanged;
            public event EventHandler<ValueChangedEventArgs> ValueChanged;

            public void Dispose()
            {
                Successors.UnsetAll();
            }

            public INotificationResult Notify(IList<INotificationResult> sources)
            {
                return this;
            }

            INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
            {
                return this;
            }

            INotifyExpression<TValue> INotifyExpression<TValue>.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
            {
                return this;
            }

            INotifyExpression<TValue> INotifyExpression<TValue>.Reduce()
            {
                return this;
            }

            #region embedded change notification

            TValue IValueChangedNotificationResult<TValue>.OldValue { get { return oldValue; } }

            object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

            TValue IValueChangedNotificationResult<TValue>.NewValue { get { return value; } }

            object IValueChangedNotificationResult.NewValue { get { return value; } }

            INotifiable INotificationResult.Source { get { return this; } }

            bool INotificationResult.Changed { get { return true; } }

            void INotificationResult.IncreaseReferences(int references) { }

            void INotificationResult.FreeReference() { }
            #endregion
        }

        private readonly Dictionary<TKey, Entry> inner;

        /// <summary>
        /// Creates a new change-aware dictionary
        /// </summary>
        public ChangeAwareDictionary() : this((IEqualityComparer<TKey>)null) { }

        /// <summary>
        /// Creates a new change-aware dictionary
        /// </summary>
        /// <param name="comparer">The comparer that should be used to compare dictionary entries</param>
        public ChangeAwareDictionary(IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, Entry>(comparer);
        }

        /// <summary>
        /// Creates a new change-aware dictionary based on a template
        /// </summary>
        /// <param name="template">The template dictionary</param>
        public ChangeAwareDictionary(ChangeAwareDictionary<TKey, TValue> template)
        {
            if (template == null)
            {
                inner = new Dictionary<TKey, Entry>();
            }
            else
            {
                inner = new Dictionary<TKey, Entry>(template.inner, template.inner.Comparer);
            }
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
