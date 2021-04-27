using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations
{
    internal static class CollectionUtils<TValue>
    {
        public static void SynchronizeCollectionsLeftToRight( ICollection<TValue> rights, ICollection<TValue> lefts, ISynchronizationContext context )
        {
            if(context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections( lefts, rights, context );
                return;
            }
            if(rights.IsReadOnly) throw new InvalidOperationException( "Collection is read-only!" );
            IEnumerable<TValue> rightsSaved;
            HashSet<TValue> doubles;
            if(context.Direction == SynchronizationDirection.LeftToRight)
            {
                rightsSaved = null;
                doubles = null;
            }
            else
            {
                rightsSaved = rights.ToArray();
                doubles = new HashSet<TValue>();
            }
            foreach(var item in lefts)
            {
                if(!rights.Contains( item ))
                {
                    rights.Add( item );
                }
                else if(context.Direction != SynchronizationDirection.LeftToRight)
                {
                    doubles.Add( item );
                }
            }
            if(context.Direction == SynchronizationDirection.LeftWins)
            {
                foreach(var item in rightsSaved.Except( doubles ))
                {
                    lefts.Add( item );
                }
            }
            else if(context.Direction == SynchronizationDirection.LeftToRightForced)
            {
                foreach(var item in rightsSaved.Except( doubles ))
                {
                    rights.Remove( item );
                }
            }
        }

        public static void SynchronizeCollectionsRightToLeft( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            if(context.Direction == SynchronizationDirection.CheckOnly)
            {
                MatchCollections( lefts, rights, context );
                return;
            }
            if(lefts.IsReadOnly) throw new InvalidOperationException( "Collection is read-only!" );
            IEnumerable<TValue> leftsSaved;
            HashSet<TValue> doubles;
            if(context.Direction == SynchronizationDirection.RightToLeft)
            {
                leftsSaved = null;
                doubles = null;
            }
            else
            {
                leftsSaved = lefts.ToArray();
                doubles = new HashSet<TValue>();
            }
            foreach(var item in rights)
            {
                if(!lefts.Contains( item ))
                {
                    lefts.Add( item );
                }
                else if(context.Direction != SynchronizationDirection.RightToLeft)
                {
                    doubles.Add( item );
                }
            }
            if(context.Direction == SynchronizationDirection.RightWins)
            {
                foreach(var item in leftsSaved.Except( doubles ))
                {
                    rights.Add( item );
                }
            }
            else if(context.Direction == SynchronizationDirection.RightToLeftForced)
            {
                foreach(var item in leftsSaved.Except( doubles ))
                {
                    lefts.Remove( item );
                }
            }
        }

        private static void MatchCollections( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
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

        private static void AddInconsistencyItemMissingLeft( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue item )
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( context, rights, lefts, item, true ) );
        }

        private static void AddInconsistencyItemMissingRight( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, TValue left )
        {
            context.Inconsistencies.Add( new MissingItemInconsistency<TValue>( context, lefts, rights, left, false ) );
        }

        public static void ProcessRightChangesForLefts( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e )
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    for(int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        TValue item = (TValue)e.OldItems[i];
                        if(context.Direction != SynchronizationDirection.CheckOnly)
                        {
                            lefts.Remove( item );
                        }
                        else
                        {
                            AddInconsistencyElementOnlyExistsInLeft( lefts, rights, context, item );
                        }
                    }
                }
                if(e.NewItems != null)
                {
                    for(int i = 0; i < e.NewItems.Count; i++)
                    {
                        TValue item = (TValue)e.NewItems[i];
                        if(context.Direction != SynchronizationDirection.CheckOnly)
                        {
                            lefts.Add( item );
                        }
                        else
                        {
                            AddInconsistencyElementOnlyExistsInRight( lefts, rights, context, item );
                        }
                    }
                }
            }
            else
            {
                if(context.Direction != SynchronizationDirection.CheckOnly)
                {
                    var rightsSaved = new List<TValue>( rights );
                    lefts.Clear();
                    foreach(var item in rightsSaved)
                    {
                        lefts.Add( item );
                    }
                }
                else
                {
                    SynchronizeCollectionsRightToLeft( lefts, rights, context );
                }
            }
        }

        public static void ProcessLeftChangesForRights( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e )
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    for(int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        TValue item = (TValue)e.OldItems[i];
                        if(item != null)
                        {
                            if(context.Direction != SynchronizationDirection.CheckOnly)
                            {
                                rights.Remove( item );
                            }
                            else
                            {
                                AddInconsistencyElementOnlyExistsInRight( lefts, rights, context, item );
                            }
                        }
                    }
                }
                if(e.NewItems != null)
                {
                    for(int i = 0; i < e.NewItems.Count; i++)
                    {
                        TValue item = (TValue)e.NewItems[i];
                        if(context.Direction != SynchronizationDirection.CheckOnly)
                        {
                            rights.Add( item );
                        }
                        else
                        {
                            AddInconsistencyElementOnlyExistsInLeft( lefts, rights, context, item );
                        }
                    }
                }
            }
            else
            {
                if(context.Direction != SynchronizationDirection.CheckOnly)
                {
                    var leftsSaved = new List<TValue>( lefts );
                    rights.Clear();
                    foreach(var item in leftsSaved)
                    {
                        rights.Add( item );
                    }
                }
                else
                {
                    SynchronizeCollectionsLeftToRight( rights, lefts, context );
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
