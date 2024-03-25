using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a successor list for a constant
    /// </summary>
    public class SingletonSuccessorList : ISuccessorList
    {
        /// <summary>
        /// The static instance
        /// </summary>
        public static SingletonSuccessorList Instance { get; } = new SingletonSuccessorList();

        private SingletonSuccessorList() { }

        /// <inheritdoc />
        public bool HasSuccessors => true;

        /// <inheritdoc />
        public bool IsAttached => true;

        /// <inheritdoc />
        public int Count => 0;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => Enumerable.Empty<INotifiable>();

        /// <inheritdoc />
        public void Set(INotifiable node) { }

        /// <inheritdoc />
        public void SetDummy() { }

        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false) { }

        /// <inheritdoc />
        public void UnsetAll() { }

        /// <inheritdoc />
        public INotifiable GetSuccessor(int index)
        {
            throw new NotSupportedException();
        }
    }
}
