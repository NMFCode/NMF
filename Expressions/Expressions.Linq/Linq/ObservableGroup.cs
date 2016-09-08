using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public sealed class ObservableGroup<TKey, TItem> : ReadOnlyObservableCollection<TItem>, INotifyGrouping<TKey, TItem>, IGroupingExpression<TKey, TItem>
    {
        private TKey key;

        internal new IList<TItem> Items { get { return base.Items; } }

        public ObservableGroup(TKey key)
            : base(new ObservableCollection<TItem>())
        {
            this.key = key;
        }
        
        public TKey Key { get { return key; } }

        public ISuccessorList Successors { get; } = new DummySuccessorList();

        public IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public INotifyEnumerable<TItem> AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException();
        }

        public void Dispose() { }

        private class DummySuccessorList : ISuccessorList
        {
            INotifiable ISuccessorList.this[int index] { get { return null; } }

            bool ISuccessorList.HasSuccessors { get { return false; } }

            bool ISuccessorList.IsAttached { get { return false; } }

            event EventHandler ISuccessorList.Attached { add { } remove { } }

            event EventHandler ISuccessorList.Detached { add { } remove { } }

            IEnumerator<INotifiable> IEnumerable<INotifiable>.GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }

            void ISuccessorList.Set(INotifiable node) { }

            void ISuccessorList.SetDummy() { }

            void ISuccessorList.Unset(INotifiable node, bool leaveDummy) { }

            void ISuccessorList.UnsetAll() { }
        }
    }
}
