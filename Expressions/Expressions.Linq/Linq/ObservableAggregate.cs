using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public abstract class ObservableAggregate<TSource, TAccumulator, TResult> : INotifyValue<TResult>
    {
        private INotifyEnumerable<TSource> source;
        private int attachedCount;

        protected TAccumulator Accumulator { get; set; }

        protected ObservableAggregate(INotifyEnumerable<TSource> source, TAccumulator accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
            Accumulator = accumulator;
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                var oldValue = Value;
                if (e.OldItems != null)
                {
                    foreach (TSource item in e.OldItems)
                    {
                        RemoveItem(item);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (TSource item in e.NewItems)
                    {
                        AddItem(item);
                    }
                }
                var newValue = Value;
                if (!EqualityComparer<TResult>.Default.Equals(newValue, oldValue))
                {
                    OnValueChanged(new ValueChangedEventArgs(oldValue, newValue));
                }
            }
            else
            {
                var oldValue = Value;
                ResetAccumulator();
                var newValue = Value;
                if (!EqualityComparer<TResult>.Default.Equals(newValue, oldValue))
                {
                    OnValueChanged(new ValueChangedEventArgs(oldValue, newValue));
                }
            }
        }

        protected INotifyEnumerable<TSource> Source
        {
            get { return source; }
        }

        protected abstract void ResetAccumulator();

        protected abstract void RemoveItem(TSource item);

        protected abstract void AddItem(TSource item);

        private void AttachCore()
        {
            ResetAccumulator();
            foreach (var item in source)
            {
                AddItem(item);
            }
            source.CollectionChanged += SourceCollectionChanged;
        }

        public abstract TResult Value { get; }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null) ValueChanged(this, e);
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Detach()
        {
            if (attachedCount == 1)
            {
                DetachCore();
            }
            attachedCount--;
        }

        protected virtual void DetachCore()
        {
            source.CollectionChanged -= SourceCollectionChanged;
        }

        public void Attach()
        {
            if (attachedCount == 0)
            {
                AttachCore();
            }
            attachedCount++;
        }


        public bool IsAttached
        {
            get { return attachedCount > 0; }
        }
    }
}
