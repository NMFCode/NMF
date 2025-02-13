using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace NMF.Expressions.Linq.Facade
{
    internal class BufferCollection<T> : INotifyCollectionChanged, IList, IReadOnlyCollection<T>, ICollection<T>, INotifyEnumerable<T>
    {
        private readonly List<T> _items;
        private readonly INotifyEnumerable<T> _elements;

        private const string UnderlyingCollectionIsReadOnly = "The underlying collection is read-only.";

        public BufferCollection(INotifyEnumerable<T> elements)
        {
            _items = new List<T>(elements);
            if (elements is INotifyCollectionChanged collectionChanged)
            {
                collectionChanged.CollectionChanged += InnerCollectionChanged;
            }
            _elements = elements;
        }

        private void InnerCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _items.Clear();
                _items.AddRange(_elements);
                CollectionChanged?.Invoke(this, e);
                return;
            }
            var oldItemsIndex = e.OldStartingIndex;
            var newItemsIndex = e.NewStartingIndex;
            if (e.OldItems != null)
            {
                oldItemsIndex = RemoveAll(e.OldItems, e.OldStartingIndex);
            }
            if (e.NewItems != null)
            {
                if (e.NewStartingIndex == -1)
                {
                    int idx = _items.Count;
                    foreach (var foundItem in FindItems(e.NewItems))
                    {
                        _items.Insert(foundItem.index, foundItem.item);
                        idx = Math.Min(idx, foundItem.index);
                    }
                    newItemsIndex = idx;
                }
                else
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        _items.Insert(e.NewStartingIndex + i, (T)e.NewItems[i]);
                    }
                }
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems, newItemsIndex));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems, oldItemsIndex));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewItems, e.OldItems, oldItemsIndex));
                    break;
                case NotifyCollectionChangedAction.Move:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, e.NewItems, newItemsIndex, oldItemsIndex));
                    break;
            }
        }

        private int RemoveAll(IList items, int stopIndex)
        {
            if (stopIndex < 0)
            {
                stopIndex = 0;
            }
            var idx = -1;
            for (int i = _items.Count - 1; i >= stopIndex; i--)
            {
                if (items.Contains(_items[i]))
                {
                    _items.RemoveAt(i);
                    idx = i;
                }
            }
            return idx;
        }

        private IEnumerable<(int index, T item)> FindItems(IList items)
        {
            var index = 0;
            var itemIndex = 0;
            if (items == null || items.Count == 0)
            {
                yield break;
            }
            var targetItem = (T)items[index];
            foreach (var item in _elements)
            {
                if (EqualityComparer<T>.Default.Equals(item, targetItem))
                {
                    yield return (index, item);
                    itemIndex++;
                    if (itemIndex == items.Count)
                    {
                        yield break;
                    }
                    targetItem = (T)items[itemIndex];
                }
                index++;
            }
        }

        public object this[int index] { get => ((IList)_items)[index]; set => ((IList)_items)[index] = value; }

        bool IList.IsFixedSize => ((IList)_items).IsFixedSize;

        bool ICollection.IsSynchronized => ((ICollection)_items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_items).SyncRoot;

        public ISuccessorList Successors => _elements.Successors;

        public IEnumerable<INotifiable> Dependencies => _elements.Dependencies;

        public ExecutionMetaData ExecutionMetaData => _elements.ExecutionMetaData;

        public int Count => _items.Count;

        public bool IsReadOnly => _elements is not ICollection<T> collection || collection.IsReadOnly;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        int IList.Add(object value)
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly && value is T item)
            {
                collection.Add(item);
                return collection.Count - 1;
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        void ICollection<T>.Add(T item)
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly)
            {
                collection.Add(item);
                return;
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        void IList.Clear()
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly)
            {
                collection.Clear();
                return;
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        void ICollection<T>.Clear()
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly)
            {
                collection.Clear();
                return;
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        bool IList.Contains(object value)
        {
            return ((IList)_items).Contains(value);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_items).CopyTo(array, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return ((IList)_items).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        void IList.Remove(object value)
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly && value is T item)
            {
                collection.Remove(item);
                return;
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        bool ICollection<T>.Remove(T item)
        {
            return Remove(item);
        }

        private bool Remove(T item)
        {
            if (_elements is ICollection<T> collection && !collection.IsReadOnly)
            {
                return collection.Remove(item);
            }
            throw new NotSupportedException(UnderlyingCollectionIsReadOnly);
        }

        void IList.RemoveAt(int index)
        {
            Remove(_items[index]);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return _elements.Notify(sources);
        }

        public void Dispose()
        {
        }
    }
}
