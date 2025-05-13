using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Diagnostics;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes an abstract base class for collection-valued DDG nodes
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public abstract class ObservableEnumerable<T> : INotifyEnumerable<T>, ICollection<T>, IEnumerable<T>, INotifyCollectionChanged, IDisposable, ISuccessorList, IList
    {
        /// <summary>
        /// Raises the collection changed event for an added item
        /// </summary>
        /// <param name="item">The item that is added</param>
        /// <param name="index">The index at which the item is added or -1</param>
        [DebuggerStepThrough]
        protected void OnAddItem(T item, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Raises the collection changed event for added items
        /// </summary>
        /// <param name="items">The items that are added</param>
        /// <param name="index">The start index at which items are added</param>
        [DebuggerStepThrough]
        protected void OnAddItems(IEnumerable<T> items, int index = -1)
        {
            if (!HasEventSubscriber || items == null) return;
            var added = items as List<T> ?? items.ToList();
            if (added.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added, index));
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "[" + GetType().Name + "]";
        }

        /// <summary>
        /// Raises the events for the collections of changes
        /// </summary>
        /// <param name="added">A list of the added items</param>
        /// <param name="removed">A list of the removed items</param>
        /// <param name="moved">A list of the moved items</param>
        /// <param name="oldItemsStartIndex">the start index of old items or -1</param>
        /// <param name="newItemsStartIndex">the start index of new items or -1</param>
        protected void RaiseEvents(IList<T> added, IList<T> removed, IList<T> moved, int oldItemsStartIndex = -1, int newItemsStartIndex = -1)
        {
            if (added != null && removed != null && added.Count == removed.Count)
            {
                OnReplaceItems(removed, added, newItemsStartIndex);
            }
            else
            {
                OnRemoveItems(removed, oldItemsStartIndex);
                OnAddItems(added, newItemsStartIndex);
            }
            OnMoveItems(moved, oldItemsStartIndex, newItemsStartIndex);
        }

        /// <summary>
        /// Raises the event that an item was removed
        /// </summary>
        /// <param name="item">The item that was removed</param>
        /// <param name="index">The index at which the item was removed or -1</param>
        [DebuggerStepThrough]
        protected void OnRemoveItem(T item, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        /// <summary>
        /// Raises the event that items were removed
        /// </summary>
        /// <param name="items">The items that have been removed</param>
        /// <param name="index">The index at which items have been removed</param>
        [DebuggerStepThrough]
        protected void OnRemoveItems(IEnumerable<T> items, int index = -1)
        {
            if (!HasEventSubscriber || items == null) return;
            var removed = items as List<T> ?? items.ToList();
            if (removed.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed, index));
            }
        }

        /// <summary>
        /// Raises the event that items were replaced
        /// </summary>
        /// <param name="oldItems">the old items</param>
        /// <param name="newItems">the new items</param>
        /// <param name="index">the index at which the items have been replaced</param>
        [DebuggerStepThrough]
        protected void OnReplaceItems(IEnumerable<T> oldItems, IEnumerable<T> newItems, int index = -1)
        {
            if (!HasEventSubscriber) return;
            var added = newItems as List<T> ?? newItems.ToList();
            var removed = oldItems as List<T> ?? oldItems.ToList();
            if (added.Count > 0 && removed.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    added, removed, index));
            }
        }

        /// <summary>
        /// Raises the event that the collection was cleared
        /// </summary>
        [DebuggerStepThrough]
        protected void OnCleared()
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raises the event that the item was replaced
        /// </summary>
        /// <param name="item">the new item</param>
        /// <param name="old">the old item</param>
        /// <param name="index">the index of the elemnt or -1</param>
        [DebuggerStepThrough]
        protected void OnReplaceItem(T old, T item, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, old, index));
        }

        /// <summary>
        /// Raises the event that the item was moved
        /// </summary>
        /// <param name="item">the moved item</param>
        /// <param name="oldIndex">the old index</param>
        /// <param name="newIndex">the new index</param>
        [DebuggerStepThrough]
        protected void OnMoveItem(T item, int oldIndex = 0, int newIndex = 0)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, Math.Max( newIndex, 0), Math.Max( oldIndex, 0)));
        }

        /// <summary>
        /// Raises the event that the items were moved
        /// </summary>
        /// <param name="items">the moved items</param>
        /// <param name="oldIndex">the old index</param>
        /// <param name="newIndex">the new index</param>
        [DebuggerStepThrough]
        protected void OnMoveItems(IEnumerable<T> items, int oldIndex = 0, int newIndex = 0)
        {
            if (!HasEventSubscriber || items == null) return;
            var moved = items as List<T> ?? items.ToList();
            if (moved.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move,
                moved, Math.Max( newIndex, 0 ), Math.Max( oldIndex, 0 ) ) );
            }
        }

        /// <summary>
        /// Indicates whether the collection has event subscribers attached
        /// </summary>
        protected bool HasEventSubscriber
        {
            get
            {
                return CollectionChanged != null;
            }
        }

        /// <summary>
        /// Raises a collection changed event
        /// </summary>
        /// <param name="e">the event data</param>
        [DebuggerStepThrough]
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc />
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Detach();
        }

        /// <inheritdoc />
        public virtual void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc />
        public virtual bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual bool Contains(T item)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            foreach (var element in this)
            {
                if (comparer.Equals(element, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public virtual int Count
        {
            get
            {
                var counter = 0;
                using (var enumerator = GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }

        /// <inheritdoc />
        public ISuccessorList Successors => this;

        /// <inheritdoc />
        public abstract IEnumerable<INotifiable> Dependencies { get; }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            OnAttach();
        }

        private void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        #region SuccessorList

        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        int ISuccessorList.Count => successors.Count;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => successors;

        /// <inheritdoc />
        public virtual bool IsFixedSize => false;

        /// <inheritdoc />
        public bool IsSynchronized => false;

        /// <inheritdoc />
        public object SyncRoot => null;

        /// <inheritdoc />
        public virtual object this[int index] { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }


        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
            {
                isDummySet = false;
            }
            else
            {
                if (successors.Count == 1)
                {
                    Attach();
                }
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attach();
            }
        }


        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
            {
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            }
            if (!(isDummySet = leaveDummy))
            {
                Detach();
            }
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detach();
            }
        }

        /// <inheritdoc />
        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion

        /// <summary>
        /// Gets called when a successor attaches and there was no successor before
        /// </summary>
        protected virtual void OnAttach() { }

        /// <summary>
        /// Gets called when the last successor detaches
        /// </summary>
        protected virtual void OnDetach() { }

        /// <inheritdoc />
        public abstract INotificationResult Notify(IList<INotificationResult> sources);


        #region ICollection methods

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        object IList.this[int index] { get => GetElementAt(index); set => throw new NotSupportedException(); }

        private T GetElementAt(int index)
        {
            foreach (var item in this)
            {
                if (index == 0)
                {
                    return item;
                }
                index--;
            }
#pragma warning disable S112 // General or reserved exceptions should never be thrown
            throw new IndexOutOfRangeException();
#pragma warning restore S112 // General or reserved exceptions should never be thrown
        }

        int IList.Add(object value)
        {
            if (value is T casted)
            {
                Add(casted);
                return Count;
            }
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            if (value is T typeCasted)
            {
                return Contains(typeCasted);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (value is T typecasted)
            {
                var index = 0;
                foreach (var item in this)
                {
                    if (EqualityComparer<T>.Default.Equals(item, typecasted))
                    {
                        return index;
                    }
                    index++;
                }
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (value is T casted)
            {
                Add(casted);
                return;
            }
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            if (value is T typeCasted)
            {
                Remove(typeCasted);
            }
        }

        void IList.RemoveAt(int index)
        {
            Remove(GetElementAt(index));
        }

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (var item in this)
            {
                array.SetValue(item, index);
                index++;
            }
        }

        #endregion
    }
}
