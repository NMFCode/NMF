using NMF.Expressions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a collection that listens for updates
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class NotifyCollection<T> : ObservableCollection<T>, INotifyEnumerable<T>, INotifyCollection<T>, ICollectionExpression<T>, ISuccessorList
    {
        private readonly CollectionChangeListener<T> listener;

        /// <inheritdoc />
        public virtual IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        /// <inheritdoc />
        public ISuccessorList Successors => this;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NotifyCollection()
        {
            listener = new CollectionChangeListener<T>(this);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Successors.UnsetAll();
        }

        /// <inheritdoc />
        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            return CollectionChangedNotificationResult<T>.Transfer(sources[0] as ICollectionChangedNotificationResult, this);
        }

        private void Attach()
        {
            listener.Subscribe(this);
        }

        private void Detach()
        {
            listener.Unsubscribe();
        }

        INotifyCollection<T> ICollectionExpression<T>.AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return this;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[NotifyCollection Count={Count}]";
        }


        #region SuccessorList

        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        int ISuccessorList.Count => successors.Count;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => successors;


        /// <inheritdoc />
        public void Set( INotifiable node )
        {
            if(node == null)
                throw new ArgumentNullException( nameof( node ) );

            successors.Add( node );
            if(isDummySet)
            {
                isDummySet = false;
            }
            else
            {
                if(successors.Count == 1)
                {
                    Attach();
                }
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if(successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attach();
            }
        }


        /// <inheritdoc />
        public void Unset( INotifiable node, bool leaveDummy = false )
        {
            if(node == null)
                throw new ArgumentNullException( nameof( node ) );

            if(!successors.Remove( node ))
            {
                throw new InvalidOperationException( "The specified node is not registered as the successor." );
            }
            if(!(isDummySet = leaveDummy))
            {
                Detach();
            }
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if(IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detach();
            }
        }

        /// <inheritdoc />
        public INotifiable GetSuccessor( int index )
        {
            return successors[index];
        }

        #endregion
    }
}
