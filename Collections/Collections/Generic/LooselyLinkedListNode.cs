using System;
using System.Collections;
using System.Collections.Generic;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Denotes a loosely linked list implementation
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class LooselyLinkedList<T> : ICollection<T>
    {
        private readonly LooselyLinkedListNode<T> first;
        private LooselyLinkedListNode<T> last;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LooselyLinkedList()
        {
            first = new LooselyLinkedListNode<T>(default(T));
            last = first;
        }

        /// <summary>
        /// Gets the last node
        /// </summary>
        public LooselyLinkedListNode<T> Last
        {
            get
            {
                return last;
            }
        }

        /// <summary>
        /// Gets the first node
        /// </summary>
        public LooselyLinkedListNode<T> First
        {
            get
            {
                return first;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all nodes
        /// </summary>
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

        /// <inheritdoc />
        public void Add(T item)
        {
            var newNode = new LooselyLinkedListNode<T>(item);
            last.Next = newNode;
            last = newNode;
        }

        /// <summary>
        /// Adds the provided node
        /// </summary>
        /// <param name="newNode">The node to add</param>
        public void Add(LooselyLinkedListNode<T> newNode)
        {
            AddAfter(last, newNode);
        }

        /// <summary>
        /// Adds the given node after the provided node
        /// </summary>
        /// <param name="node">The node after which the new node should be added</param>
        /// <param name="newNode">The new node</param>
        /// <exception cref="ArgumentNullException">Thrown if either is null</exception>
        public void AddAfter(LooselyLinkedListNode<T> node, LooselyLinkedListNode<T> newNode)
        {
            if (newNode == null) throw new ArgumentNullException(nameof(newNode));
            if (node == null) throw new ArgumentNullException(nameof(node));
            
            var lastNew = newNode;
            while (lastNew.Next != null)
            {
                lastNew = lastNew.Next;
            }
            if (node == last) last = lastNew;
            lastNew.Next = node.Next;
            node.Next = newNode;
        }

        /// <summary>
        /// Adds the given value after the provided node
        /// </summary>
        /// <param name="node">The node after which the new node should be added</param>
        /// <param name="value">The value to add</param>
        public void AddAfter(LooselyLinkedListNode<T> node, T value)
        {
            AddAfter(node, new LooselyLinkedListNode<T>(value));
        }

        /// <summary>
        /// Adds the given node at the beginning of the list
        /// </summary>
        /// <param name="newNode">the node to add</param>
        public void AddFirst(LooselyLinkedListNode<T> newNode)
        {
            AddAfter(first, newNode);
        }

        /// <inheritdoc />
        public void Clear()
        {
            first.Next = null;
            last = first;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(first);
        }

        /// <summary>
        /// Cut the linked list after the given node
        /// </summary>
        /// <param name="node">The node of the linked list</param>
        /// <exception cref="ArgumentNullException">Thrown if node is null</exception>
        public void CutAfter(LooselyLinkedListNode<T> node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            node.Next = null;
            last = node;
        }

        /// <inheritdoc />
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

        private sealed class Enumerator : IEnumerator<T>
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

    /// <summary>
    /// Denotes a node in a loosely linked list
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class LooselyLinkedListNode<T>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="value">The value represented by this node</param>
        public LooselyLinkedListNode(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of this node
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets the next node
        /// </summary>
        public LooselyLinkedListNode<T> Next { get; set; }

        /// <summary>
        /// Gets a collection with all values starting at the current node
        /// </summary>
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

        /// <summary>
        /// Removes the next element
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if there is no next element</exception>
        public void CutNext()
        {
            if (Next == null) throw new InvalidOperationException("Next is null");
            var nextNext = Next.Next;
            Next = nextNext;
        }

        /// <summary>
        /// Creates a dummy node for the given node
        /// </summary>
        /// <param name="node">The node for which to create a dummy node</param>
        /// <returns>A new node whose next pointer points to the given node</returns>
        public static LooselyLinkedListNode<T> CreateDummyFor(LooselyLinkedListNode<T> node)
        {
            var dummy = new LooselyLinkedListNode<T>(default(T));
            dummy.Next = node;
            return dummy;
        }
    }
}
