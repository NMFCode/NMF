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
        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();

        public virtual IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public IList<INotifiable> Successors { get { return successors; } }

        public NotifyCollection()
        {
            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
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
            ExecutionContext.Instance.AddChangeListener<T>(this, this);
        }

        private void Detach()
        {
            ExecutionContext.Instance.RemoveChangeListener(this, this);
        }
    }
}
