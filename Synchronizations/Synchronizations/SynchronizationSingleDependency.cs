using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    internal class SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>
    {
        internal SynchronizationRule<TDepLeft, TDepRight> childRule;

        internal Func<TLeft, ITransformationContext, TDepLeft> leftGetter;
        internal Func<TRight, ITransformationContext, TDepRight> rightGetter;
        internal Action<TRight, ITransformationContext, TDepRight> rightSetter;
        internal Action<TLeft, ITransformationContext, TDepLeft> leftSetter;

        private readonly ObservingFunc<TLeft, ITransformationContext, TDepLeft> leftFunc;
        private readonly ObservingFunc<TRight, ITransformationContext, TDepRight> rightFunc;

        public SynchronizationSingleDependency(SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, bool allowLeftSetterNull, bool allowRightSetterNull)
            : this(childRule, leftSelector, rightSelector, null, null, allowLeftSetterNull, allowRightSetterNull) { }

        public SynchronizationSingleDependency(SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, ITransformationContext, TDepLeft>> leftSelector, Expression<Func<TRight, ITransformationContext, TDepRight>> rightSelector, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Action<TRight, ITransformationContext, TDepRight> rightSetter, bool allowLeftSetterNull, bool allowRightSetterNull)
        {
            if (childRule == null) throw new ArgumentNullException(nameof(childRule));
            if (leftSelector == null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));

            this.childRule = childRule;

            leftGetter = ExpressionCompileRewriter.Compile(leftSelector);
            rightGetter = ExpressionCompileRewriter.Compile(rightSelector);
            if (leftSetter == null)
            {
                var leftSetterExp = SetExpressionRewriter.CreateSetter(leftSelector);
                if (leftSetterExp != null)
                {
                    this.leftSetter = ExpressionCompileRewriter.Compile(leftSetterExp);
                }
                else if (!allowLeftSetterNull)
                {
                    throw new ArgumentException( $"The expression '{leftSelector}' cannot be inverted", nameof( leftSelector ) );
                }
                leftFunc = Observable.Func(leftSelector);
            }
            else
            {
                this.leftSetter = leftSetter;
                leftFunc = Observable.Func(leftSelector, leftSetter);
            }
            if (rightSetter == null)
            {
                var rightSetterExp = SetExpressionRewriter.CreateSetter(rightSelector);
                if (rightSetterExp != null)
                {
                    this.rightSetter = ExpressionCompileRewriter.Compile(rightSetterExp);
                }
                else if (!allowRightSetterNull)
                {
                    throw new ArgumentException( $"The expression {rightSelector} cannot be inverted", nameof( rightSelector ) );
                }
                rightFunc = Observable.Func(rightSelector);
            }
            else
            {
                this.rightSetter = rightSetter;
                rightFunc = Observable.Func(rightSelector, rightSetter);
            }
        }

        private IDisposable CreateLeftToRightOnlySynchronization(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            return CreateLeftToRightSynchronization(false, syncComputation);
        }

        private IDisposable CreateLeftToRightSynchronization(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            return CreateLeftToRightSynchronization(true, syncComputation);
        }

        private IDisposable CreateLeftToRightSynchronization(bool allowRightToLeft, SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            IDisposable dependency = null;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticLTRDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    dependency = HandleOneWayLTRSynchronization(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowRightToLeft)
                    {
                        dependency = HandleTwoWayLTRSynchronization(syncComputation);
                    }
                    else
                    {
                        dependency = HandleOneWayLTRSynchronization(syncComputation);
                    }
                    break;
                default:
                    break;
            }
            if (dependency != null) syncComputation.Dependencies.Add(dependency);

            return dependency;
        }

        private IDisposable CreateRightToLeftOnlySynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            return CreateRightToLeftSynchronization(false, syncComputation);
        }

        private IDisposable CreateRightToLeftSynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            return CreateRightToLeftSynchronization(true, syncComputation);
        }

        private IDisposable CreateRightToLeftSynchronization(bool allowLeftToRight, SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            IDisposable dependency = null;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticRTLDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    dependency = HandleOneWayRTLSynchronization(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowLeftToRight)
                    {
                        dependency = HandleTwoWayRTLSynchronization(syncComputation);
                    }
                    else
                    {
                        dependency = HandleOneWayRTLSynchronization(syncComputation);
                    }
                    break;
                default:
                    break;
            }
            if (dependency != null) syncComputation.Dependencies.Add(dependency);
            return dependency;
        }

        private IDisposable HandleTwoWayLTRSynchronization(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Input, syncComputation.TransformationContext);
            left.Successors.SetDummy();
            var right = rightFunc.InvokeReversable(syncComputation.Opposite.Input, syncComputation.TransformationContext);
            right.Successors.SetDummy();
            Action<TLeft, ITransformationContext, TDepLeft> leftSetter = (l, ctx, val) => left.Value = val;
            Action<TRight, ITransformationContext, TDepRight> rightSetter = (r, ctx, val) => right.Value = val;
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                left.Value, right.Value, leftSetter, rightSetter);
            return new IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight>(left, right, syncComputation, this);
        }

        private IDisposable HandleTwoWayRTLSynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Opposite.Input, syncComputation.TransformationContext);
            left.Successors.SetDummy();
            var right = rightFunc.InvokeReversable(syncComputation.Input, syncComputation.TransformationContext);
            right.Successors.SetDummy();
            Action<TLeft, ITransformationContext, TDepLeft> leftSetter = ( l, ctx, val ) => left.Value = val;
            Action<TRight, ITransformationContext, TDepRight> rightSetter = ( r, ctx, val ) => right.Value = val;
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                right.Value, left.Value, leftSetter, rightSetter);
            return new IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight>(left, right, syncComputation.Opposite, this);
        }

        private IDisposable HandleOneWayLTRSynchronization(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var input = leftFunc.Observe(syncComputation.Input, syncComputation.TransformationContext);
            input.Successors.SetDummy();
            Action<TLeft, ITransformationContext, TDepLeft> leftSetter = (left, ctx, val) =>
            {
                if(input is INotifyReversableValue<TDepLeft> reversable && reversable.IsReversable)
                {
                    reversable.Value = val;
                }
                else
                {
                    throw new InvalidOperationException( string.Format( "The expression {0} cannot be written to as it is not reversable.", input ) );
                }
            };
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input.Value, rightGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext), leftSetter, rightSetter);
            return new OneWayLeftToRightSynchronization(input, syncComputation, this);
        }

        private sealed class OneWayLeftToRightSynchronization : IDisposable
        {
            public INotifyValue<TDepLeft> Left { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public OneWayLeftToRightSynchronization(INotifyValue<TDepLeft> left, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Left = left;
                Parent = parent;
                Computation = computation;

                Left.ValueChanged += Left_ValueChanged;
            }

            private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Parent.CallLTRTransformationForInput(Computation, SynchronizationDirection.LeftToRightForced, Left.Value, Parent.rightGetter(Computation.Opposite.Input, Computation.TransformationContext), Parent.leftSetter, Parent.rightSetter);
            }

            public void Dispose()
            {
                Left.ValueChanged -= Left_ValueChanged;
                Left.Dispose();
            }
        }

        private IDisposable HandleOneWayRTLSynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var input = rightFunc.Observe(syncComputation.Input, syncComputation.TransformationContext);
            input.Successors.SetDummy();
            Action<TRight, ITransformationContext, TDepRight> rightSetter = (right, ctx, val) =>
            {
                if(input is INotifyReversableValue<TDepRight> reversable && reversable.IsReversable)
                {
                    reversable.Value = val;
                }
                else
                {
                    throw new InvalidOperationException( string.Format( "The expression {0} cannot be written to as it is not reversable.", input ) );
                }
            };
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input.Value, leftGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext), leftSetter, rightSetter);
            return new OneWayRightToLeftDependency(input, syncComputation, this);
        }

        private sealed class OneWayRightToLeftDependency : IDisposable
        {
            public INotifyValue<TDepRight> Right { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TRight, TLeft> Computation { get; private set; }

            public OneWayRightToLeftDependency(INotifyValue<TDepRight> right, SynchronizationComputation<TRight, TLeft> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Right = right;
                Parent = parent;
                Computation = computation;

                Right.ValueChanged += Left_ValueChanged;
            }

            private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Parent.CallRTLTransformationForInput(Computation, SynchronizationDirection.RightToLeftForced, Right.Value, Parent.leftGetter(Computation.Opposite.Input, Computation.TransformationContext), Parent.leftSetter, Parent.rightSetter);
            }

            public void Dispose()
            {
                Right.ValueChanged -= Left_ValueChanged;
                Right.Dispose();
            }
        }

        private void HandleStaticLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var input = leftGetter(syncComputation.Input, syncComputation.TransformationContext);
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input, rightGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext ), leftSetter, rightSetter);
        }

        private void HandleStaticRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var input = rightGetter(syncComputation.Input, syncComputation.TransformationContext );
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input, leftGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext ), leftSetter, rightSetter);
        }

        internal void CallLTRTransformationForInput(SynchronizationComputation<TLeft, TRight> syncComputation, SynchronizationDirection direction, TDepLeft input, TDepRight context, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Action<TRight, ITransformationContext, TDepRight> rightSetter)
        {
            if (direction == SynchronizationDirection.CheckOnly)
            {
                // two-way change propagation is handled through dependency
                if (syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.None)
                {
                    Match(syncComputation, input, rightGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext));
                }
                return;
            }
            var right = syncComputation.Opposite.Input;
            if (input != null)
            {
                if (rightSetter != null)
                {
                    var comp = syncComputation.TransformationContext.CallTransformation(childRule.LeftToRight, new object[] { input }, new object[] { context });
                    if (comp == null) return;
                    SetRight(syncComputation, rightSetter, right, comp);
                }
                return;
            }

            if (direction == SynchronizationDirection.LeftWins)
            {
                CallRTLTransformationForInput(syncComputation.Opposite, SynchronizationDirection.LeftWins, context, input, leftSetter, rightSetter);
            }
            else if (direction == SynchronizationDirection.LeftToRightForced)
            {
                if (rightSetter == null)
                {
                    return;
                }
                rightSetter(right, syncComputation.TransformationContext, default);
            }
        }

        private static void SetRight(SynchronizationComputation<TLeft, TRight> syncComputation, Action<TRight, ITransformationContext, TDepRight> rightSetter, TRight right, Computation comp)
        {
            if (!comp.IsDelayed)
            {
                rightSetter(right, syncComputation.TransformationContext, (TDepRight)comp.Output);
            }
            else
            {
                comp.OutputInitialized += (o, e) => rightSetter(right, syncComputation.TransformationContext, (TDepRight)comp.Output);
            }
        }

        internal void CallRTLTransformationForInput(SynchronizationComputation<TRight, TLeft> syncComputation, SynchronizationDirection direction, TDepRight input, TDepLeft context, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, Action<TRight, ITransformationContext, TDepRight> rightSetter)
        {
            if (direction == SynchronizationDirection.CheckOnly)
            {
                // two-way change propagation is handled through dependency
                if (syncComputation.SynchronizationContext.ChangePropagation == ChangePropagationMode.None)
                {
                    Match(syncComputation.Opposite, leftGetter(syncComputation.Opposite.Input, syncComputation.TransformationContext), input);
                }
                return;
            }
            var left = syncComputation.Opposite.Input;
            if (input != null)
            {
                if (leftSetter != null)
                {
                    var comp = syncComputation.TransformationContext.CallTransformation(childRule.RightToLeft, new object[] { input }, new object[] { context });
                    if (comp == null) return;
                    SetLeft(syncComputation, leftSetter, left, comp);
                }
                return;
            }
            if (direction == SynchronizationDirection.RightWins)
            {
                CallLTRTransformationForInput(syncComputation.Opposite, SynchronizationDirection.RightWins, context, input, leftSetter, rightSetter);
            }
            else if (direction == SynchronizationDirection.RightToLeftForced)
            {
                if (leftSetter == null)
                {
                    return;
                }
                leftSetter(left, syncComputation.TransformationContext, default);
            }
        }

        private static void SetLeft(SynchronizationComputation<TRight, TLeft> syncComputation, Action<TLeft, ITransformationContext, TDepLeft> leftSetter, TLeft left, Computation comp)
        {
            if (!comp.IsDelayed)
            {
                leftSetter(left, syncComputation.TransformationContext, (TDepLeft)comp.Output);
            }
            else
            {
                comp.OutputInitialized += (o, e) => leftSetter(left, syncComputation.TransformationContext, (TDepLeft)comp.Output);
            }
        }

        private void Match(SynchronizationComputation<TLeft, TRight> baseCorrespondence, TDepLeft leftValue, TDepRight rightValue)
        {
            if (leftValue == null && rightValue == null) return;
            if ((leftValue != null && rightValue == null)
                || leftValue == null
                || !childRule.ShouldCorrespond(leftValue, rightValue, baseCorrespondence.SynchronizationContext))
            {
                baseCorrespondence.SynchronizationContext.Inconsistencies.Add(new ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight>(this, baseCorrespondence, leftValue, rightValue));
            }
        }

        public ITransformationRuleDependency CreateLeftToRightDependency(ObservingFunc<TLeft, TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedLeftToRightDependency(this, guard);
            }
            else
            {
                return new LeftToRightDependency(this);
            }
        }

        public ITransformationRuleDependency CreateRightToLeftDependency(ObservingFunc<TLeft, TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedRrightToLeftDependency(this, guard);
            }
            else
            {
                return new RightToLeftDependency(this);
            }
        }

        public ITransformationRuleDependency CreateLeftToRightOnlyDependency(ObservingFunc<TLeft, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedLeftToRightOnlyDependency(this, guard);
            }
            else
            {
                return new LeftToRightOnlyDependency(this);
            }
        }

        public ITransformationRuleDependency CreateRightToLeftOnlyDependency(ObservingFunc<TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedRightToLeftOnlyDependency(this, guard);
            }
            else
            {
                return new RightToLeftOnlyDependency(this);
            }
        }

        private sealed class LeftToRightDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LeftToRightDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateLeftToRightSynchronization(true, (SynchronizationComputation<TLeft, TRight>)computation);
            }
        }

        private sealed class LeftToRightOnlyDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LeftToRightOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateLeftToRightSynchronization(false, (SynchronizationComputation<TLeft, TRight>)computation);
            }
        }

        private sealed class RightToLeftDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RightToLeftDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateRightToLeftSynchronization(true, (SynchronizationComputation<TRight, TLeft>)computation);
            }
        }

        private sealed class RightToLeftOnlyDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RightToLeftOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateRightToLeftSynchronization(false, (SynchronizationComputation<TRight, TLeft>)computation);
            }
        }

        private sealed class GuardedLeftToRightDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private readonly ObservingFunc<TLeft, TRight, bool> guard;

            public GuardedLeftToRightDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, TRight, bool> guard)
            {
                this.parent = parent;
                this.guard = guard;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                var syncComputation = (SynchronizationComputation<TLeft, TRight>)computation;
                var left = syncComputation.Input;
                var right = syncComputation.Opposite.Input;
                switch (syncComputation.SynchronizationContext.ChangePropagation)
                {
                    case ChangePropagationMode.None:
                        if (guard.Evaluate(left, right)) parent.CreateLeftToRightSynchronization(syncComputation);
                        break;
                    case ChangePropagationMode.OneWay:
                    case ChangePropagationMode.TwoWay:
                        var tracker = guard.Observe(left, right);
                        tracker.Successors.SetDummy();
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TLeft, TRight>(syncComputation, parent.CreateLeftToRightSynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private sealed class GuardedLeftToRightOnlyDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private readonly ObservingFunc<TLeft, bool> guard;

            public GuardedLeftToRightOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, bool> guard)
            {
                this.parent = parent;
                this.guard = guard;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                var syncComputation = (SynchronizationComputation<TLeft, TRight>)computation;
                var left = syncComputation.Input;
                switch (syncComputation.SynchronizationContext.ChangePropagation)
                {
                    case ChangePropagationMode.None:
                        if (guard.Evaluate(left)) parent.CreateLeftToRightOnlySynchronization(syncComputation);
                        break;
                    case ChangePropagationMode.OneWay:
                    case ChangePropagationMode.TwoWay:
                        var tracker = guard.Observe(left);
                        tracker.Successors.SetDummy();
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TLeft, TRight>(syncComputation, parent.CreateLeftToRightOnlySynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private sealed class GuardedRrightToLeftDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private readonly ObservingFunc<TLeft, TRight, bool> guard;

            public GuardedRrightToLeftDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, TRight, bool> guard)
            {
                this.parent = parent;
                this.guard = guard;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                var syncComputation = (SynchronizationComputation<TRight, TLeft>)computation;
                var left = syncComputation.Opposite.Input;
                var right = syncComputation.Input;
                switch (syncComputation.SynchronizationContext.ChangePropagation)
                {
                    case ChangePropagationMode.None:
                        if (guard.Evaluate(left, right)) parent.CreateRightToLeftSynchronization(syncComputation);
                        break;
                    case ChangePropagationMode.OneWay:
                    case ChangePropagationMode.TwoWay:
                        var tracker = guard.Observe(left, right);
                        tracker.Successors.SetDummy();
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TRight, TLeft>(syncComputation, parent.CreateRightToLeftSynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private sealed class GuardedRightToLeftOnlyDependency : OutputDependency
        {
            private readonly SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private readonly ObservingFunc<TRight, bool> guard;

            public GuardedRightToLeftOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TRight, bool> guard)
            {
                this.parent = parent;
                this.guard = guard;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                var syncComputation = (SynchronizationComputation<TRight, TLeft>)computation;
                var right = syncComputation.Input;
                switch (syncComputation.SynchronizationContext.ChangePropagation)
                {
                    case ChangePropagationMode.None:
                        if (guard.Evaluate(right)) parent.CreateRightToLeftOnlySynchronization(syncComputation);
                        break;
                    case ChangePropagationMode.OneWay:
                    case ChangePropagationMode.TwoWay:
                        var tracker = guard.Observe(right);
                        tracker.Successors.SetDummy();
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TRight, TLeft>(syncComputation, parent.CreateRightToLeftOnlySynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
