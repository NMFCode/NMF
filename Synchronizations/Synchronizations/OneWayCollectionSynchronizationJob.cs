using NMF.Expressions;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations
{
    internal class OneWayCollectionSynchronizationJob<TSource, TTarget, TValue>
    {
        private readonly Func<TSource, ITransformationContext, IEnumerableExpression<TValue>> sourceGetter;
        private readonly Func<TTarget, ITransformationContext, ICollection<TValue>> targetGetter;

        private readonly bool isEarly;

        public OneWayCollectionSynchronizationJob( Func<TSource, ITransformationContext, IEnumerableExpression<TValue>> sourceGetter, Func<TTarget, ITransformationContext, ICollection<TValue>> targetGetter, bool isEarly )
        {
            this.sourceGetter = sourceGetter;
            this.targetGetter = targetGetter;

            this.isEarly = isEarly;
        }

        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }

        private IEnumerable<TValue> GetSourceItems( TSource source, ITransformationContext context, bool incremental )
        {
            var lefts = sourceGetter( source, context );
            if(incremental)
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TValue> GetTargetCollection( TTarget right, ITransformationContext context )
        {
            return targetGetter( right, context );
        }

        protected IDisposable Perform( TSource source, TTarget target, ISynchronizationContext context )
        {
            var targets = GetTargetCollection( target, context );
            switch(context.ChangePropagation)
            {
                case Transformations.ChangePropagationMode.None:
                    SynchronizeCollections( GetSourceItems( source, context, false ), targets, context );
                    return null;
                case Transformations.ChangePropagationMode.OneWay:
                case Transformations.ChangePropagationMode.TwoWay:
                    var sources = GetSourceItems( source, context, true );
                    SynchronizeCollections( sources, targets, context );
                    return RegisterChangePropagationHooks( sources, targets, context );
                default:
                    throw new InvalidOperationException( "Change propagation mode is not supported" );
            }
        }

        protected virtual void SynchronizeCollections( IEnumerable<TValue> source, ICollection<TValue> targets, ISynchronizationContext context )
        {
            if(targets != null)
            {
                if(targets.IsReadOnly) throw new InvalidOperationException( "Collection is read-only!" );
                IEnumerable<TValue> rightsSaved = targets;
                if(source == null || (context.Direction == SynchronizationDirection.LeftToRightForced || context.Direction == SynchronizationDirection.RightToLeftForced))
                {
                    rightsSaved = targets.ToArray();
                    targets.Clear();
                }
                var doubles = new HashSet<TValue>();
                foreach(var item in source)
                {
                    if(!targets.Contains( item ))
                    {
                        targets.Add( item );
                    }
                    else
                    {
                        doubles.Add( item );
                    }
                }
            }
            else
            {
                throw new NotSupportedException( "Target collection must not be null!" );
            }
        }

        private IDisposable RegisterChangePropagationHooks( IEnumerable<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            if(context.ChangePropagation != ChangePropagationMode.None)
            {
                if(lefts is INotifyCollectionChanged)
                {
                    return new NotificationHook( lefts, rights, context );
                }
            }
            return null;
        }

        private static void ProcessSourceChanges( IEnumerable<TValue> source, ICollection<TValue> targets, ISynchronizationContext context, NotifyCollectionChangedEventArgs e )
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
                            targets.Remove( item );
                        }
                    }
                }
                if(e.NewItems != null)
                {
                    for(int i = 0; i < e.NewItems.Count; i++)
                    {
                        TValue item = (TValue)e.NewItems[i];
                        targets.Add( item );
                    }
                }
            }
            else
            {
                var leftsSaved = new List<TValue>( source );
                targets.Clear();
                foreach(var item in leftsSaved)
                {
                    targets.Add( item );
                }
            }
        }

        private class NotificationHook : IDisposable
        {
            public IEnumerable<TValue> Lefts { get; private set; }
            public ICollection<TValue> Rights { get; private set; }
            public ISynchronizationContext Context { get; private set; }

            public NotificationHook( IEnumerable<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;

                if(lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                ProcessSourceChanges( Lefts, Rights, Context, e );
            }

            public void Dispose()
            {
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }
    }

    internal class LeftToRightCollectionSynchronizationJob<TLeft, TRight, TValue> : OneWayCollectionSynchronizationJob<TLeft, TRight, TValue>, ISynchronizationJob<TLeft, TRight>
    {
        public LeftToRightCollectionSynchronizationJob( Func<TLeft, ITransformationContext, IEnumerableExpression<TValue>> sourceGetter, Func<TRight, ITransformationContext, ICollection<TValue>> targetGetter, bool isEarly ) : base( sourceGetter, targetGetter, isEarly )
        {
        }

        public IDisposable Perform( SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context )
        {
            if(direction.IsLeftToRight())
            {
                return Perform( computation.Input, computation.Opposite.Input, context );
            }
            else
            {
                return null;
            }
        }
    }

    internal class RightToLeftCollectionSynchronizationJob<TLeft, TRight, TValue> : OneWayCollectionSynchronizationJob<TRight, TLeft, TValue>, ISynchronizationJob<TLeft, TRight>
    {
        public RightToLeftCollectionSynchronizationJob( Func<TRight, ITransformationContext, IEnumerableExpression<TValue>> sourceGetter, Func<TLeft, ITransformationContext, ICollection<TValue>> targetGetter, bool isEarly ) : base( sourceGetter, targetGetter, isEarly )
        {
        }

        public IDisposable Perform( SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context )
        {
            if(direction.IsRightToLeft())
            {
                return Perform( computation.Opposite.Input, computation.Input, context );
            }
            else
            {
                return null;
            }
        }
    }
}
