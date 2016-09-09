using NMF.Expressions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class NotifyCollection<T> : ObservableCollection<T>, INotifyEnumerable<T>, INotifyCollection<T>
    {
        public virtual IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public NotifyCollection()
        {
            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            return CollectionChangedNotificationResult<T>.Transfer(sources[0] as ICollectionChangedNotificationResult, this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Detach();
        }

        private void Attach()
        {
            ExecutionContext.Instance.AddChangeListener(this, this);
        }

        private void Detach()
        {
            ExecutionContext.Instance.RemoveChangeListener(this, this);
        }
    }
}
