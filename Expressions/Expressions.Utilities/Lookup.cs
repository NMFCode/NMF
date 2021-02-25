using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes an incremental lookup implementation
    /// </summary>
    /// <typeparam name="TSource">The source type of elements</typeparam>
    /// <typeparam name="TKey">The key type</typeparam>
    public class Lookup<TSource, TKey> : ILookupExpression<TSource, TKey>, IEnumerableExpression<TKey>
    {
        private readonly IEnumerableExpression<TSource> source;
        private readonly ObservingFunc<TSource, TKey> keySelector;
        private readonly Dictionary<TKey, LookupSlave> lookupCache = new Dictionary<TKey, LookupSlave>();
        private IncrementalLookup<TSource, TKey> incremental;

        /// <inheritdoc />
        public IEnumerableExpression<TKey> Keys
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a new lookup for the given source and key selector
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">A function that selects the keys of a given element</param>
        public Lookup(IEnumerableExpression<TSource> source, ObservingFunc<TSource, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
        }

        /// <inheritdoc />
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

        private class LookupSlave : IEnumerableExpression<TSource>
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

            private IncrementalLookup<TSource, TKey>.IncrementalLookupSlave Incremental
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
                incremental = new IncrementalLookup<TSource, TKey>(source.AsNotifiable(), keySelector);
                incremental.Successors.SetDummy();
            }
        }

        /// <inheritdoc />
        public INotifyEnumerable<TKey> AsNotifiable()
        {
            PerformIncrementalLookup();
            return incremental;
        }

        /// <inheritdoc />
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
    }
}
