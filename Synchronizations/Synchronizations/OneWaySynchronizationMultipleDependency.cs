using NMF.Expressions;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class OneWaySynchronizationMultipleDependency<TSource, TTarget, TSourceDep, TTargetDep> : OutputDependency
        where TSource : class
        where TTarget : class
        where TSourceDep : class
        where TTargetDep : class
    {
        private TransformationRuleBase<TSource, TTarget> parentRule;
        private TransformationRuleBase<TSourceDep, TTargetDep> childRule;

        private Func<TSource, ITransformationContext, IEnumerableExpression<TSourceDep>> __sourceGetter;
        private Func<TTarget, ITransformationContext, ICollection<TTargetDep>> __targetGetter;

        public OneWaySynchronizationMultipleDependency(TransformationRuleBase<TSource, TTarget> parentRule,TransformationRuleBase<TSourceDep, TTargetDep> childRule, Expression<Func<TSource, ITransformationContext, IEnumerableExpression<TSourceDep>>> leftSelector, Expression<Func<TTarget, ITransformationContext, ICollection<TTargetDep>>> rightSelector)
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

        private IEnumerable<TSourceDep> GetSourceItems(TSource source, ITransformationContext context, bool incremental)
        {
            var lefts = __sourceGetter(source, context);
            if (incremental)
            {
                return lefts.AsNotifiable();
            }
            else
            {
                return lefts;
            }
        }

        private ICollection<TTargetDep> GetTargetCollection(TTarget right, ITransformationContext context)
        {
            return __targetGetter(right, context);
        }

        protected override void HandleReadyComputation(Computation computation)
        {
            var syncComputation = computation as SynchronizationComputation<TSource, TTarget>;
            var input = GetSourceItems(syncComputation.Input, computation.TransformationContext, syncComputation.SynchronizationContext.ChangePropagation != ChangePropagationMode.None);
            syncComputation.DoWhenOutputIsAvailable((inp, outp) =>
            {
                var dependency = SynchronizeCollections(input, GetTargetCollection(outp, syncComputation.TransformationContext), syncComputation.SynchronizationContext, syncComputation.OmitCandidateSearch);
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

                if(lefts is INotifyCollectionChanged notifier)
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
                if(Lefts is INotifyCollectionChanged notifier)
                {
                    notifier.CollectionChanged -= LeftsChanged;
                }
            }
        }
    }
}
