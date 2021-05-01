using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents a node in a graph which can notify its successors
    /// and gets notified by its dependencies.
    /// </summary>
    public interface INotifiable : IDisposable
    {
        /// <summary>
        /// The nodes that will get notified by this node.
        /// </summary>
        ISuccessorList Successors { get; }

        /// <summary>
        /// Nodes that notify this node.
        /// </summary>
        IEnumerable<INotifiable> Dependencies { get; }

        /// <summary>
        /// Gets called when one of the dependencies signals a notification.
        /// </summary>
        /// <param name="sources">Contains information about what triggered this notification.</param>
        /// <returns>An object describing the changes that happened in this notification.</returns>
        INotificationResult Notify(IList<INotificationResult> sources);

        /// <summary>
        /// Used by the execution engine during incremental execution.
        /// </summary>
        ExecutionMetaData ExecutionMetaData { get; }
    }


    /// <summary>
    /// Represents a node in a graph which can notify its successors
    /// and gets notified by its dependencies.
    /// </summary>
    public abstract class Notifiable : INotifiable, IDisposable, ISuccessorList
    {
        private readonly ExecutionMetaData metadata = new ExecutionMetaData();

        /// <summary>
        /// The nodes that will get notified by this node.
        /// </summary>
        public ISuccessorList Successors => this;

        /// <summary>
        /// Nodes that notify this node.
        /// </summary>
        public abstract IEnumerable<INotifiable> Dependencies { get; }

        /// <summary>
        /// Gets called when one of the dependencies signals a notification.
        /// </summary>
        /// <param name="sources">Contains information about what triggered this notification.</param>
        /// <returns>An object describing the changes that happened in this notification.</returns>
        public abstract INotificationResult Notify(IList<INotificationResult> sources);

        /// <inheritdoc />
        public void Dispose()
        {
            Successors.UnsetAll();
        }

        /// <summary>
        /// Gets called when there is a client
        /// </summary>
        protected virtual void Attach() { }

        /// <summary>
        /// Gets called when there is no client any more
        /// </summary>
        protected virtual void Detach() { }

        /// <summary>
        /// Used by the execution engine during incremental execution.
        /// </summary>
        public ExecutionMetaData ExecutionMetaData { get { return metadata; } }

        #region SuccessorList


        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public INotifiable this[int index] { get { return successors[index]; } }

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        public int Count => successors.Count;

        public IEnumerable<INotifiable> AllSuccessors => successors;

        /// <inheritdoc />
        public event EventHandler Attached;


        /// <inheritdoc />
        public event EventHandler Detached;

        /// <inheritdoc />
        public IEnumerator<INotifiable> GetEnumerator()
        {
            return successors.GetEnumerator();
        }


        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
            {
                isDummySet = false;
            }
            else
            {
                if (successors.Count == 1)
                {
                    Attach();
                    Attached?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attach();
                Attached?.Invoke(this, EventArgs.Empty);
            }
        }


        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            if (!(isDummySet = leaveDummy))
                Detached?.Invoke(this, EventArgs.Empty);
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detached?.Invoke(this, EventArgs.Empty);
            }
        }

        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion
    }
}
