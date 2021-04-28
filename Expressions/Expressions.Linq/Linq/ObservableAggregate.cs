using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public abstract class ObservableAggregate<TSource, TAccumulator, TResult> : NotifyValue<TResult>, IValueChangedNotificationResult<TResult>
    {
        private readonly INotifyEnumerable<TSource> source;
        private TResult oldValue;

        protected TAccumulator Accumulator { get; set; }

        protected ObservableAggregate(INotifyEnumerable<TSource> source, TAccumulator accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
            Accumulator = accumulator;
        }

        protected INotifyEnumerable<TSource> Source
        {
            get { return source; }
        }

        protected abstract void ResetAccumulator();

        protected abstract void RemoveItem(TSource item);

        protected abstract void AddItem(TSource item);

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

        protected override void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var sourceChange = (ICollectionChangedNotificationResult<TSource>)sources[0];
            oldValue = Value;
            if (sourceChange.IsReset)
            {
                ResetAccumulator();
                foreach (var item in source)
                {
                    AddItem(item);
                }
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
