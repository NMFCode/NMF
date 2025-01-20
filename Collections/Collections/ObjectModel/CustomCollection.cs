using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes the base class for a custom collection
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    /// <remarks>The idea behind this class is that developers turn an IEnumerableExpression into a ICollectionExpression by implementing Add, Remove and Clear</remarks>
    public abstract class CustomCollection<T> : ICollectionExpression<T>
    {
        /// <summary>
        /// Gets the inner collection
        /// </summary>
        public IEnumerableExpression<T> Inner { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">The inner collection</param>
        /// <exception cref="ArgumentNullException">Thrown if the inner collection is null</exception>
        protected CustomCollection(IEnumerableExpression<T> inner)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));

            Inner = inner;
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return Inner.Count();
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public abstract void Add(T item);

        /// <inheritdoc />
        public INotifyCollection<T> AsNotifiable()
        {
            return new Notifiable(this, Inner.AsNotifiable());
        }

        /// <inheritdoc />
        public abstract void Clear();

        /// <inheritdoc />
        public virtual bool Contains(T item)
        {
            return Inner.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in Inner)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        /// <inheritdoc />
        public abstract bool Remove(T item);

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class Notifiable : INotifyCollection<T>
        {
            public CustomCollection<T> Parent { get; private set; }
            public INotifyEnumerable<T> Inner { get; private set; }

            public Notifiable(CustomCollection<T> parent, INotifyEnumerable<T> inner)
            {
                Parent = parent;
                Inner = inner;
            }

            public ISuccessorList Successors
            {
                get
                {
                    return Inner.Successors;
                }
            }

            public IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    return Inner.Dependencies;
                }
            }

            public ExecutionMetaData ExecutionMetaData
            {
                get
                {
                    return Inner.ExecutionMetaData;
                }
            }

            public int Count
            {
                get
                {
                    return Inner.Count();
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add
                {
                    Inner.CollectionChanged += value;
                }
                remove
                {
                    Inner.CollectionChanged -= value;
                }
            }

            public void Add(T item)
            {
                Parent.Add(item);
            }

            public void Clear()
            {
                Parent.Clear();
            }

            public bool Contains(T item)
            {
                return Parent.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                Parent.CopyTo(array, arrayIndex);
            }

            public void Dispose()
            {
                Inner.Dispose();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return Inner.GetEnumerator();
            }

            public INotificationResult Notify(IList<INotificationResult> sources)
            {
                return Inner.Notify(sources);
            }

            public bool Remove(T item)
            {
                return Parent.Remove(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
