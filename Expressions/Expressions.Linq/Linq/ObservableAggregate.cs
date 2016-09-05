using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public abstract class ObservableAggregate<TSource, TAccumulator, TResult> : INotifyValue<TResult>
    {
        private ShortList<INotifiable> successors = new ShortList<INotifiable>();
        private INotifyEnumerable<TSource> source;

        protected TAccumulator Accumulator { get; set; }

        protected ObservableAggregate(INotifyEnumerable<TSource> source, TAccumulator accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
            Accumulator = accumulator;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        protected INotifyEnumerable<TSource> Source
        {
            get { return source; }
        }

        protected abstract void ResetAccumulator();

        protected abstract void RemoveItem(TSource item);

        protected abstract void AddItem(TSource item);
        
        public abstract TResult Value { get; }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null) ValueChanged(this, e);
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
            ResetAccumulator();
            foreach (var item in source)
            {
                AddItem(item);
            }
            OnAttach();
        }

        private void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
        }

        protected virtual void OnAttach() { }

        protected virtual void OnDetach() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
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
                foreach (var item in sourceChange.AllRemovedItems)
                {
                    RemoveItem(item);
                }
                foreach (var item in sourceChange.AllAddedItems)
                {
                    AddItem(item);
                }
            }

            var newValue = Value;
            if (!EqualityComparer<TResult>.Default.Equals(newValue, oldValue))
            {
                OnValueChanged(new ValueChangedEventArgs(oldValue, newValue));
                return new ValueChangedNotificationResult<TResult>(this, oldValue, newValue);
            }
            else
                return UnchangedNotificationResult.Instance;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Successors.Clear();
        }

        public IList<INotifiable> Successors { get { return successors; } }

        public IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();
    }
}
