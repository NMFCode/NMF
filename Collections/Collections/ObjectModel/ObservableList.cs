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
        public INotifyCollection<T> AsNotifiable()
        {
            return this.WithUpdates();
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
