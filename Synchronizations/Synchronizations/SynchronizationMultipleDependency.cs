using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>
        : IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight>, IInconsistencyDescriptor<object, object, object, object>
    {
        private readonly SynchronizationRule<TDepLeft, TDepRight> childRule;

        private readonly Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>> __leftGetter;
        private readonly Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>> __rightGetter;

        public SynchronizationMultipleDependency(SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>>> rightSelector)
        {
            if (childRule == null) throw new ArgumentNullException(nameof(childRule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            this.childRule = childRule;

            __leftGetter = ExpressionCompileRewriter.Compile(leftSelector);
            __rightGetter = ExpressionCompileRewriter.Compile(rightSelector);
        }

        private ICollection<TDepLeft> GetLefts(TLeft left, ITransformationContext context, bool incremental)
        {
            var lefts = __leftGetter(left, context);
            if (incremental)
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TDepRight> GetRights(TRight right, ITransformationContext context, bool incremental)
        {
            var rights = __rightGetter(right, context);
            if (incremental)
            {
                return rights.AsNotifiable();
            }
            else
            {
                return rights;
            }
        }

        public void HandleLeftToRightDependency(Computation computation)
        {
            var syncComputation = computation as SynchronizationComputation<TLeft, TRight>;
            var input = GetLefts(syncComputation.Input, computation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeLTRCollections(inp, outp, input, GetRights(outp, syncComputation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
                if (dependency != null)
                {
                    syncComputation.Dependencies.Add(dependency);
                }
            });
        }

        public void HandleRightToLeftDependency(Computation computation)
        {
            var syncComputation = computation as SynchronizationComputation<TRight, TLeft>;
            var input = GetRights(syncComputation.Input, computation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeRTLCollections(inp, outp, GetLefts(outp, syncComputation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), input, syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
                if (dependency != null)
                {
                    syncComputation.Dependencies.Add(dependency);
                }
            });
        }

        protected IDisposable SynchronizeLTRCollections(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (rights != null)
            {
                childRule.SynchronizeCollectionsLeftToRight(leftElement, rightElement, rights, lefts, context, ignoreCandidates, this);
                return RegisterLeftChangePropagationHooks(leftElement, rightElement, lefts, rights, context);
            }
            else
            {
                throw new NotSupportedException("Target collection must not be null!");
            }
        }

        private IDisposable RegisterLeftChangePropagationHooks(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.OneWay && lefts is INotifyCollectionChanged)
            {
                return new LeftToRightHook(leftElement, rightElement, lefts, rights, context, this);
            }
            return RegisterTwoWayChangePropagation(leftElement, rightElement, lefts, rights, context);
        }

        private IDisposable RegisterRightChangePropagationHooks(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.OneWay && rights is INotifyCollectionChanged)
            {
                return new RightToLeftHook(leftElement, rightElement, lefts, rights, context, this);
            }
            return RegisterTwoWayChangePropagation(leftElement, rightElement, lefts, rights, context);
        }

        private IDisposable RegisterTwoWayChangePropagation(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.TwoWay)
            {
                var leftNotifier = lefts is INotifyCollectionChanged;
                var rightNotifier = rights is INotifyCollectionChanged;
                if (leftNotifier)
                {
                    if (rightNotifier)
                    {
                        return new BidirectionalHook(leftElement, rightElement, lefts, rights, context, this);
                    }
                    else
                    {
                        return new LeftToRightHook(leftElement, rightElement, lefts, rights, context, this);
                    }
                }
                else if (rightNotifier)
                {
                    return new RightToLeftHook(leftElement, rightElement, lefts, rights, context, this);
                }
            }
            return null;
        }

        private void ProcessRightChangesForLefts(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    ProcessRightRemovals(leftElement, rightElement, lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessRightAdditions(leftElement, rightElement, lefts, rights, context, e);
                }
            }
            else
            {
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    var rightsSaved = new List<TDepRight>(rights);
                    lefts.Clear();
                    foreach (var item in rightsSaved)
                    {
                        AddCorrespondingToLefts(lefts, context, item);
                    }
                }
                else
                {
                    childRule.SynchronizeCollectionsRightToLeft(leftElement, rightElement, lefts, rights, context, false, this);
                }
            }
        }

        private void ProcessRightAdditions(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                TDepRight item = (TDepRight)e.NewItems[i];
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    AddCorrespondingToLefts(lefts, context, item);
                }
                else
                {
                    AddInconsistencyElementOnlyExistsInRight(leftElement, rightElement, lefts, rights, context,
                        context.Trace.ResolveIn(childRule.RightToLeft, item), item);
                }
            }
        }

        private void ProcessRightRemovals(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            for (int i = e.OldItems.Count - 1; i >= 0; i--)
            {
                TDepRight item = (TDepRight)e.OldItems[i];
                var left = context.Trace.ResolveIn(childRule.RightToLeft, item);
                if (left != null)
                {
                    if (context.Direction != SynchronizationDirection.CheckOnly)
                    {
                        lefts.Remove(left);
                    }
                    else
                    {
                        AddInconsistencyElementOnlyExistsInLeft(leftElement, rightElement, lefts, rights, context, left, item);
                    }
                }
            }
        }

        private void ProcessLeftChangesForRights(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    ProcessLeftRemovals(leftElement, rightElement, lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessLeftAdditions(leftElement, rightElement, lefts, rights, context, e);
                }
            }
            else
            {
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    var leftsSaved = new List<TDepLeft>(lefts);
                    rights.Clear();
                    foreach (var item in leftsSaved)
                    {
                        AddCorrespondingToRights(rights, context, item);
                    }
                }
                else
                {
                    childRule.SynchronizeCollectionsLeftToRight(leftElement, rightElement, rights, lefts, context, false, this);
                }
            }
        }

        private void ProcessLeftAdditions(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                TDepLeft item = (TDepLeft)e.NewItems[i];
                if (context.Direction != SynchronizationDirection.CheckOnly)
                {
                    AddCorrespondingToRights(rights, context, item);
                }
                else
                {
                    AddInconsistencyElementOnlyExistsInLeft(leftElement, rightElement, lefts, rights, context, item,
                        context.Trace.ResolveIn(childRule.LeftToRight, item));
                }
            }
        }

        private void ProcessLeftRemovals(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            for (int i = e.OldItems.Count - 1; i >= 0; i--)
            {
                TDepLeft item = (TDepLeft)e.OldItems[i];
                var right = context.Trace.ResolveIn(childRule.LeftToRight, item);
                if (right != null)
                {
                    if (context.Direction != SynchronizationDirection.CheckOnly)
                    {
                        rights.Remove(right);
                    }
                    else
                    {
                        AddInconsistencyElementOnlyExistsInRight(leftElement, rightElement, lefts, rights, context, item, right);
                    }
                }
            }
        }

        private void AddInconsistencyElementOnlyExistsInRight(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, TDepLeft left, TDepRight right)
        {
            // check whether the item is missing on the right hand side
            var missingRight = new MissingItemInconsistency<TDepLeft, TDepRight>(leftElement, rightElement, this, context, childRule.LeftToRight, lefts, rights, left, false);
            if (!context.Inconsistencies.Remove(missingRight))
            {
                var missingLeft = new MissingItemInconsistency<TDepRight, TDepLeft>(rightElement, leftElement, this, context, childRule.RightToLeft, rights, lefts, right, true);
                context.Inconsistencies.Add(missingLeft);
            }
        }

        private void AddInconsistencyElementOnlyExistsInLeft(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, TDepLeft left, TDepRight right)
        {
            // check whether the item is missing on the right hand side
            var missingLeft = new MissingItemInconsistency<TDepRight, TDepLeft>(rightElement, leftElement, this, context, childRule.RightToLeft, rights, lefts, right, true);
            if (!context.Inconsistencies.Remove(missingLeft))
            {
                var missingRight = new MissingItemInconsistency<TDepLeft, TDepRight>(leftElement, rightElement, this, context, childRule.LeftToRight, lefts, rights, left, false);
                context.Inconsistencies.Add(missingRight);
            }
        }

        private void AddCorrespondingToLefts(ICollection<TDepLeft> lefts, ISynchronizationContext context, TDepRight item)
        {
            var comp = context.CallTransformation(childRule.RightToLeft, new object[] { item }, null) as SynchronizationComputation<TDepRight, TDepLeft>;
            comp.DoWhenOutputIsAvailable((inp, outp) =>
            {
                lefts.Add(outp);
            });
        }

        private void AddCorrespondingToRights(ICollection<TDepRight> rights, ISynchronizationContext context, TDepLeft item)
        {
            var comp = context.CallTransformation(childRule.LeftToRight, new object[] { item }, null) as SynchronizationComputation<TDepLeft, TDepRight>;
            comp.DoWhenOutputIsAvailable((inp, outp) =>
            {
                rights.Add(outp);
            });
        }

        protected IDisposable SynchronizeRTLCollections(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (lefts != null)
            {
                childRule.SynchronizeCollectionsRightToLeft(leftElement, rightElement, lefts, rights, context, ignoreCandidates, this);
                return RegisterRightChangePropagationHooks(leftElement, rightElement, lefts, rights, context);
            }
            else
            {
                throw new NotSupportedException("Target collection must not be null!");
            }
        }

        public ITransformationRuleDependency CreateLeftToRightDependency()
        {
            return new LeftToRightDependency(this);
        }

        public ITransformationRuleDependency CreateRightToLeftDependency()
        {
            return new RightToLeftDependency(this);
        }

        private Func<TLeft, TRight, TDepLeft, TDepRight, string> _leftDescriptor;
        private Func<TLeft, TRight, TDepLeft, TDepRight, string> _rightDescriptor;

        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> DescribeLeftChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor)
        {
            _leftDescriptor = descriptor;
            return this;
        }

        public IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> DescribeRightChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor)
        {
            _rightDescriptor = descriptor;
            return this;
        }

        public string DescribeLeft(object left, object right, object depLeft, object depRight)
        {
            if (_leftDescriptor != null && left is TLeft leftElement && right is TRight rightElement && depLeft is TDepLeft depLeftEl && depRight is TDepRight depRightEl)
            {
                return _leftDescriptor(leftElement, rightElement, depLeftEl, depRightEl);
            }
            if (depRight != null)
            {
                return $"Add {depRight} to {left}";
            }
            else
            {
                return $"Remove {depLeft} (missing in {right})";
            }
        }

        public string DescribeRight(object left, object right, object depLeft, object depRight)
        {
            if (_rightDescriptor != null && left is TLeft leftElement && right is TRight rightElement && depLeft is TDepLeft depLeftEl && depRight is TDepRight depRightEl)
            {
                return _rightDescriptor(leftElement, rightElement, depLeftEl, depRightEl);
            }
            if (depLeft != null)
            {
                return $"Add {depLeft} to {right}";
            }
            else
            {
                return $"Remove {depRight} (missing in {left})";
            }
        }

        private sealed class LeftToRightDependency : OutputDependency
        {
            private readonly SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LeftToRightDependency(SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.HandleLeftToRightDependency(computation);
            }
        }

        private sealed class RightToLeftDependency : OutputDependency
        {
            private readonly SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RightToLeftDependency(SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.HandleRightToLeftDependency(computation);
            }
        }

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
        private abstract class NotificationHook : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
        {
            public ICollection<TDepLeft> Lefts { get; private set; }
            public ICollection<TDepRight> Rights { get; private set; }
            public ISynchronizationContext Context { get; private set; }
            public SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }

            public object LeftElement { get; private set; }
            public object RightElement { get; private set; }

            protected NotificationHook(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
                Parent = parent;
                LeftElement = leftElement;
                RightElement = rightElement;
            }

            public abstract void Dispose();
        }

        private sealed class LeftToRightHook : NotificationHook
        {
            public LeftToRightHook(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(leftElement, rightElement, lefts, rights, context, parent)
            {
                if(lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Parent.ProcessLeftChangesForRights(LeftElement, RightElement, Lefts, Rights, Context, e);
            }

            public override void Dispose()
            {
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }

        private sealed class RightToLeftHook : NotificationHook
        {
            public RightToLeftHook(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(leftElement, rightElement, lefts, rights, context, parent)
            {
                if(rights is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += RightsChanged;
                }
            }

            private void RightsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Parent.ProcessRightChangesForLefts(LeftElement, RightElement, Lefts, Rights, Context, e);
            }

            public override void Dispose()
            {
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= RightsChanged;
                }
            }
        }

        private sealed class BidirectionalHook : NotificationHook
        {
            private bool isProcessingChange = false;

            public BidirectionalHook(object leftElement, object rightElement, ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(leftElement, rightElement, lefts, rights, context, parent)
            {
                if (lefts is INotifyCollectionChanged leftNotifier)
                {
                    leftNotifier.CollectionChanged += LeftsChanged;
                }
                if (rights is INotifyCollectionChanged rightNotifier)
                {
                    rightNotifier.CollectionChanged += RightsChanged;
                }
            }

            private void LeftsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (!isProcessingChange)
                {
                    isProcessingChange = true;
                    try
                    {
                        Parent.ProcessLeftChangesForRights(LeftElement, RightElement, Lefts, Rights, Context, e);
                    }
                    finally
                    {
                        isProcessingChange = false;
                    }
                }
            }

            private void RightsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (!isProcessingChange)
                {
                    isProcessingChange = true;
                    try
                    {
                        Parent.ProcessRightChangesForLefts(LeftElement, RightElement, Lefts, Rights, Context, e);
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
                if (Rights is INotifyCollectionChanged rightNotifier)
                {
                    rightNotifier.CollectionChanged -= RightsChanged;
                }
            }
        }
    }
}
