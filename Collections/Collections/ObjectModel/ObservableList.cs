using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace NMF.Collections.ObjectModel
{
    public class ObservableList<T> : ObservableCollectionExtended<T>, IListExpression<T>
    {
        private INotifyCollection<T> proxy;

        public INotifyCollection<T> AsNotifiable()
        {
            if (proxy == null) proxy = this.WithUpdates();
            return proxy;
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        public override string ToString()
        {
            return $"[List Count={Count}]";
        }
    }
}
