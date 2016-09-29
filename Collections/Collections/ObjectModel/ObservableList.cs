using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NMF.Expressions;

namespace NMF.Collections.ObjectModel
{
    public class ObservableList<T> : ObservableCollectionExtended<T>, INotifyEnumerable<T>, INotifyCollection<T>, IListExpression<T>, ICollectionExpression
    {
        void INotifyEnumerable.Attach() { }

        void INotifyEnumerable.Detach() { }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        public INotifyCollection<T> AsNotifiable()
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
    }
}
