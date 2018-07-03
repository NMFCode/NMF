using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace NMF.Expressions.Linq
{
    public class Lookup<TSource, TKey> : ILookupExpression<TSource, TKey>, IEnumerableExpression<TKey>
    {
        private IEnumerableExpression<TSource> source;
        private ObservingFunc<TSource, TKey> keySelector;
        private Dictionary<TKey, LookupSlave> lookupCache = new Dictionary<TKey, LookupSlave>();
        private Incremental incremental;

        public IEnumerableExpression<TKey> Keys
        {
            get
            {
                return this;
            }
        }

        public Lookup(IEnumerableExpression<TSource> source, ObservingFunc<TSource, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
        }

        public IEnumerableExpression<TSource> this[TKey key]
        {
            get
            {
                return GetOrCreateLookup(key);
            }
        }

        private LookupSlave GetOrCreateLookup(TKey key)
        {
            LookupSlave lookup;
            if (!lookupCache.TryGetValue(key, out lookup))
            {
                lookup = new LookupSlave(this, key);
                lookupCache.Add(key, lookup);
            }
            return lookup;
        }

        private void PerformLookup()
        {
            foreach (var item in source)
            {
                var key = keySelector.Evaluate(item);
                var lookup = GetOrCreateLookup(key);
                lookup.Items.Add(item);
            }
        }

        protected class LookupSlave : IEnumerableExpression<TSource>
        {
            private Lookup<TSource, TKey> parent;
            private List<TSource> items;
            private TKey key;

            public ICollection<TSource> Items
            {
                get
                {
                    if (items == null) items = new List<TSource>();
                    return items;
                }
            }

            private IncrementalLookupBase<TSource, TKey>.IncrementalLookupSlave Incremental
            {
                get; set;
            }

            public LookupSlave(Lookup<TSource, TKey> parent, TKey key)
            {
                this.parent = parent;
                this.key = key;
            }

            public INotifyEnumerable<TSource> AsNotifiable()
            {
                if (Incremental == null)
                {
                    parent.PerformIncrementalLookup();
                    Incremental = parent.incremental.GetLookup(key);
                }
                return Incremental;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                if (items == null) parent.PerformLookup();
                return items.GetEnumerator();
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

        private void PerformIncrementalLookup()
        {
            if (incremental == null)
            {
                incremental = new Incremental(this, source.AsNotifiable(), keySelector);
                incremental.Successors.SetDummy();
            }
        }

        public INotifyEnumerable<TKey> AsNotifiable()
        {
            PerformIncrementalLookup();
            return incremental;
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return lookupCache.Keys.GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyLookup<TSource, TKey> ILookupExpression<TSource, TKey>.AsNotifiable()
        {
            PerformIncrementalLookup();
            return incremental;
        }

        protected class Incremental : IncrementalLookupBase<TSource, TKey>
        {
            private Lookup<TSource, TKey> parent;

            public Incremental(Lookup<TSource, TKey> parent, INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector) : base(source, keySelector)
            {
                this.parent = parent;
            }
        }
    }
}
