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

        protected IDisposable Perform( TSource source, TTarget target, ISynchronizationContext context, bool force, bool wins )
        {
            var targets = GetTargetCollection( target, context );
            var noChangePropagation = context.ChangePropagation == ChangePropagationMode.None;
            var sources = GetSourceItems( source, context, noChangePropagation );
            CollectionUtils<TValue>.SynchronizeCollections(sources, targets, force, wins);
            return noChangePropagation ? null : RegisterChangePropagationHooks(sources, targets, context);
        }

        private IDisposable RegisterChangePropagationHooks( IEnumerable<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            if (context.ChangePropagation != ChangePropagationMode.None && lefts is INotifyCollectionChanged)
            {
                return new NotificationHook(lefts, rights, context);
            }
            return null;
        }
        private sealed class NotificationHook : IDisposable
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
                ProcessSourceChanges( Lefts, Rights, e );
            }

            private static void ProcessSourceChanges(IEnumerable<TValue> source, ICollection<TValue> targets, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action != NotifyCollectionChangedAction.Reset)
                {
                    if (e.OldItems != null)
                    {
                        ProcessSourceRemovals(targets, e);
                    }
                    if (e.NewItems != null)
                    {
                        ProcessSourceAdditions(targets, e);
                    }
                }
                else
                {
                    ProcessReset(source, targets);
                }
            }

            private static void ProcessReset(IEnumerable<TValue> source, ICollection<TValue> targets)
            {
                var leftsSaved = new List<TValue>(source);
                targets.Clear();
                foreach (var item in leftsSaved)
                {
                    targets.Add(item);
                }
            }

            private static void ProcessSourceAdditions(ICollection<TValue> targets, NotifyCollectionChangedEventArgs e)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    TValue item = (TValue)e.NewItems[i];
                    targets.Add(item);
                }
            }

            private static void ProcessSourceRemovals(ICollection<TValue> targets, NotifyCollectionChangedEventArgs e)
            {
                for (int i = e.OldItems.Count - 1; i >= 0; i--)
                {
                    TValue item = (TValue)e.OldItems[i];
                    if (item != null)
                    {
                        targets.Remove(item);
                    }
                }
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
                return Perform( computation.Input, computation.Opposite.Input, context,
                    context.Direction == SynchronizationDirection.LeftToRightForced,
                    context.Direction == SynchronizationDirection.LeftWins);
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
                return Perform( computation.Opposite.Input, computation.Input, context,
                    context.Direction == SynchronizationDirection.RightToLeftForced,
                    context.Direction == SynchronizationDirection.RightWins);
            }
            else
            {
                return null;
            }
        }
    }
}
