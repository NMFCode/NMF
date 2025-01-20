using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Synchronizations
{
    internal static class CollectionUtils<TValue>
    {
        public static void SynchronizeCollectionsLeftToRight(IEnumerable<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context)
        {
            if(context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections( lefts, rights, context );
                return;
            }
            SynchronizeCollections(lefts, rights,
                context.Direction == SynchronizationDirection.LeftToRightForced,
                context.Direction == SynchronizationDirection.LeftWins);
        }

        public static void SynchronizeCollectionsRightToLeft(ICollection<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context)
        {
            if (context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections(lefts, rights, context);
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

        private static void MatchCollections(IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context )
        {
            var rightsRemaining = new HashSet<TValue>( rights );
            foreach(var left in lefts)
            {
                if(!rightsRemaining.Remove( left ))
                {
                    AddInconsistencyItemMissingRight( lefts, rights, context, left );
                }
            }
            foreach(var item in rightsRemaining)
            {
                AddInconsistencyItemMissingLeft( lefts, rights, context, item );
            }
        }

        private static void AddInconsistencyItemMissingLeft(IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, TValue item )
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( context, rights as ICollection<TValue>, lefts as ICollection<TValue>, item, true ) );
        }

        private static void AddInconsistencyItemMissingRight(IEnumerable<TValue> lefts, IEnumerable<TValue> rights, ISynchronizationContext context, TValue left )
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( context, lefts as ICollection<TValue>, rights as ICollection<TValue>, left, false ) );
        }

        public static void ProcessRightChangesForLefts( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e )
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    ProcessRemovaldFromRight(lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessAdditionsToRights(lefts, rights, context, e);
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
                    SynchronizeCollectionsRightToLeft( lefts, rights, context );
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

        public static void ProcessLeftChangesForRights( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e )
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    ProcessRemovalFromLefts(lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessAdditionsToLefts(lefts, rights, context, e);
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
                    SynchronizeCollectionsLeftToRight(lefts, rights, context);
                }
            }
        }

        private static void ProcessAdditionsToLefts(ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                    AddInconsistencyElementOnlyExistsInLeft(lefts, rights, context, item);
                }
            }
        }

        private static void ProcessAdditionsToRights(ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                    AddInconsistencyElementOnlyExistsInRight(lefts, rights, context, item);
                }
            }
        }

        private static void ProcessRemovaldFromRight(ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                    AddInconsistencyElementOnlyExistsInLeft(lefts, rights, context, item);
                }
            }
        }

        private static void ProcessRemovalFromLefts(ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                        AddInconsistencyElementOnlyExistsInRight(lefts, rights, context, item);
                    }
                }
            }
        }

        private static void AddInconsistencyElementOnlyExistsInRight( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue right )
        {
            // check whether the item is missing on the right hand side
            var missingRight = new MissingItemInconsistency<TValue>( context, lefts, rights, right, false );
            if(!context.Inconsistencies.Remove( missingRight ))
            {
                var missingLeft = new MissingItemInconsistency<TValue>( context, rights, lefts, right, true );
                context.Inconsistencies.Add( missingLeft );
            }
        }

        private static void AddInconsistencyElementOnlyExistsInLeft( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue left )
        {
            // check whether the item is missing on the right hand side
            var missingLeft = new MissingItemInconsistency<TValue>( context, rights, lefts, left, true );
            if(!context.Inconsistencies.Remove( missingLeft ))
            {
                var missingRight = new MissingItemInconsistency<TValue>( context, lefts, rights, left, false );
                context.Inconsistencies.Add( missingRight );
            }
        }

    }
}
