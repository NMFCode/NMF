using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class OneExpression<T> : IEnumerableExpression<T>, INotifyEnumerable<T>
    {
        private readonly T _item;

        public OneExpression(T item)
        {
            _item = item;
        }

        public ISuccessorList Successors => SingletonSuccessorList.Instance;

        public IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

        public ExecutionMetaData ExecutionMetaData
        {
            get;
        } = new ExecutionMetaData();

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { }
            remove { }
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return _item;
        }

        public INotificationResult Notify( IList<INotificationResult> sources )
        {
            return UnchangedNotificationResult.Instance;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
