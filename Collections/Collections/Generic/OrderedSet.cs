using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Denotes an implementation of an ordered set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class OrderedSet<T> : DecoratedSet<T>, ISet<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IOrderedSet<T>
    {
        private readonly List<T> itemOrder = new List<T>();

        /// <inheritdoc />
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

        /// <inheritdoc />
        public sealed override bool Remove(T item)
        {
            return Remove(item, IndexOf(item));
        }

        /// <summary>
        /// Removes the given item at the given index
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <param name="index">The index of the item</param>
        /// <returns>True, if the removal was successful, otherwise false</returns>
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

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();
            itemOrder.Clear();
        }

        /// <inheritdoc />
        public override IEnumerator<T> GetEnumerator()
        {
            return itemOrder.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return itemOrder.IndexOf(item);
        }

        /// <inheritdoc />
        public virtual void Insert(int index, T item)
        {
            if (Items.Add(item))
            {
                itemOrder.Insert(index, item);
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            Remove(itemOrder[index], index);
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Replaces the item at the given index
        /// </summary>
        /// <param name="index">The index on which the item is replaced</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
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

        /// <summary>
        /// Returns a readonly view of the oredered set
        /// </summary>
        /// <returns></returns>
        public ReadOnlyOrderedSet<T> AsReadOnly()
        {
            return new ReadOnlyOrderedSet<T>(this);
        }
    }
}
