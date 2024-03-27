using NMF.Expressions;
using NMF.Expressions.Linq;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes a base class for an observable list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableList<T> : ObservableCollectionExtended<T>, IListExpression<T>
    {
        private INotifyCollection<T> proxy;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[List Count={Count}]";
        }
    }
}
