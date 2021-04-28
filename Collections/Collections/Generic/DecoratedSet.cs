using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    public class DecoratedSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IList
    {
        protected HashSet<T> Items { get; private set; }

        public DecoratedSet()
        {
            Items = new HashSet<T>();
        }

        public virtual bool Add(T item)
        {
            return item != null && Items.Add(item);
        }

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

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return Items.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return Items.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return Items.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return Items.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return Items.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return Items.SetEquals(other);
        }

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

        public virtual void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Items.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return Items.Remove(item);
        }

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
            ((ICollection)Items).CopyTo(array, index);
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
            get { return ((ICollection)Items).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)Items).SyncRoot; }
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
