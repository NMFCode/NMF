using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a class optimized for a single element
    /// </summary>
    [DebuggerDisplay("{isDummySet ? \"<dummy>\" : successor?.ToString()}")]
    [DebuggerStepThrough]
    public class SingleSuccessorList : ISuccessorList, IEnumerable<INotifiable>
    {
        private bool isDummySet = false;
        private INotifiable successor;

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successor != null;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successor != null;

        /// <inheritdoc />
        public int Count => successor != null ? 1 : 0;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => this;

        /// <inheritdoc />
        public event EventHandler Attached;
        /// <inheritdoc />
        public event EventHandler Detached;

        /// <inheritdoc />
        public IEnumerator<INotifiable> GetEnumerator()
        {
            yield return successor;
        }

        /// <inheritdoc />
        public INotifiable GetSuccessor(int index)
        {
            if (index != 0)
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112 // General or reserved exceptions should never be thrown
            return isDummySet ? null : successor;
        }

        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (successor != null)
                throw new InvalidOperationException("Only one successor is allowed.");

            successor = node;
            if (isDummySet)
                isDummySet = false;
            else
                Attached?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void SetDummy()
        {
            if (successor == null && !isDummySet)
            {
                isDummySet = true;
                Attached?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node != successor)
                throw new InvalidOperationException("The specified node is not registered as the successor.");

            successor = null;
            if (!(isDummySet = leaveDummy))
                Detached?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successor = null;
                Detached?.Invoke(this, EventArgs.Empty);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
