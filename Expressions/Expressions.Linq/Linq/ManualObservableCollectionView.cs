using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ManualObservableCollectionView<T> : INotifyEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private IEnumerable<T> source;

        public ManualObservableCollectionView(IEnumerable<T> source)
        {
            this.source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }

        public void NotifyAddItems(IEnumerable<T> addedItems)
        {
            if (addedItems != null)
            {
                List<T> items = addedItems as List<T>;
                if (items == null) items = addedItems.ToList();
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
            }
        }

        public void NotifyAddItem(T item)
        {
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void NotifyRemoveItems(IEnumerable<T> removedItems)
        {
            if (removedItems != null)
            {
                List<T> items = removedItems as List<T>;
                if (items == null) items = removedItems.ToList();
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
            }
        }

        public void NotifyRemoveItem(T item)
        {
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public void NotifyReset()
        {
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Attach()
        {
        }

        public void Detach()
        {
        }

        public bool IsAttached
        {
            get { return true; }
        }

        public void Dispose()
        {
        }
    }
}
