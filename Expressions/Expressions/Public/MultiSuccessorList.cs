using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class MultiSuccessorList : ISuccessorList
    {
        private bool isDummySet = false;
        private List<INotifiable> successors = new List<INotifiable>();
        
        public INotifiable this[int index] { get { return successors[index]; } }

        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        public bool IsAttached => isDummySet || successors.Count > 0;

        public event EventHandler Attached;
        public event EventHandler Detached;

        public IEnumerator<INotifiable> GetEnumerator()
        {
            return successors.GetEnumerator();
        }

        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
                isDummySet = false;
            else
                Attached?.Invoke(this, EventArgs.Empty);
        }

        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attached?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            if (!(isDummySet = leaveDummy))
                Detached?.Invoke(this, EventArgs.Empty);
        }

        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detached?.Invoke(this, EventArgs.Empty);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
