using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class OrderedSet<T> : DecoratedSet<T>, ISet<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IOrderedSet<T>
    {
        private List<T> itemOrder = new List<T>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        protected virtual void OnInsertingItem(T item, ref bool cancel, int index) { }

        protected virtual void OnInsertItem(T item, int index) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        protected virtual void OnRemovingItem(T item, ref bool cancel, int index) { }

        protected virtual void OnRemoveItem(T item, int index) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#")]
        protected virtual void OnReplacingItem(T oldItem, T newItem, ref bool cancel) { }

        protected virtual void OnReplaceItem(T oldItem, T newItem) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual void OnClearing(ref bool cancel) { }

        protected virtual void OnCleared() { }

        public override bool Add(T item)
        {
            if (!Items.Contains(item))
            {
                bool cancel = false;
                var index = Count;
                OnInsertingItem(item, ref cancel, index);
                if (cancel) return false;
                Items.Add(item);
                itemOrder.Add(item);
                OnInsertItem(item, index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Remove(T item)
        {
            return Remove(item, IndexOf(item));
        }

        private bool Remove(T item, int index)
        {
            if (Items.Contains(item))
            {
                bool cancel = false;
                OnRemovingItem(item, ref cancel, index);
                if (cancel) return false;
                Items.Remove(item);
                itemOrder.RemoveAt(index);
                OnRemoveItem(item, index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Clear()
        {
            bool cancel = false;
            OnClearing(ref cancel);
            if (cancel) return;
            base.Clear();
            itemOrder.Clear();
            OnCleared();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return itemOrder.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return itemOrder.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (!Items.Contains(item))
            {
                bool cancel = false;
                OnInsertingItem(item, ref cancel, index);
                if (cancel) return;
                Items.Add(item);
                itemOrder.Insert(index, item);
                OnInsertItem(item, index);
            }
        }

        public void RemoveAt(int index)
        {
            Remove(itemOrder[index], index);
        }

        public T this[int index]
        {
            get
            {
                return itemOrder[index];
            }
            set
            {
                var item = itemOrder[index];
                if (!object.Equals(item, value))
                {
                    bool cancel = false;
                    OnReplacingItem(item, value, ref cancel);
                    if (cancel) return;
                    itemOrder[index] = value;
                    Items.Remove(item);
                    Items.Add(value);
                    OnReplaceItem(item, value);
                }
            }
        }

        int IList.Add(object value)
        {
            Add((T)value);
            return itemOrder.Count - 1;
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            Remove((T)value);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)itemOrder).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)itemOrder).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)itemOrder).SyncRoot; }
        }


        bool IList.IsReadOnly
        {
            get { return false; }
        }

        public ReadOnlyOrderedSet<T> AsReadOnly()
        {
            return new ReadOnlyOrderedSet<T>(this);
        }
    }
}
