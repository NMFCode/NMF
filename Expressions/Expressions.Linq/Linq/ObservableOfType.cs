using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace NMF.Expressions.Linq
{
    internal class ObservableOfType<T> : ObservableEnumerable<T>
    {
        public INotifyEnumerable Source { get; private set; }

        public ObservableOfType(INotifyEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;

            Attach();
        }

        void SourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                OnCleared();
            }
            else
            {
                if (e.OldItems != null)
                {
                    OnRemoveItems(SL.OfType<T>(e.OldItems));
                }
                if (e.NewItems != null)
                {
                    OnAddItems(SL.OfType<T>(e.NewItems));
                }
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return SL.OfType<T>(Source).GetEnumerator();
        }

        protected override void AttachCore()
        {
            Source.Attach();

            Source.CollectionChanged += SourceCollectionChanged;
        }

        protected override void DetachCore()
        {
            Source.Detach();

            Source.CollectionChanged -= SourceCollectionChanged;
        }
    }

    internal class ObservableOfTypeCollection<TSource, T> : ObservableOfType<T>, INotifyCollection<T>
        where T : TSource
    {
        public ObservableOfTypeCollection(INotifyCollection<TSource> source)
            : base(source) { }


        void ICollection<T>.Add(T item)
        {
            var coll = Source as INotifyCollection<TSource>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            var coll = Source as INotifyCollection<TSource>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            var list = new List<T>(this);
            if (list.Count == coll.Count)
            {
                coll.Clear();
            }
            else
            {
                foreach (var item in list)
                {
                    coll.Remove(item);
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                var coll = Source as INotifyCollection<TSource>;
                return coll == null || coll.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = Source as INotifyCollection<TSource>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
