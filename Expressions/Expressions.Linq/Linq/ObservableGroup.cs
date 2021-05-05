using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableGroup<TKey, TItem> : NotifyCollection<TItem>, INotifyGrouping<TKey, TItem>, IGroupingExpression<TKey, TItem>
    {
        private readonly TKey key;

        public ObservableGroup(TKey key)
        {
            this.key = key;
        }
        
        public TKey Key { get { return key; } }

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public INotifyEnumerable<TItem> AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
