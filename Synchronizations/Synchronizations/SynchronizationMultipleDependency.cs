using NMF.Expressions;
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
    public class SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight>
        where TLeft : class
        where TRight : class
        where TDepLeft : class
        where TDepRight : class
    {
        private SynchronizationRule<TLeft, TRight> parentRule;
        private SynchronizationRule<TDepLeft, TDepRight> childRule;

        private Func<TLeft, ICollectionExpression<TDepLeft>> __leftGetter;
        private Func<TRight, ICollectionExpression<TDepRight>> __rightGetter;

        public SynchronizationMultipleDependency(SynchronizationRule<TLeft, TRight> parentRule, SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, ICollectionExpression<TDepLeft>>> leftSelector, Expression<Func<TRight, ICollectionExpression<TDepRight>>> rightSelector)
        {
            if (parentRule == null) throw new ArgumentNullException("parentRule");
            if (childRule == null) throw new ArgumentNullException("childRule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            this.parentRule = parentRule;
            this.childRule = childRule;

            this.__leftGetter = ExpressionCompileRewriter.Compile(leftSelector);
            this.__rightGetter = ExpressionCompileRewriter.Compile(rightSelector);
        }

        private ICollection<TDepLeft> GetLefts(TLeft left, bool incremental)
        {
            var lefts = __leftGetter(left);
            if (incremental)
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TDepRight> GetRights(TRight right, bool incremental)
        {
            var rights = __rightGetter(right);
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
            var input = GetLefts(syncComputation.Input, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeLTRCollections(input, GetRights(outp, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
                if (dependency != null)
                {
                    syncComputation.Dependencies.Add(dependency);
                }
            });
        }

        public void HandleRightToLeftDependency(Computation computation)
        {
            var syncComputation = computation as SynchronizationComputation<TRight, TLeft>;
            var input = GetRights(syncComputation.Input, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeRTLCollections(GetLefts(outp, syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.TwoWay), input, syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
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
            if (context.ChangePropagation == ChangePropagationMode.OneWay)
            {
                if (lefts is INotifyCollectionChanged)
                {
                    return new LeftToRightHook(lefts, rights, context, this);
                }
            }
            return RegisterTwoWayChangePropagation(lefts, rights, context);
        }

        private IDisposable RegisterRightChangePropagationHooks(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation == ChangePropagationMode.OneWay)
            {
                if (rights is INotifyCollectionChanged)
                {
                    return new RightToLeftHook(lefts, rights, context, this);
                }
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
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        TDepRight item = (TDepRight)e.OldItems[i];
                        var left = context.Trace.ResolveIn(childRule.RightToLeft, item);
                        if (left != null)
                        {
                            lefts.Remove(left);
                        }
                    }
                }
                if (e.NewItems != null)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        TDepRight item = (TDepRight)e.NewItems[i];
                        AddCorrespondingToLefts(lefts, context, item);
                    }
                }
            }
            else
            {
                var rightsSaved = new List<TDepRight>(rights);
                lefts.Clear();
                foreach (var item in rightsSaved)
                {
                    AddCorrespondingToLefts(lefts, context, item);
                }
            }
        }

        private void ProcessLeftChangesForRights(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        TDepLeft item = (TDepLeft)e.OldItems[i];
                        var right = context.Trace.ResolveIn(childRule.LeftToRight, item);
                        if (right != null)
                        {
                            rights.Remove(right);
                        }
                    }
                }
                if (e.NewItems != null)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        TDepLeft item = (TDepLeft)e.NewItems[i];
                        AddCorrespondingToRights(rights, context, item);
                    }
                }
            }
            else
            {
                var leftsSaved = new List<TDepLeft>(lefts);
                rights.Clear();
                foreach (var item in leftsSaved)
                {
                    AddCorrespondingToRights(rights, context, item);
                }
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
            return new LTRDependency(this);
        }

        public ITransformationRuleDependency CreateRightToLeftDependency()
        {
            return new RTLDependency(this);
        }

        private class LTRDependency : OutputDependency
        {
            private SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LTRDependency(SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.HandleLeftToRightDependency(computation);
            }
        }

        private class RTLDependency : OutputDependency
        {
            private SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RTLDependency(SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.HandleRightToLeftDependency(computation);
            }
        }

        private abstract class NotificationHook : IDisposable
        {
            public ICollection<TDepLeft> Lefts { get; private set; }
            public ICollection<TDepRight> Rights { get; private set; }
            public ISynchronizationContext Context { get; private set; }
            public SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }

            public NotificationHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
                Parent = parent;
            }

            public abstract void Dispose();
        }

        private class LeftToRightHook : NotificationHook
        {
            public LeftToRightHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
            {
                var notifier = lefts as INotifyCollectionChanged;
                if (notifier != null)
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
                var notifier = Lefts as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }

        private class RightToLeftHook : NotificationHook
        {
            public RightToLeftHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
            {
                var notifier = rights as INotifyCollectionChanged;
                if (notifier != null)
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
                var notifier = Lefts as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= RightsChanged;
                }
            }
        }

        private class BidirectionalHook : NotificationHook
        {
            private bool isProcessingChange = false;

            public BidirectionalHook(ICollection<TDepLeft> lefts, ICollection<TDepRight> rights, ISynchronizationContext context, SynchronizationMultipleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
                : base(lefts, rights, context, parent)
            {
                var leftNotifier = lefts as INotifyCollectionChanged;
                var rightNotifier = rights as INotifyCollectionChanged;
                if (leftNotifier != null)
                {
                    leftNotifier.CollectionChanged += LeftsChanged;
                }
                if (rightNotifier != null)
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
                var leftNotifier = Lefts as INotifyCollectionChanged;
                var rightNotifier = Rights as INotifyCollectionChanged;
                if (leftNotifier != null)
                {
                    leftNotifier.CollectionChanged -= LeftsChanged;
                }
                if (rightNotifier != null)
                {
                    rightNotifier.CollectionChanged -= RightsChanged;
                }
            }
        }
    }

    internal class OneWaySynchronizationMultipleDependency<TSource, TTarget, TSourceDep, TTargetDep> : OutputDependency
        where TSource : class
        where TTarget : class
        where TSourceDep : class
        where TTargetDep : class
    {
        private TransformationRuleBase<TSource, TTarget> parentRule;
        private TransformationRuleBase<TSourceDep, TTargetDep> childRule;

        private Func<TSource, IEnumerableExpression<TSourceDep>> __sourceGetter;
        private Func<TTarget, ICollection<TTargetDep>> __targetGetter;

        public OneWaySynchronizationMultipleDependency(TransformationRuleBase<TSource, TTarget> parentRule,TransformationRuleBase<TSourceDep, TTargetDep> childRule, Expression<Func<TSource, IEnumerableExpression<TSourceDep>>> leftSelector, Expression<Func<TTarget, ICollection<TTargetDep>>> rightSelector)
        {
            if (parentRule == null) throw new ArgumentNullException("parentRule");
            if (childRule == null) throw new ArgumentNullException("childRule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            this.parentRule = parentRule;
            this.childRule = childRule;

            this.__sourceGetter = ExpressionCompileRewriter.Compile(leftSelector);
            this.__targetGetter = ExpressionCompileRewriter.Compile(rightSelector);
        }

        private IEnumerable<TSourceDep> GetSourceItems(TSource source, bool incremental)
        {
            var lefts = __sourceGetter(source);
            if (incremental)
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TTargetDep> GetTargetCollection(TTarget right)
        {
            return __targetGetter(right);
        }

        protected override void HandleReadyComputation(Computation computation)
        {
            var syncComputation = computation as SynchronizationComputation<TSource, TTarget>;
            var input = GetSourceItems(syncComputation.Input, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeCollections(input, GetTargetCollection(outp), syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
                if (dependency != null)
                {
                    syncComputation.Dependencies.Add(dependency);
                }
            });
        }

        protected virtual IDisposable SynchronizeCollections(IEnumerable<TSourceDep> source, ICollection<TTargetDep> targets, ISynchronizationContext context, bool ignoreCandidates)
        {
            if (targets != null)
            {
                if (targets.IsReadOnly) throw new InvalidOperationException("Collection is read-only!");
                IEnumerable<TTargetDep> rightsSaved = targets;
                if (source == null || (context.Direction == SynchronizationDirection.LeftToRightForced || context.Direction == SynchronizationDirection.RightToLeftForced))
                {
                    rightsSaved = targets.ToArray();
                    targets.Clear();
                }
                var doubles = new HashSet<TTargetDep>();
                IEnumerable rightContext = ignoreCandidates ? null : targets;
                foreach (var item in source)
                {
                    var comp = (SynchronizationComputation<TSourceDep, TTargetDep>)context.CallTransformation(childRule, new object[] { item }, rightContext);
                    comp.DoWhenOutputIsAvailable((inp, outp) =>
                    {
                        if (!targets.Contains(outp))
                        {
                            targets.Add(outp);
                        }
                        else
                        {
                            doubles.Add(outp);
                        }
                    });
                }
                return RegisterChangePropagationHooks(source, targets, context);
            }
            else
            {
                throw new NotSupportedException("Target collection must not be null!");
            }
        }

        private IDisposable RegisterChangePropagationHooks(IEnumerable<TSourceDep> lefts, ICollection<TTargetDep> rights, ISynchronizationContext context)
        {
            if (context.ChangePropagation != ChangePropagationMode.None)
            {
                if (lefts is INotifyCollectionChanged)
                {
                    return new NotificationHook(lefts, rights, context, this);
                }
            }
            return null;
        }

        private void ProcessSourceChanges(IEnumerable<TSourceDep> source, ICollection<TTargetDep> targets, ISynchronizationContext context, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        TSourceDep item = (TSourceDep)e.OldItems[i];
                        var right = context.Trace.ResolveIn(childRule, item);
                        if (right != null)
                        {
                            targets.Remove(right);
                        }
                    }
                }
                if (e.NewItems != null)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        TSourceDep item = (TSourceDep)e.NewItems[i];
                        AddCorrespondingToTargets(targets, context, item);
                    }
                }
            }
            else
            {
                var leftsSaved = new List<TSourceDep>(source);
                targets.Clear();
                foreach (var item in leftsSaved)
                {
                    AddCorrespondingToTargets(targets, context, item);
                }
            }
        }

        private void AddCorrespondingToTargets(ICollection<TTargetDep> targets, ISynchronizationContext context, TSourceDep item)
        {
            var comp = context.CallTransformation(childRule, new object[] { item }, null) as SynchronizationComputation<TSourceDep, TTargetDep>;
            comp.DoWhenOutputIsAvailable((inp, outp) =>
            {
                targets.Add(outp);
            });
        }

        private class NotificationHook : IDisposable
        {
            public IEnumerable<TSourceDep> Lefts { get; private set; }
            public ICollection<TTargetDep> Rights { get; private set; }
            public ISynchronizationContext Context { get; private set; }
            public OneWaySynchronizationMultipleDependency<TSource, TTarget, TSourceDep, TTargetDep> Parent { get; private set; }

            public NotificationHook(IEnumerable<TSourceDep> lefts, ICollection<TTargetDep> rights, ISynchronizationContext context, OneWaySynchronizationMultipleDependency<TSource, TTarget, TSourceDep, TTargetDep> parent)
            {
                Lefts = lefts;
                Rights = rights;
                Context = context;
                Parent = parent;

                var notifier = lefts as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged += LeftsChanged;
                }
            }

            private void LeftsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Parent.ProcessSourceChanges(Lefts, Rights, Context, e);
            }

            public void Dispose()
            {
                var notifier = Lefts as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }
    }
}
