using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace NMF.Expressions.Linq.Facade
{
    internal sealed class BufferCollection<T> : IList, IReadOnlyCollection<T>, ICollection<T>, INotifyEnumerable<T>
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

        private void InnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _items.Clear();
                _items.AddRange(_elements);
                CollectionChanged?.Invoke(this, e);
                return;
            }
            if (e.OldItems != null)
            {
                RemoveAll(e.OldItems, e.OldStartingIndex);
            }
            if (e.NewItems != null)
            {
                Addtems(e.NewItems);
            }
        }

        private void RemoveAll(IList items, int stopIndex)
        {
            if (stopIndex < 0)
            {
                stopIndex = 0;
            }
            for (int i = _items.Count - 1; i >= stopIndex; i--)
            {
                var item = _items[i];
                if (items.Contains(item))
                {
                    _items.RemoveAt(i);
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, i));
                }
            }
        }

        private void Addtems(IList items)
        {
            var itemsProcessed = 0;
            var index = 0;
            if (items == null || items.Count == 0)
            {
                return;
            }
            foreach (var item in _elements)
            {
                if (index >= _items.Count || !EqualityComparer<T>.Default.Equals(_items[index], item))
                {
                    if (items.Contains(item))
                    {
                        _items.Insert(index, item);
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
                        itemsProcessed++;
                        if (itemsProcessed == items.Count)
                        {
                            return;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Error: Unexpected item {item} in the buffered collection.");
                        var oldItem = _items[index];
                        _items[index] = item;
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
                    }
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

        public bool IsOrdered => true;

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

        public void RequireOrder(bool isOrderRequired)
        {
            _elements.RequireOrder(isOrderRequired);
        }
    }
}
