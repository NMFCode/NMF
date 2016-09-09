using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    [DebuggerDisplay("{isDummySet ? \"<dummy>\" : successor?.ToString()}")]
    [DebuggerStepThrough]
    public class SingleSuccessorList : ISuccessorList
    {
        private bool isDummySet = false;
        private INotifiable successor;

        public INotifiable this[int index]
        {
            get
            {
                if (index != 0)
                    throw new IndexOutOfRangeException(nameof(index));
                return isDummySet ? null : successor;
            }
        }

        public bool HasSuccessors => !isDummySet && successor != null;

        public bool IsAttached => isDummySet || successor != null;

        public event EventHandler Attached;
        public event EventHandler Detached;

        public IEnumerator<INotifiable> GetEnumerator()
        {
            yield return successor;
        }

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

        public void SetDummy()
        {
            if (successor == null && !isDummySet)
            {
                isDummySet = true;
                Attached?.Invoke(this, EventArgs.Empty);
            }
        }

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

        public void UnsetAll()
        {
            if (successor != null)
                Unset(successor);
            else if (isDummySet)
            {
                isDummySet = false;
                Detached?.Invoke(this, EventArgs.Empty);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
