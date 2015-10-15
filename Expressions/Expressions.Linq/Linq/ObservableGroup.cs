using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public class ObservableGroup<TKey, TItem> : ReadOnlyObservableCollection<TItem>, INotifyGrouping<TKey, TItem>, IGroupingExpression<TKey, TItem>
    {
        private TKey key;

        internal IList<TItem> ItemsInternal
        {
            get
            {
                return Items;
            }
        }

        public ObservableGroup(TKey key)
            : base(new ObservableCollection<TItem>())
        {
            this.key = key;
        }


        public TKey Key
        {
            get { return key; }
        }

        public void Attach() { }

        public void Detach() { }

        public bool IsAttached
        {
            get { return true; }
        }

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
