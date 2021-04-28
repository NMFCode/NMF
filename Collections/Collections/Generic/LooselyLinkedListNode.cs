using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    public class LooselyLinkedList<T> : ICollection<T>
    {
        private readonly LooselyLinkedListNode<T> first;
        private LooselyLinkedListNode<T> last;

        public LooselyLinkedList()
        {
            first = new LooselyLinkedListNode<T>(default(T));
            last = first;
        }

        public LooselyLinkedListNode<T> Last
        {
            get
            {
                return last;
            }
        }

        public LooselyLinkedListNode<T> First
        {
            get
            {
                return first;
            }
        }

        public int Count
        {
            get
            {
                var count = 0;
                var current = first;
                while (current.Next != null)
                {
                    count++;
                    current = current.Next;
                }
                return count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<LooselyLinkedListNode<T>> Nodes
        {
            get
            {
                var current = first.Next;
                while (current != null)
                {
                    yield return current;
                    current = current.Next;
                }
            }
        }

        public void Add(T item)
        {
            var newNode = new LooselyLinkedListNode<T>(item);
            last.Next = newNode;
            last = newNode;
        }

        public void Add(LooselyLinkedListNode<T> newNode)
        {
            AddAfter(last, newNode);
        }

        public void AddAfter(LooselyLinkedListNode<T> node, LooselyLinkedListNode<T> newNode)
        {
            if (newNode == null) throw new ArgumentNullException("newNode");
            if (node == null) throw new ArgumentNullException("node");
            
            var lastNew = newNode;
            while (lastNew.Next != null)
            {
                lastNew = lastNew.Next;
            }
            if (node == last) last = lastNew;
            lastNew.Next = node.Next;
            node.Next = newNode;
        }

        public void AddAfter(LooselyLinkedListNode<T> node, T value)
        {
            AddAfter(node, new LooselyLinkedListNode<T>(value));
        }

        public void AddFirst(LooselyLinkedListNode<T> newNode)
        {
            AddAfter(first, newNode);
        }

        public void Clear()
        {
            first.Next = null;
            last = first;
        }

        public bool Contains(T item)
        {
            var current = first.Next;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(item, current.Value))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var current = first.Next;
            while (current != null)
            {
                array[arrayIndex] = current.Value;
                arrayIndex++;
                current = current.Next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(first);
        }

        public void CutAfter(LooselyLinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            node.Next = null;
            last = node;
        }

        public bool Remove(T item)
        {
            var current = first;
            while (current.Next != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Next.Value, item))
                {
                    current.Next = current.Next.Next;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator : IEnumerator<T>
        {
            private LooselyLinkedListNode<T> current;
            private readonly LooselyLinkedListNode<T> first;

            public Enumerator(LooselyLinkedListNode<T> first)
            {
                this.current = first;
                this.first = first;
            }

            public T Current
            {
                get
                {
                    return current.Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return current.Value;
                }
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                current = current.Next;
                return current != null;
            }

            public void Reset()
            {
                current = first;
            }
        }
    }

    public class LooselyLinkedListNode<T>
    {
        public LooselyLinkedListNode(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public LooselyLinkedListNode<T> Next { get; set; }

        public IEnumerable<T> FromHere
        {
            get
            {
                var current = this;
                while (current != null)
                {
                    yield return current.Value;
                    current = current.Next;
                }
            }
        }

        public void CutNext()
        {
            if (Next == null) throw new InvalidOperationException("Next is null");
            var nextNext = Next.Next;
            Next = nextNext;
        }

        public static LooselyLinkedListNode<T> CreateDummyFor(LooselyLinkedListNode<T> node)
        {
            var dummy = new LooselyLinkedListNode<T>(default(T));
            dummy.Next = node;
            return dummy;
        }
    }
}
