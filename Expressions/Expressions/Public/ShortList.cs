using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class ShortList<T> : IList<T>, INotifyCollectionChanged
    {
        private static NotifyCollectionChangedEventArgs addArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new object());
        private static NotifyCollectionChangedEventArgs removeArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new object());
        private static NotifyCollectionChangedEventArgs resetArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        private T head;
        private bool usesHead = false;
        private List<T> tail = new List<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public T this[int index]
        {
            get { return index == 0 ? head : tail[index - 1]; }
            set { Insert(index, value); }
        }

        public int Count
        {
            get { return usesHead ? tail.Count + 1 : 0; }
        }

        public bool IsReadOnly { get { return false; } }

        public ShortList() { }

        public ShortList(T item)
        {
            head = item;
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            if (usesHead)
            {
                tail.Clear();
                head = default(T);
                usesHead = false;
                RaiseCollectionChanged(resetArgs);
            }
        }

        public bool Contains(T item)
        {
            if (ReferenceEquals(item, head) || EqualityComparer<T>.Default.Equals(item, head))
                return true;
            return tail.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (usesHead)
            {
                array[arrayIndex] = head;
                tail.CopyTo(array, arrayIndex + 1);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            if (ReferenceEquals(item, head) || EqualityComparer<T>.Default.Equals(item, head))
                return 0;
            var tailResult = tail.IndexOf(item);
            return tailResult < 0 ? tailResult : tailResult + 1;
        }

        public void Insert(int index, T item)
        {
            if (index == 0)
            {
                head = item;
                usesHead = true;
            }
            else
                tail.Insert(index - 1, item);
            RaiseCollectionChanged(addArgs);
        }

        public bool Remove(T item)
        {
            if (!usesHead)
                return false;

            if (ReferenceEquals(item, head) || EqualityComparer<T>.Default.Equals(item, head))
            {
                if (tail.Count == 0)
                {
                    head = default(T);
                    usesHead = false;
                }
                else
                {
                    head = tail[0];
                    tail.RemoveAt(0);
                }
                RaiseCollectionChanged(removeArgs);
                return true;
            }

            if (tail.Remove(item))
            {
                RaiseCollectionChanged(removeArgs);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index == 0)
            {
                head = default(T);
                usesHead = false;
            }
            else
            {
                tail.RemoveAt(index - 1);
            }
            RaiseCollectionChanged(removeArgs);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }
    }
}
