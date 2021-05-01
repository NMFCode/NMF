using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Expressions.Linq
{
    public abstract class ObservableEnumerable<T> : INotifyEnumerable<T>, ICollection<T>, IEnumerable<T>, INotifyCollectionChanged, IDisposable, ISuccessorList
    {

        [DebuggerStepThrough]
        protected void OnAddItem(T item, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

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

        internal void RaiseEvents(IList<T> added, IList<T> removed, IList<T> moved, int oldItemsStartIndex = 0, int newItemsStartIndex = 0)
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

        [DebuggerStepThrough]
        protected void OnRemoveItem(T item, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

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

        [DebuggerStepThrough]
        protected void OnCleared()
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        [DebuggerStepThrough]
        protected void OnUpdateItem(T item, T old, int index = -1)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, old, index));
        }

        [DebuggerStepThrough]
        protected void OnMoveItem(T item, int oldIndex = 0, int newIndex = 0)
        {
            if (!HasEventSubscriber) return;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }

        [DebuggerStepThrough]
        protected void OnMoveItems(IEnumerable<T> items, int oldIndex = 0, int newIndex = 0)
        {
            if (!HasEventSubscriber || items == null) return;
            var moved = items as List<T> ?? items.ToList();
            if (moved.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move,
                moved, newIndex, oldIndex));
            }
        }

        protected bool HasEventSubscriber
        {
            get
            {
                return CollectionChanged != null;
            }
        }

        [DebuggerStepThrough]
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Detach();
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

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

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");

            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

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

        public ISuccessorList Successors => this;

        public abstract IEnumerable<INotifiable> Dependencies { get; }

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

        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion

        protected virtual void OnAttach() { }

        protected virtual void OnDetach() { }

        public abstract INotificationResult Notify(IList<INotificationResult> sources);
    }
}
