using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class OrderedSet<T> : DecoratedSet<T>, ISet<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IOrderedSet<T>
    {
        private readonly List<T> itemOrder = new List<T>();

        public override bool Add(T item)
        {
            if (Items.Add(item))
            {
                itemOrder.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public sealed override bool Remove(T item)
        {
            return Remove(item, IndexOf(item));
        }

        protected virtual bool Remove(T item, int index)
        {
            if (Items.Remove(item))
            {
                itemOrder.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Clear()
        {
            base.Clear();
            itemOrder.Clear();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return itemOrder.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return itemOrder.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            if (Items.Add(item))
            {
                itemOrder.Insert(index, item);
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
                if (!EqualityComparer<T>.Default.Equals(item, value))
                {
                    Replace(index, item, value);
                }
            }
        }

        protected virtual void Replace(int index, T oldValue, T newValue)
        {
            itemOrder[index] = newValue;
            Items.Remove(oldValue);
            Items.Add(newValue);
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
