using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Synchronizations
{
    internal class CollectionSynchronizationJob<TLeft, TRight, TValue> : ISynchronizationJob<TLeft, TRight>
    {
        private readonly Func<TLeft, ICollectionExpression<TValue>> leftFunc;
        private readonly Func<TRight, ICollectionExpression<TValue>> rightFunc;

        private readonly bool isEarly;

        public CollectionSynchronizationJob( Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector, bool isEarly )
        {
            if(leftSelector == null) throw new ArgumentNullException( "leftSelector" );
            if(rightSelector == null) throw new ArgumentNullException( "rightSelector" );

            leftFunc = leftSelector;
            rightFunc = rightSelector;

            this.isEarly = isEarly;
        }

        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }


        private ICollection<TValue> GetLefts( SynchronizationComputation<TLeft, TRight> computation, ISynchronizationContext context )
        {
            var lefts = leftFunc( computation.Input );
            if (context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay || (context.ChangePropagation == Transformations.ChangePropagationMode.OneWay && context.Direction.IsLeftToRight()))
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TValue> GetRights( SynchronizationComputation<TLeft, TRight> computation, ISynchronizationContext context )
        {
            var rights = rightFunc( computation.Opposite.Input );
            if(context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay || (context.ChangePropagation == Transformations.ChangePropagationMode.OneWay && context.Direction.IsRightToLeft()))
            {
                return rights.AsNotifiable();
            }
            else
            {
                return rights;
            }
        }

        public IDisposable Perform( SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context )
        {
            var lefts = GetLefts( computation, context );
            var rights = GetRights( computation, context );

            if (context.Direction.IsLeftToRight())
            {
                CollectionUtils<TValue>.SynchronizeCollectionsLeftToRight( rights, lefts, context );
                return RegisterLeftChangePropagationHooks( lefts, rights, context );
            }
            else
            {
                CollectionUtils<TValue>.SynchronizeCollectionsRightToLeft( lefts, rights, context );
                return RegisterRightChangePropagationHooks( lefts, rights, context );
            }
        }

        private IDisposable RegisterLeftChangePropagationHooks( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            switch(context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if(lefts is INotifyCollectionChanged)
                    {
                        return new LeftToRightHook( lefts, rights, context );
                    }
                    return null;
                case ChangePropagationMode.TwoWay:
                    return RegisterTwoWayChangePropagation( lefts, rights, context );
                default:
                    throw new NotSupportedException($"The change propagation mode {context.ChangePropagation} is not supported");
            }
        }

        private IDisposable RegisterRightChangePropagationHooks( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            switch(context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if(rights is INotifyCollectionChanged)
                    {
                        return new RightToLeftHook( lefts, rights, context );
                    }
                    return null;
                case ChangePropagationMode.TwoWay:
                    return RegisterTwoWayChangePropagation( lefts, rights, context );
                default:
                    throw new NotSupportedException( $"The change propagation mode {context.ChangePropagation} is not supported" );
            }
        }

        private IDisposable RegisterTwoWayChangePropagation( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            if(context.ChangePropagation == ChangePropagationMode.TwoWay)
            {
                var leftNotifier = lefts is INotifyCollectionChanged;
                var rightNotifier = rights is INotifyCollectionChanged;
                if(leftNotifier)
                {
                    if(rightNotifier)
                    {
                        return new BidirectionalHook( lefts, rights, context );
                    }
                    else
                    {
                        return new LeftToRightHook( lefts, rights, context );
                    }
                }
                else if(rightNotifier)
                {
                    return new RightToLeftHook( lefts, rights, context );
                }
            }
            return null;
        }

        private abstract class NotificationHook : IDisposable
        {
            public ICollection<TValue> Lefts { get; private set; }
            public ICollection<TValue> Rights { get; private set; }
            public ISynchronizationContext Context { get; private set; }

            public NotificationHook( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
            }

            public abstract void Dispose();
        }

        private class LeftToRightHook : NotificationHook
        {
            public LeftToRightHook( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
                : base( lefts, rights, context )
            {
                if(lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                CollectionUtils<TValue>.ProcessLeftChangesForRights( Lefts, Rights, Context, e );
            }

            public override void Dispose()
            {
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }

        private class RightToLeftHook : NotificationHook
        {
            public RightToLeftHook( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
                : base( lefts, rights, context )
            {
                if(rights is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += RightsChanged;
                }
            }

            private void RightsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                CollectionUtils<TValue>.ProcessRightChangesForLefts( Lefts, Rights, Context, e );
            }

            public override void Dispose()
            {
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= RightsChanged;
                }
            }
        }

        private class BidirectionalHook : NotificationHook
        {
            private bool isProcessingChange = false;

            public BidirectionalHook( ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
                : base( lefts, rights, context )
            {
                if(lefts is INotifyCollectionChanged leftNotifier)
                {
                    leftNotifier.CollectionChanged += LeftsChanged;
                }
                if(rights is INotifyCollectionChanged rightNotifier)
                {
                    rightNotifier.CollectionChanged += RightsChanged;
                }
            }

            private void LeftsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                if(!isProcessingChange)
                {
                    isProcessingChange = true;
                    try
                    {
                        CollectionUtils<TValue>.ProcessLeftChangesForRights( Lefts, Rights, Context, e );
                    }
                    finally
                    {
                        isProcessingChange = false;
                    }
                }
            }

            private void RightsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                if(!isProcessingChange)
                {
                    isProcessingChange = true;
                    try
                    {
                        CollectionUtils<TValue>.ProcessRightChangesForLefts( Lefts, Rights, Context, e );
                    }
                    finally
                    {
                        isProcessingChange = false;
                    }
                }
            }

            public override void Dispose()
            {
                if(Lefts is INotifyCollectionChanged leftNotifier)
                {
                    leftNotifier.CollectionChanged -= LeftsChanged;
                }
                if(Rights is INotifyCollectionChanged rightNotifier)
                {
                    rightNotifier.CollectionChanged -= RightsChanged;
                }
            }
        }
    }
}
