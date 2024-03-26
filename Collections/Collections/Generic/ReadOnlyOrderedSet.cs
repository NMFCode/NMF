using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Denotes a readonly ordered set view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ReadOnlyOrderedSet<T> : ISet<T>, ICollection<T>, IList<T>, IList, ICollection, IEnumerable<T>, IEnumerable, IOrderedSet<T>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">The ordered set of which to create a view</param>
        /// <exception cref="ArgumentNullException">Thrown if parent is null</exception>
        public ReadOnlyOrderedSet(OrderedSet<T> parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            this.parent = parent;
        }

        private readonly OrderedSet<T> parent;

        /// <inheritdoc />
        public bool Add(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return parent.IsProperSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return parent.IsProperSupersetOf(other);
        }

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return parent.IsSubsetOf(other);
        }

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return parent.IsSupersetOf(other);
        }

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            return parent.Overlaps(other);
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            return parent.SetEquals(other);
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return parent.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            parent.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return parent.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return parent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parent.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return parent.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public int Add(object value)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public bool Contains(object value)
        {
            return parent.Contains((T)value);
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            return parent.IndexOf((T)value);
        }

        /// <inheritdoc />
        public void Insert(int index, object value)
        {
            throw new InvalidOperationException("This collection is read-only and must not be modified");
        }

        /// <inheritdoc />
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            ((ICollection)parent).CopyTo(array, index);
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return ((ICollection)parent).IsSynchronized; }
        }

        /// <inheritdoc />
        public object SyncRoot
        {
            get { return ((ICollection)parent).SyncRoot; }
        }
    }
}
