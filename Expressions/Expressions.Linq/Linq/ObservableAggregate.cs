using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public abstract class ObservableAggregate<TSource, TAccumulator, TResult> : NotifyValue<TResult>
    {
        private INotifyEnumerable<TSource> source;

        protected TAccumulator Accumulator { get; set; }

        protected ObservableAggregate(INotifyEnumerable<TSource> source, TAccumulator accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
            Accumulator = accumulator;

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        protected INotifyEnumerable<TSource> Source
        {
            get { return source; }
        }

        protected abstract void ResetAccumulator();

        protected abstract void RemoveItem(TSource item);

        protected abstract void AddItem(TSource item);

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            ResetAccumulator();
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var sourceChange = (CollectionChangedNotificationResult<TSource>)sources[0];
            var oldValue = Value;
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
                return new ValueChangedNotificationResult<TResult>(this, oldValue, newValue);
            }
            else
                return UnchangedNotificationResult.Instance;
        }

        public override IEnumerable<INotifiable> Dependencies { get { yield return source; } }
    }
}
