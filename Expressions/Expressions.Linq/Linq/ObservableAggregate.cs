using System;
using System.Collections.Generic;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes an abstract class for an aggregator incrementalization
    /// </summary>
    /// <typeparam name="TSource">The source type of elements</typeparam>
    /// <typeparam name="TAccumulator">The accumulator used</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public abstract class ObservableAggregate<TSource, TAccumulator, TResult> : NotifyValue<TResult>, IValueChangedNotificationResult<TResult>
    {
        private readonly INotifyEnumerable<TSource> source;
        private TResult oldValue;

        /// <summary>
        /// Gets the current accumulator
        /// </summary>
        protected TAccumulator Accumulator { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="source">The incrementalized source collection</param>
        /// <param name="accumulator">The initial value for the accumulator</param>
        /// <exception cref="ArgumentNullException">Thrown if source is null</exception>
        protected ObservableAggregate(INotifyEnumerable<TSource> source, TAccumulator accumulator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            this.source = source;
            Accumulator = accumulator;
        }

        /// <summary>
        /// Gets the source collection
        /// </summary>
        protected INotifyEnumerable<TSource> Source
        {
            get { return source; }
        }

        /// <summary>
        /// Resets the accumulator
        /// </summary>
        protected abstract void ResetAccumulator();

        /// <summary>
        /// Removes the given item
        /// </summary>
        /// <param name="item">the item</param>
        protected abstract void RemoveItem(TSource item);

        /// <summary>
        /// Adds the given item
        /// </summary>
        /// <param name="item">the item</param>
        protected abstract void AddItem(TSource item);

        /// <inheritdoc />
        protected override void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            ResetAccumulator();
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        /// <inheritdoc />
        protected override void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        /// <inheritdoc />
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var sourceChange = (ICollectionChangedNotificationResult<TSource>)sources[0];
            oldValue = Value;
            if (sourceChange.IsReset)
            {
                NotifyReset();
            }
            else
            {
                if (sourceChange.RemovedItems != null)
                {
                    foreach (var item in sourceChange.RemovedItems)
                    {
                        RemoveItem(item);
                    }
                }
                if (sourceChange.AddedItems != null)
                {
                    foreach (var item in sourceChange.AddedItems)
                    {
                        AddItem(item);
                    }
                }
            }

            var newValue = Value;
            if (!EqualityComparer<TResult>.Default.Equals(newValue, oldValue))
            {
                OnValueChanged(oldValue, newValue);
                return this;
            }
            else
                return UnchangedNotificationResult.Instance;
        }

        private void NotifyReset()
        {
            ResetAccumulator();
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        #region value change notification
        TResult IValueChangedNotificationResult<TResult>.OldValue { get { return oldValue; } }

        TResult IValueChangedNotificationResult<TResult>.NewValue { get { return Value; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return Value; } }

        INotifiable INotificationResult.Source { get { return this; } }

        bool INotificationResult.Changed { get { return true; } }

        void INotificationResult.IncreaseReferences(int references) { }

        void INotificationResult.FreeReference() { }
        #endregion
    }
}
