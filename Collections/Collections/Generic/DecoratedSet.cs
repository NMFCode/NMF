using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Denotes an extensible implementation of a hashset
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DecoratedSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IList
    {
        /// <summary>
        /// The actual hashset in which items are stored
        /// </summary>
        protected HashSet<T> Items { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public DecoratedSet()
        {
            Items = new HashSet<T>();
        }

        /// <inheritdoc />
        public virtual bool Add(T item)
        {
            return item != null && Items.Add(item);
        }

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null) return;
            if (other == this)
            {
                Clear();
                return;
            }
            foreach (var item in other)
            {
                Remove(item);
            }
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other != null)
            {
                if (other != this)
                {
                    foreach (var item in this.ToArray())
                    {
                        if (!other.Contains(item)) Remove(item);
                    }
                }
            }
            else
            {
                Clear();
            }
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return Items.IsProperSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return Items.IsProperSupersetOf(other);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return Items.IsSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return Items.IsSupersetOf(other);
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            return Items.Overlaps(other);
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            return Items.SetEquals(other);
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other != null)
            {
                if (other != this)
                {
                    foreach (var item in other)
                    {
                        if (Items.Contains(item))
                        {
                            Remove(item);
                        }
                        else
                        {
                            Add(item);
                        }
                    }
                }
                else
                {
                    Clear();
                }
            }
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other)
        {
            if (other != null && other != this)
            {
                foreach (var item in other)
                {
                    Add(item);
                }
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            Items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return Items.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public virtual bool Remove(T item)
        {
            return Items.Remove(item);
        }

        /// <inheritdoc />
        public virtual IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            foreach(var item in Items)
            {
                array.SetValue(item, index++);
            }
        }

        int IList.Add(object value)
        {
            Add((T)value);
            return 0;
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        void IList.Clear()
        {
            Clear();
        }

        int IList.IndexOf(object value)
        {
            throw new InvalidOperationException();
        }

        void IList.Insert(int index, object value)
        {
            Add((T)value);
        }

        void IList.Remove(object value)
        {
            Remove((T)value);
        }

        void IList.RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                throw new InvalidOperationException();
            }

            set
            {
                throw new InvalidOperationException();
            }
        }
    }
}
