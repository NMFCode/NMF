using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Linq;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableConcat<TSource> : ObservableEnumerable<TSource>
    {
        public override string ToString()
        {
            return "[Concat]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly IEnumerable<TSource> source2;
        private readonly INotifyEnumerable<TSource> observableSource2;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (observableSource2 != null)
                    yield return observableSource2;
            }
        }

        public ObservableConcat(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source2 == null) throw new ArgumentNullException(nameof(source2));

            this.source = source;
            this.source2 = source2;
            this.observableSource2 = source2.WithUpdates(false);
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Concat(source, source2).GetEnumerator();
        }
        
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TSource>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            var moved = notification.MovedItems;

            foreach (var change in sources.OfType<ICollectionChangedNotificationResult>())
            {
                if (change.IsReset)
                {
                    OnCleared();
                    notification.TurnIntoReset();
                    return notification;
                }

                ProcessChanges(notification, added, removed, moved, change);
            }

            RaiseEvents(added, removed, moved, notification.OldItemsStartIndex, notification.NewItemsStartIndex);
            return notification;
        }

        private void ProcessChanges(CollectionChangedNotificationResult<TSource> notification, List<TSource> added, List<TSource> removed, List<TSource> moved, ICollectionChangedNotificationResult change)
        {
            var offset = change.Source == source ? 0 : SL.Count(source);
            if (change.AddedItems != null)
            {
                added.AddRange(SL.Cast<TSource>(change.AddedItems));
                if (change.NewItemsStartIndex != -1)
                {
                    notification.UpdateNewStartIndex(offset + change.NewItemsStartIndex);
                }
            }
            if (change.RemovedItems != null)
            {
                removed.AddRange(SL.Cast<TSource>(change.RemovedItems));
                if (change.OldItemsStartIndex != -1)
                {
                    notification.UpdateOldStartIndex(offset + change.OldItemsStartIndex);
                }
            }
            if (change.MovedItems != null)
            {
                moved.AddRange(SL.Cast<TSource>(change.MovedItems));
                if (change.NewItemsStartIndex != -1)
                {
                    notification.UpdateNewStartIndex(offset + change.NewItemsStartIndex);
                }
                if (change.OldItemsStartIndex != -1)
                {
                    notification.UpdateOldStartIndex(offset + change.OldItemsStartIndex);
                }
            }
        }

        public override void RequireOrder(bool isOrderRequired)
        {
            source.RequireOrder(isOrderRequired);
            observableSource2?.RequireOrder(isOrderRequired);
        }

        public override bool IsOrdered => source.IsOrdered && (observableSource2 == null || observableSource2.IsOrdered);
    }
}
