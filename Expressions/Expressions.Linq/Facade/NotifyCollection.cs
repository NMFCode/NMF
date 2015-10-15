using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class NotifyCollection<T> : ObservableCollection<T>, INotifyEnumerable<T>, INotifyCollection<T>
    {
        public void Attach() { }

        public void Detach() { }

        public bool IsAttached
        {
            get { return true; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
