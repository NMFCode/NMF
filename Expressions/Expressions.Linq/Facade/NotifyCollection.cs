using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class NotifyCollection<T> : ObservableCollection<T>, INotifyEnumerable<T>, INotifyCollection<T>
    {
        private ShortList<INotifiable> successors = new ShortList<INotifiable>();

        public virtual IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public IList<INotifiable> Successors { get { return successors; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new UnchangedNotificationResult(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
