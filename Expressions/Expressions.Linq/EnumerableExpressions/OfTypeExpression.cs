﻿using System;
using System.Collections.Generic;
using System.Linq;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;
using System.Collections;
using System.Collections.Specialized;

namespace NMF.Expressions
{
    internal class OfTypeExpression<T> : IEnumerableExpression<T>
    {
        public IEnumerableExpression Source { get; private set; }
        private INotifyEnumerable<T> notifyEnumerable;

        public OfTypeExpression(IEnumerableExpression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Source = source;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().OfType<T>();
            }
            return notifyEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.OfType<T>(Source).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }

    internal class OfTypeCollectionExpression<TSource, T> : OfTypeExpression<T>, ICollectionExpression<T>
        where T : TSource
    {
        private readonly ICollectionExpression<TSource> casted;
        public OfTypeCollectionExpression(ICollectionExpression<TSource> source) : base(source)
        {
            casted = source;
        }

        public int Count
        {
            get
            {
                return SL.OfType<T>(Source).Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return casted.IsReadOnly;
            }
        }

        public void Add(T item)
        {
            casted.Add(item);
        }

        public void Clear()
        {
            foreach (var item in SL.OfType<T>(Source).ToArray())
            {
                casted.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return casted.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public bool Remove(T item)
        {
            return casted.Remove(item);
        }

        INotifyCollection<T> ICollectionExpression<T>.AsNotifiable()
        {
            return casted.AsNotifiable().OfType<TSource, T>();
        }
    }

    internal class OfTypeCollectionExpression<T> : OfTypeExpression<T>, ICollectionExpression<T>
    {
        private readonly ICollectionExpression casted;
        public OfTypeCollectionExpression(ICollectionExpression source) : base(source)
        {
            casted = source;
        }

        public int Count
        {
            get
            {
                return SL.OfType<T>(casted).Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            casted.Add(item);
        }

        public void Clear()
        {
            foreach (var item in SL.OfType<T>(Source).ToArray())
            {
                casted.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return casted.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public bool Remove(T item)
        {
            if (casted.Contains(item))
            {
                casted.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        INotifyCollection<T> ICollectionExpression<T>.AsNotifiable()
        {
            return new Notifiable(casted.AsNotifiable().OfType<T>(), casted);
        }

        private class Notifiable : INotifyCollection<T>
        {
            private readonly INotifyEnumerable<T> inner;
            private readonly IList underlyingCollection;

            public ISuccessorList Successors { get; }

            public IEnumerable<INotifiable> Dependencies { get { yield return inner; } }
            public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

            public Notifiable(INotifyEnumerable<T> inner, IList underlyingCollection)
            {
                this.inner = inner;
                this.underlyingCollection = underlyingCollection;

                var successors = new MultiSuccessorList();
                successors.Attached += (obj, e) => Attach();
                successors.Detached += (obj, e) => Detach();
                Successors = successors;
            }

            public int Count
            {
                get
                {
                    return SL.Count(inner);
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return underlyingCollection.IsReadOnly;
                }
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Add(T item)
            {
                underlyingCollection.Add(item);
            }

            public void Clear()
            {
                foreach (var item in inner.ToArray())
                {
                    underlyingCollection.Remove(item);
                }
            }

            public bool Contains(T item)
            {
                return underlyingCollection.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                foreach (var item in this)
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return inner.GetEnumerator();
            }

            public bool Remove(T item)
            {
                if (underlyingCollection.Contains(item))
                {
                    underlyingCollection.Remove(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return inner.GetEnumerator();
            }

            private void Attach()
            {
                foreach (var dep in Dependencies)
                    dep.Successors.Set(this);
            }

            private void Detach()
            {
                foreach (var dep in Dependencies)
                    dep.Successors.Unset(this);
            }

            public void Dispose()
            {
                Detach();
            }

            public INotificationResult Notify(IList<INotificationResult> sources)
            {
                var change = (ICollectionChangedNotificationResult<T>)sources[0];

                var handler = CollectionChanged;
                if (handler != null)
                {
                    if (change.IsReset)
                    {
                        handler.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                    else
                    {
                        if (change.RemovedItems.Any())
                        {
                            handler.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, change.RemovedItems));
                        }
                        if (change.AddedItems.Any())
                        {
                            handler.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, change.AddedItems));
                        }
                    }
                }

                return CollectionChangedNotificationResult<T>.Transfer(change, this);
            }
        }
    }
}
