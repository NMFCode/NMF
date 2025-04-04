﻿using System;
using System.Collections.Generic;

namespace NMF.Expressions.Linq.Linq
{
    internal class ObservableSimpleSelect<T, TResult> : ObservableEnumerable<TResult>
    {
        private readonly INotifyEnumerable<T> _source;
        private readonly Func<T, TResult> _resultSelector;

        public ObservableSimpleSelect(INotifyEnumerable<T> source, Func<T, TResult> resultSelector)
        {
            _source = source;
            _resultSelector = resultSelector;
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return _source;
            }
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            return System.Linq.Enumerable.Select( _source, _resultSelector ).GetEnumerator();
        }

        public override INotificationResult Notify( IList<INotificationResult> sources )
        {
            var notification = CollectionChangedNotificationResult<TResult>.Create( this );
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            var moved = notification.MovedItems;

            var newStartIndex = int.MaxValue;
            var oldStartIndex = int.MaxValue;

            foreach(var change in sources)
            {
                if (change is ICollectionChangedNotificationResult<T> actualChange)
                {
                    if (actualChange.IsReset)
                    {
                        notification.TurnIntoReset();
                        OnCleared();
                        return notification;
                    }
                    NotifyCore(added, removed, moved, ref newStartIndex, ref oldStartIndex, actualChange);
                }
            }
            if (added.Count > 0 || removed.Count > 0)
            {
                RaiseEvents( added, removed, moved, oldStartIndex, newStartIndex );
            }
            return UnchangedNotificationResult.Instance;
        }

        private void NotifyCore(List<TResult> added, List<TResult> removed, List<TResult> moved, ref int newStartIndex, ref int oldStartIndex, ICollectionChangedNotificationResult<T> actualChange)
        {
            if (actualChange.RemovedItems != null && actualChange.RemovedItems.Count > 0)
            {
                foreach (var item in actualChange.RemovedItems)
                {
                    ProcessRemovedItem(added, removed, item);
                }
            }
            if (actualChange.AddedItems != null && actualChange.AddedItems.Count > 0)
            {
                foreach (var item in actualChange.AddedItems)
                {
                    ProcessAddedItem(added, removed, item);
                }
            }
            if (actualChange.MovedItems != null && actualChange.MovedItems.Count > 0)
            {
                foreach (var item in actualChange.MovedItems)
                {
                    moved.Add(_resultSelector(item));
                }
            }
            newStartIndex = Math.Min(newStartIndex, actualChange.NewItemsStartIndex);
            oldStartIndex = Math.Min(oldStartIndex, actualChange.OldItemsStartIndex);
        }

        private void ProcessAddedItem(List<TResult> added, List<TResult> removed, T item)
        {
            var actualItem = _resultSelector(item);
            var indexOfRemove = removed.Count > 0 ? removed.IndexOf(actualItem) : -1;
            if (indexOfRemove != -1)
            {
                added.Add(actualItem);
            }
            else
            {
                removed.RemoveAt(indexOfRemove);
            }
        }

        private void ProcessRemovedItem(List<TResult> added, List<TResult> removed, T item)
        {
            var actualItem = _resultSelector(item);
            var indexOfRemove = added.Count > 0 ? added.IndexOf(actualItem) : -1;
            if (indexOfRemove != -1)
            {
                removed.Add(actualItem);
            }
            else
            {
                added.RemoveAt(indexOfRemove);
            }
        }
    }
}
