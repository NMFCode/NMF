using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    internal class SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>
    {
        private readonly SynchronizationRule<TDepLeft, TDepRight> childRule;

        private readonly Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>> __leftGetter;
        private readonly Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>> __rightGetter;

        public SynchronizationMultipleDependency(SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, ITransformationContext, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ITransformationContext, ICollectionExpression<TDepRight>>> rightSelector)
        {
            if (childRule == null) throw new ArgumentNullException("childRule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

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
                var dependency = SynchronizeLTRCollections(input, GetRights(outp, syncComputation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
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
                var dependency = SynchronizeRTLCollections(GetLefts(outp, syncComputation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), input, syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
                if (dependency != null)
                {
                    syncComputation.Dependencies.Add(dependency);
                }
            });
        }

        protected IDisposable SynchronizeLTRCollections(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (rights != null)
            {
                childRule.SynchronizeCollectionsLeftToRight(rights, lefts, context, ignoreCandidates);
                return RegisterLeftChangePropagationHooks(lefts, rights, context);
            }
            else
            {
                throw new NotSupportedException("Target collection must not be null!");
            }
        }

        private IDisposable RegisterLeftChangePropagationHooks(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.OneWay && lefts is INotifyCollectionChanged)
            {
                return new LeftToRightHook(lefts, rights, context, this);
            }
            return RegisterTwoWayChangePropagation(lefts, rights, context);
        }

        private IDisposable RegisterRightChangePropagationHooks(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.OneWay && rights is INotifyCollectionChanged)
            {
                return new RightToLeftHook(lefts, rights, context, this);
            }
            return RegisterTwoWayChangePropagation(lefts, rights, context);
        }

        private IDisposable RegisterTwoWayChangePropagation(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.TwoWay)
            {
                var leftNotifier = lefts is INotifyCollectionChanged;
                var rightNotifier = rights is INotifyCollectionChanged;
                if (leftNotifier)
                {
                    if (rightNotifier)
                    {
                        return new BidirectionalHook(lefts, rights, context, this);
                    }
                    else
                    {
                        return new LeftToRightHook(lefts, rights, context, this);
                    }
                }
                else if (rightNotifier)
                {
                    return new RightToLeftHook(lefts, rights, context, this);
                }
            }
            return null;
        }

        private void ProcessRightChangesForLefts(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    ProcessRightRemovals(lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessRightAdditions(lefts, rights, context, e);
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
                    childRule.SynchronizeCollectionsRightToLeft(lefts, rights, context, false);
                }
            }
        }

        private void ProcessRightAdditions(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                    AddInconsistencyElementOnlyExistsInRight(lefts, rights, context,
                        context.Trace.ResolveIn(childRule.RightToLeft, item), item);
                }
            }
        }

        private void ProcessRightRemovals(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                        AddInconsistencyElementOnlyExistsInLeft(lefts, rights, context, left, item);
                    }
                }
            }
        }

        private void ProcessLeftChangesForRights(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    ProcessLeftRemovals(lefts, rights, context, e);
                }
                if (e.NewItems != null)
                {
                    ProcessLeftAdditions(lefts, rights, context, e);
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
                    childRule.SynchronizeCollectionsLeftToRight(rights, lefts, context, false);
                }
            }
        }

        private void ProcessLeftAdditions(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                    AddInconsistencyElementOnlyExistsInLeft(lefts, rights, context, item,
                        context.Trace.ResolveIn(childRule.LeftToRight, item));
                }
            }
        }

        private void ProcessLeftRemovals(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
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
                        AddInconsistencyElementOnlyExistsInRight(lefts, rights, context, item, right);
                    }
                }
            }
        }

        private void AddInconsistencyElementOnlyExistsInRight(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, TDepLeft left, TDepRight right)
        {
            // check whether the item is missing on the right hand side
            var missingRight = new MissingItemInconsistency<TDepLeft, TDepRight>(context, childRule.LeftToRight, lefts, rights, left, false);
            if (!context.Inconsistencies.Remove(missingRight))
            {
                var missingLeft = new MissingItemInconsistency<TDepRight, TDepLeft>(context, childRule.RightToLeft, rights, lefts, right, true);
                context.Inconsistencies.Add(missingLeft);
            }
        }

        private void AddInconsistencyElementOnlyExistsInLeft(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, TDepLeft left, TDepRight right)
        {
            // check whether the item is missing on the right hand side
            var missingLeft = new MissingItemInconsistency<TDepRight, TDepLeft>(context, childRule.RightToLeft, rights, lefts, right, true);
            if (!context.Inconsistencies.Remove(missingLeft))
            {
                var missingRight = new MissingItemInconsistency<TDepLeft, TDepRight>(context, childRule.LeftToRight, lefts, rights, left, false);
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

        protected IDisposable SynchronizeRTLCollections(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (lefts != null)
            {
                childRule.SynchronizeCollectionsRightToLeft(lefts, rights, context, ignoreCandidates);
                return RegisterRightChangePropagationHooks(lefts, rights, context);
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

            protected NotificationHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
                Parent = parent;
            }

            public abstract void Dispose();
        }

        private sealed class LeftToRightHook : NotificationHook
        {
            public LeftToRightHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
            {
                if(lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Parent.ProcessLeftChangesForRights(Lefts, Rights, Context, e);
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
            public RightToLeftHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
            {
                if(rights is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged += RightsChanged;
                }
            }

            private void RightsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Parent.ProcessRightChangesForLefts(Lefts, Rights, Context, e);
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

            public BidirectionalHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
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
                        Parent.ProcessLeftChangesForRights(Lefts, Rights, Context, e);
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
                        Parent.ProcessRightChangesForLefts(Lefts, Rights, Context, e);
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
