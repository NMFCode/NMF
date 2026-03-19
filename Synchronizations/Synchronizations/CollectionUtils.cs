using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Synchronizations
{
    internal static class CollectionUtils<TValue>
    {
        public static void SynchronizeCollectionsLeftToRight(object left, object right, IEnumerable<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            if(context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections( left, right, lefts, rights, context, descriptor );
                return;
            }
            SynchronizeCollections(lefts, rights,
                context.Direction == SynchronizationDirection.LeftToRightForced,
                context.Direction == SynchronizationDirection.LeftWins);
        }

        public static void SynchronizeCollectionsRightToLeft(object left, object right, ICollection<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(left, right, lefts, rights, context, descriptor);
                return;
            }
            SynchronizeCollections(rights, lefts,
                context.Direction == SynchronizationDirection.RightToLeftForced,
                context.Direction == SynchronizationDirection.RightWins);
        }

        public static void SynchronizeCollections(IEnumerable<TValue> source, ICollection<TValue> target, bool force = false, bool wins = false)
        {
            if (target == null) throw new NotSupportedException("Target collection must not be null!");
            if (target.IsReadOnly) throw new InvalidOperationException("Collection is read-only!");
            IEnumerable<TValue> targetsSaved;
            HashSet<TValue> doubles;
            if (!force && !wins)
            {
                targetsSaved = null;
                doubles = null;
            }
            else
            {
                targetsSaved = target.ToArray();
                doubles = new HashSet<TValue>();
            }
            foreach (var item in source)
            {
                if (!target.Contains(item))
                {
                    target.Add(item);
                }
                else if (force || wins)
                {
                    doubles.Add(item);
                }
            }
            if (wins && source is ICollection<TValue> sourceCollection)
            {
                AddItemsMissingInSource(sourceCollection, targetsSaved, doubles);
            }
            else if (force)
            {
                RemoveItemsNotPresentInSource(target, targetsSaved, doubles);
            }
        }

        private static void RemoveItemsNotPresentInSource(ICollection<TValue> target, IEnumerable<TValue> targetsSaved, HashSet<TValue> doubles)
        {
            foreach (var item in targetsSaved.Except(doubles))
            {
                target.Remove(item);
            }
        }

        private static void AddItemsMissingInSource(ICollection<TValue> source, IEnumerable<TValue> targetsSaved, HashSet<TValue> doubles)
        {
            foreach (var item in targetsSaved.Except(doubles))
            {
                source.Add(item);
            }
        }

        public static void MatchCollections(object leftElement, object right, IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor )
        {
            var rightsRemaining = new HashSet<TValue>( rights );
            foreach(var left in lefts)
            {
                if(!rightsRemaining.Remove( left ))
                {
                    AddInconsistencyItemMissingRight( leftElement, right, lefts, rights, context, left, descriptor );
                }
            }
            foreach(var item in rightsRemaining)
            {
                AddInconsistencyItemMissingLeft(leftElement, right, lefts, rights, context, item, descriptor );
            }
        }

        private static void AddInconsistencyItemMissingLeft(object left, object right, IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, TValue item, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( right, left, descriptor, context, rights as ICollection<TValue>, lefts as ICollection<TValue>, item, true ) );
        }

        private static void AddInconsistencyItemMissingRight(object left, object right, IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, TValue item, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( left, right, descriptor, context, lefts as ICollection<TValue>, rights as ICollection<TValue>, item, false ) );
        }

        public static void ProcessRightChangesForLefts( object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    ProcessRemovaldFromRight(left, right, lefts, rights, context, e, descriptor);
                }
                if (e.NewItems != null)
                {
                    ProcessAdditionsToRights(left, right, lefts, rights, context, e, descriptor);
                }
            }
            else
            {
                if(context.Direction != SynchronizationDirection.CheckOnly)
                {
                    ResetCollection(rights, lefts);
                }
                else
                {
                    SynchronizeCollectionsRightToLeft( left, right, lefts, rights, context, descriptor );
                }
            }
        }

        private static void ResetCollection(ICollection<TValue> source, ICollection<TValue> target)
        {
            var rightsSaved = new List<TValue>(source);
            target.Clear();
            foreach (var item in rightsSaved)
            {
                target.Add(item);
            }
        }

        public static void ProcessLeftChangesForRights(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    ProcessRemovalFromLefts(left, right, lefts, rights, context, e, descriptor);
                }
                if (e.NewItems != null)
                {
                    ProcessAdditionsToLefts(left, right, lefts, rights, context, e, descriptor);
                }
            }
            else
            {
                if(context.Direction != SynchronizationDirection.CheckOnly)
                {
                    ResetCollection(lefts, rights);
                }
                else
                {
                    SynchronizeCollectionsLeftToRight(left, right, lefts, rights, context, descriptor);
                }
            }
        }

        private static void ProcessAdditionsToLefts(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                TValue item = (TValue)e.NewItems[i];
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    rights.Add(item);
                }
                else
                {
                    AddInconsistencyElementOnlyExistsInLeft(left, right, lefts, rights, context, item, descriptor);
                }
            }
        }

        private static void ProcessAdditionsToRights(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                TValue item = (TValue)e.NewItems[i];
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    lefts.Add(item);
                }
                else
                {
                    AddInconsistencyElementOnlyExistsInRight(left, right, lefts, rights, context, item, descriptor);
                }
            }
        }

        private static void ProcessRemovaldFromRight(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            for (int i = e.OldItems.Count - 1; i >= 0; i--)
            {
                TValue item = (TValue)e.OldItems[i];
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    lefts.Remove(item);
                }
                else
                {
                    AddInconsistencyElementOnlyExistsInLeft(left, right, lefts, rights, context, item, descriptor);
                }
            }
        }

        private static void ProcessRemovalFromLefts(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            for (int i = e.OldItems.Count - 1; i >= 0; i--)
            {
                TValue item = (TValue)e.OldItems[i];
                if (item != null)
                {
                    if (context.Direction != SynchronizationDirection.CheckOnly)
                    {
                        rights.Remove(item);
                    }
                    else
                    {
                        AddInconsistencyElementOnlyExistsInRight(left, right, lefts, rights, context, item, descriptor);
                    }
                }
            }
        }

        private static void AddInconsistencyElementOnlyExistsInRight( object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue item, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            // check whether the item is missing on the right hand side
            var missingRight = new MissingItemInconsistency<TValue>( left, right, descriptor, context, lefts, rights, item, false );
            if(!context.Inconsistencies.Remove( missingRight ))
            {
                var missingLeft = new MissingItemInconsistency<TValue>( left, right, descriptor, context, rights, lefts, item, true );
                context.Inconsistencies.Add( missingLeft );
            }
        }

        private static void AddInconsistencyElementOnlyExistsInLeft(object left, object right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue item, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
        {
            // check whether the item is missing on the right hand side
            var missingLeft = new MissingItemInconsistency<TValue>( left, right, descriptor, context, rights, lefts, item, true );
            if(!context.Inconsistencies.Remove( missingLeft ))
            {
                var missingRight = new MissingItemInconsistency<TValue>( left, right, descriptor, context, lefts, rights, item, false );
                context.Inconsistencies.Add( missingRight );
            }
        }

    }
}
