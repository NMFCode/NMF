using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NMF.Synchronizations
{
    internal class CollectionSynchronizationJob<TLeft, TRight, TValue> : ISynchronizationJob<TLeft, TRight>, ISyncer<TLeft, TRight>,
        IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue>,
        IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue>,
        IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue>,
        IInconsistencyDescriptor<object, object, TValue, TValue>
    {
        private readonly Func<TLeft, ICollectionExpression<TValue>> leftFunc;
        private readonly Func<TRight, ICollectionExpression<TValue>> rightFunc;

        private readonly bool isEarly;

        private Func<TLeft, TRight, TValue, TValue, string> leftDescriptor;
        private Func<TLeft, TRight, TValue, TValue, string> rightDescriptor;

        public CollectionSynchronizationJob( Func<TLeft, ICollectionExpression<TValue>> leftSelector, Func<TRight, ICollectionExpression<TValue>> rightSelector, bool isEarly )
        {
            if(leftSelector == null) throw new ArgumentNullException( nameof(leftSelector));
            if(rightSelector == null) throw new ArgumentNullException( nameof(rightSelector));

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


        private ICollection<TValue> GetLefts( TLeft left, ISynchronizationContext context )
        {
            var lefts = leftFunc( left );
            if (context.ChangePropagation == ChangePropagationMode.TwoWay || (context.ChangePropagation == ChangePropagationMode.OneWay && context.Direction.IsLeftToRight()))
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TValue> GetRights( TRight right, ISynchronizationContext context )
        {
            var rights = rightFunc( right );
            if(context.ChangePropagation == ChangePropagationMode.TwoWay || (context.ChangePropagation == ChangePropagationMode.OneWay && context.Direction.IsRightToLeft()))
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
            return Sync(computation.Input, computation.Opposite.Input, context);
        }

        private IDisposable RegisterLeftChangePropagationHooks(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            switch(context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if(lefts is INotifyCollectionChanged)
                    {
                        return new LeftToRightHook(left, right, lefts, rights, context, this );
                    }
                    return null;
                case ChangePropagationMode.TwoWay:
                    return RegisterTwoWayChangePropagation(left, right, lefts, rights, context );
                default:
                    throw new NotSupportedException($"The change propagation mode {context.ChangePropagation} is not supported");
            }
        }

        private IDisposable RegisterRightChangePropagationHooks(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            switch(context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if(rights is INotifyCollectionChanged)
                    {
                        return new RightToLeftHook(left, right, lefts, rights, context, this );
                    }
                    return null;
                case ChangePropagationMode.TwoWay:
                    return RegisterTwoWayChangePropagation(left, right, lefts, rights, context );
                default:
                    throw new NotSupportedException( $"The change propagation mode {context.ChangePropagation} is not supported" );
            }
        }

        private IDisposable RegisterTwoWayChangePropagation(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context )
        {
            if(context.ChangePropagation == ChangePropagationMode.TwoWay)
            {
                var leftNotifier = lefts is INotifyCollectionChanged;
                var rightNotifier = rights is INotifyCollectionChanged;
                if(leftNotifier)
                {
                    if(rightNotifier)
                    {
                        return new BidirectionalHook(left, right, lefts, rights, context, this );
                    }
                    else
                    {
                        return new LeftToRightHook(left, right, lefts, rights, context, this );
                    }
                }
                else if(rightNotifier)
                {
                    return new RightToLeftHook(left, right, lefts, rights, context, this );
                }
            }
            return null;
        }

        public IDisposable Sync(TLeft left, TRight right, ISynchronizationContext context)
        {
            var lefts = GetLefts(left, context);
            var rights = GetRights(right, context);

            if (context.Direction.IsLeftToRight())
            {
                CollectionUtils<TValue>.SynchronizeCollectionsLeftToRight(left, right, lefts, rights, context, this);
                return RegisterLeftChangePropagationHooks(left, right, lefts, rights, context);
            }
            else
            {
                CollectionUtils<TValue>.SynchronizeCollectionsRightToLeft(left, right, lefts, rights, context, this);
                return RegisterRightChangePropagationHooks(left, right, lefts, rights, context);
            }
        }

        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> DescribeLeftChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            leftDescriptor = descriptor;
            return this;
        }

        public IInconsistencyDescriptorSyntax<TLeft, TRight, TValue, TValue> DescribeRightChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            rightDescriptor = descriptor;
            return this;
        }

        public string DescribeLeft(object left, object right, TValue depLeft, TValue depRight)
        {
            if (leftDescriptor != null && left is TLeft leftElement && right is TRight rightElement)
            {
                return leftDescriptor(leftElement, rightElement, depLeft, depRight);
            }
            if (depLeft == null)
            {
                return $"Add {depRight} to {left}";
            }
            else
            {
                return $"Remove {depLeft} (missing in {right})";
            }
        }

        public string DescribeRight(object left, object right, TValue depLeft, TValue depRight)
        {
            if (rightDescriptor != null && left is TLeft leftElement && right is TRight rightElement)
            {
                return rightDescriptor(leftElement,rightElement, depLeft , depRight);
            }
            if (depRight == null)
            {
                return $"Add {depLeft} to {right}";
            }
            else
            {
                return $"Remove {depRight} (missing in {left})";
            }
        }

        IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue>.DescribeLeftChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            leftDescriptor = descriptor;
            return this;
        }

        IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue>.DescribeRightChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            rightDescriptor = descriptor;
            return this;
        }

        private abstract class NotificationHook : IDisposable
        {
            public ICollection<TValue> Lefts { get; }
            public ICollection<TValue> Rights { get; }
            public ISynchronizationContext Context { get; }

            public TLeft Left { get; }

            public TRight Right { get; }

            public IInconsistencyDescriptor<object, object, TValue, TValue> Descriptor { get; }

            public NotificationHook(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
                Left = left;
                Right = right;
                Descriptor = descriptor;
            }

            public abstract void Dispose();
        }

        private class LeftToRightHook : NotificationHook
        {
            public LeftToRightHook(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor)
                : base( left, right, lefts, rights, context, descriptor )
            {
                if(lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                CollectionUtils<TValue>.ProcessLeftChangesForRights( Left, Right, Lefts, Rights, Context, e, Descriptor );
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
            public RightToLeftHook(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor )
                : base(left, right, lefts, rights, context, descriptor )
            {
                if(rights is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += RightsChanged;
                }
            }

            private void RightsChanged( object sender, NotifyCollectionChangedEventArgs e )
            {
                CollectionUtils<TValue>.ProcessRightChangesForLefts(Left, Right, Lefts, Rights, Context, e, Descriptor );
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

            public BidirectionalHook(TLeft left, TRight right, ICollection<TValue> lefts, ICollection<TValue> rights, ISynchronizationContext context, IInconsistencyDescriptor<object, object, TValue, TValue> descriptor )
                : base(left, right, lefts, rights, context, descriptor )
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
                        CollectionUtils<TValue>.ProcessLeftChangesForRights(Left, Right, Lefts, Rights, Context, e, Descriptor);
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
                        CollectionUtils<TValue>.ProcessRightChangesForLefts(Left, Right, Lefts, Rights, Context, e, Descriptor);
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
