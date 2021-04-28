using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ReadOnlyOrderedSet<T> : ISet<T>, ICollection<T>, IList<T>, IList, ICollection, IEnumerable<T>, IEnumerable, IOrderedSet<T>
    {
        public ReadOnlyOrderedSet(OrderedSet<T> parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            this.parent = parent;
        }

        private readonly OrderedSet<T> parent;

        public bool Add(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return parent.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return parent.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return parent.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return parent.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return parent.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return parent.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public void Clear()
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public bool Contains(T item)
        {
            return parent.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            parent.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return parent.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return parent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parent.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return parent.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public void RemoveAt(int index)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public T this[int index]
        {
            get
            {
                return parent[index];
            }
            set
            {
                throw new InvalidOperationException("This collection is read-only and must not be modified");
            }
        }

        public int Add(object value)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public bool Contains(object value)
        {
            return parent.Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return parent.IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        object IList.this[int index]
        {
            get
            {
                return parent[index];
            }
            set
            {
                throw new InvalidOperationException("This collection is read-only and must not be modified");
            }
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)parent).CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)parent).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)parent).SyncRoot; }
        }
    }
}
