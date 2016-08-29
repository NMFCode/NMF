using NMF.Expressions;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    internal class SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight>
        where TLeft : class
        where TRight : class
        where TDepLeft : class
        where TDepRight : class
    {
        private SynchronizationRule<TLeft, TRight> parentRule;
        private SynchronizationRule<TDepLeft, TDepRight> childRule;

        private Func<TLeft, TDepLeft> leftGetter;
        private Func<TRight, TDepRight> rightGetter;
        private Action<TRight, TDepRight> rightSetter;
        private Action<TLeft, TDepLeft> leftSetter;

        private ObservingFunc<TLeft, TDepLeft> leftFunc;
        private ObservingFunc<TRight, TDepRight> rightFunc;

        public SynchronizationSingleDependency(SynchronizationRule<TLeft, TRight> parentRule, SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector)
            : this(parentRule, childRule, leftSelector, rightSelector, null, null) { }

        public SynchronizationSingleDependency(SynchronizationRule<TLeft, TRight> parentRule, SynchronizationRule<TDepLeft, TDepRight> childRule, Expression<Func<TLeft, TDepLeft>> leftSelector, Expression<Func<TRight, TDepRight>> rightSelector, Action<TLeft, TDepLeft> leftSetter, Action<TRight, TDepRight> rightSetter)
        {
            if (parentRule == null) throw new ArgumentNullException("parentRule");
            if (childRule == null) throw new ArgumentNullException("childRule");
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            this.parentRule = parentRule;
            this.childRule = childRule;

            this.leftGetter = ExpressionCompileRewriter.Compile(leftSelector);
            this.rightGetter = ExpressionCompileRewriter.Compile(rightSelector);
            if (leftSetter == null)
            {
                var leftSetterExp = SetExpressionRewriter.CreateSetter(leftSelector);
                if (leftSetterExp != null)
                {
                    this.leftSetter = ExpressionCompileRewriter.Compile(leftSetterExp);
                }
                this.leftFunc = Observable.Func(leftSelector);
            }
            else
            {
                this.leftSetter = leftSetter;
                this.leftFunc = Observable.Func(leftSelector, leftSetter);
            }
            if (rightSetter == null)
            {
                var rightSetterExp = SetExpressionRewriter.CreateSetter(rightSelector);
                if (rightSetterExp != null)
                {
                    this.rightSetter = ExpressionCompileRewriter.Compile(rightSetterExp);
                }
                this.rightFunc = Observable.Func(rightSelector);
            }
            else
            {
                this.rightSetter = rightSetter;
                this.rightFunc = Observable.Func(rightSelector, rightSetter);
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
            var left = leftFunc.InvokeReversable(syncComputation.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Opposite.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                left.Value, right.Value, leftSetter, rightSetter);
            return new TwoWaySynchronization(left, right, syncComputation, this);
        }

        private IDisposable HandleTwoWayRTLSynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Opposite.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                right.Value, left.Value, leftSetter, rightSetter);
            return new TwoWaySynchronization(left, right, syncComputation.Opposite, this);
        }

        private class TwoWaySynchronization : IDisposable
        {
            public INotifyReversableValue<TDepLeft> Left { get; private set; }
            public INotifyReversableValue<TDepRight> Right { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public TwoWaySynchronization(INotifyReversableValue<TDepLeft> left, INotifyReversableValue<TDepRight> right, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Left = left;
                Right = right;
                Parent = parent;
                Computation = computation;

                Left.ValueChanged += Left_ValueChanged;
                Right.ValueChanged += Right_ValueChanged;
            }

            private void Right_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Action<TLeft, TDepLeft> leftSetter = (l, val) => Left.Value = val;
                Action<TRight, TDepRight> rightSetter = (r, val) => Right.Value = val;

                Parent.CallRTLTransformationForInput(Computation.Opposite, SynchronizationDirection.RightToLeftForced, Right.Value, Left.Value, leftSetter, rightSetter);
            }

            private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Action<TLeft, TDepLeft> leftSetter = (l, val) => Left.Value = val;
                Action<TRight, TDepRight> rightSetter = (r, val) => Right.Value = val;

                Parent.CallLTRTransformationForInput(Computation, SynchronizationDirection.LeftToRightForced, Left.Value, Right.Value, leftSetter, rightSetter);
            }

            public void Dispose()
            {
                Left.ValueChanged -= Left_ValueChanged;
                Right.ValueChanged -= Right_ValueChanged;
                Left.Dispose();
                Right.Dispose();
            }
        }

        private IDisposable HandleOneWayLTRSynchronization(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var input = leftFunc.Observe(syncComputation.Input);
            Action<TLeft, TDepLeft> leftSetter = (left, val) =>
            {
                var reversable = input as INotifyReversableValue<TDepLeft>;
                if (reversable != null && reversable.IsReversable)
                {
                    reversable.Value = val;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The expression {0} cannot be written to as it is not reversable.", input));
                }
            };
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input.Value, rightGetter(syncComputation.Opposite.Input), leftSetter, rightSetter);
            return new OneWayLTRSynchronization(input, syncComputation, this);
        }

        private class OneWayLTRSynchronization : IDisposable
        {
            public INotifyValue<TDepLeft> Left { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public OneWayLTRSynchronization(INotifyValue<TDepLeft> left, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Left = left;
                Parent = parent;
                Computation = computation;

                Left.ValueChanged += Left_ValueChanged;
            }

            private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Parent.CallLTRTransformationForInput(Computation, SynchronizationDirection.LeftToRightForced, Left.Value, Parent.rightGetter(Computation.Opposite.Input), Parent.leftSetter, Parent.rightSetter);
            }

            public void Dispose()
            {
                Left.ValueChanged -= Left_ValueChanged;
                Left.Dispose();
            }
        }

        private IDisposable HandleOneWayRTLSynchronization(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var input = rightFunc.Observe(syncComputation.Input);
            Action<TRight, TDepRight> rightSetter = (right, val) =>
            {
                var reversable = input as INotifyReversableValue<TDepRight>;
                if (reversable != null && reversable.IsReversable)
                {
                    reversable.Value = val;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The expression {0} cannot be written to as it is not reversable.", input));
                }
            };
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input.Value, leftGetter(syncComputation.Opposite.Input), leftSetter, rightSetter);
            return new OneWayRTLDependency(input, syncComputation, this);
        }

        private class OneWayRTLDependency : IDisposable
        {
            public INotifyValue<TDepRight> Right { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TRight, TLeft> Computation { get; private set; }

            public OneWayRTLDependency(INotifyValue<TDepRight> right, SynchronizationComputation<TRight, TLeft> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                Right = right;
                Parent = parent;
                Computation = computation;

                Right.ValueChanged += Left_ValueChanged;
            }

            private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                Parent.CallRTLTransformationForInput(Computation, SynchronizationDirection.RightToLeftForced, Right.Value, Parent.leftGetter(Computation.Opposite.Input), Parent.leftSetter, Parent.rightSetter);
            }

            public void Dispose()
            {
                Right.ValueChanged -= Left_ValueChanged;
                Right.Dispose();
            }
        }

        private void HandleStaticLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var input = leftGetter(syncComputation.Input);
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input, rightGetter(syncComputation.Opposite.Input), leftSetter, rightSetter);
        }

        private void HandleStaticRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var input = rightGetter(syncComputation.Input);
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                input, leftGetter(syncComputation.Opposite.Input), leftSetter, rightSetter);
        }

        private void CallLTRTransformationForInput(SynchronizationComputation<TLeft, TRight> syncComputation, SynchronizationDirection direction, TDepLeft input, TDepRight context, Action<TLeft, TDepLeft> leftSetter, Action<TRight, TDepRight> rightSetter)
        {
            var right = syncComputation.Opposite.Input;
            if (input != null)
            {
                if (rightSetter == null)
                {
                    return;
                }
                var comp = syncComputation.TransformationContext.CallTransformation(childRule.LeftToRight, new object[] { input }, new object [] { context });
                if (comp == null) return;
                if (!comp.IsDelayed)
                {
                    rightSetter(right, comp.Output as TDepRight);
                }
                else
                {
                    comp.OutputInitialized += (o, e) => rightSetter(right, comp.Output as TDepRight);
                }
            }
            else
            {
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
                    rightSetter(right, null);
                }
            }
        }

        private void CallRTLTransformationForInput(SynchronizationComputation<TRight, TLeft> syncComputation, SynchronizationDirection direction, TDepRight input, TDepLeft context, Action<TLeft, TDepLeft> leftSetter, Action<TRight, TDepRight> rightSetter)
        {
            var left = syncComputation.Opposite.Input;
            if (input != null)
            {
                if (leftSetter == null)
                {
                    return;
                }
                var comp = syncComputation.TransformationContext.CallTransformation(childRule.RightToLeft, new object[] { input }, new object[] { context });
                if (comp == null) return;
                if (!comp.IsDelayed)
                {
                    leftSetter(left, comp.Output as TDepLeft);
                }
                else
                {
                    comp.OutputInitialized += (o, e) => leftSetter(left, comp.Output as TDepLeft);
                };
            }
            else
            {
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
                    leftSetter(left, null);
                }
            }
        }

        public ITransformationRuleDependency CreateLeftToRightDependency(ObservingFunc<TLeft, TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedLTRDependency(this, guard);
            }
            else
            {
                return new LTRDependency(this);
            }
        }

        public ITransformationRuleDependency CreateRightToLeftDependency(ObservingFunc<TLeft, TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedRTLDependency(this, guard);
            }
            else
            {
                return new RTLDependency(this);
            }
        }

        public ITransformationRuleDependency CreateLeftToRightOnlyDependency(ObservingFunc<TLeft, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedLTROnlyDependency(this, guard);
            }
            else
            {
                return new LTROnlyDependency(this);
            }
        }

        public ITransformationRuleDependency CreateRightToLeftOnlyDependency(ObservingFunc<TRight, bool> guard)
        {
            if (guard != null)
            {
                return new GuardedRTLOnlyDependency(this, guard);
            }
            else
            {
                return new RTLOnlyDependency(this);
            }
        }

        private class LTRDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LTRDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateLeftToRightSynchronization(true, (SynchronizationComputation<TLeft, TRight>)computation);
            }
        }

        private class LTROnlyDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public LTROnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateLeftToRightSynchronization(false, (SynchronizationComputation<TLeft, TRight>)computation);
            }
        }

        private class RTLDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RTLDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateRightToLeftSynchronization(true, (SynchronizationComputation<TRight, TLeft>)computation);
            }
        }

        private class RTLOnlyDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;

            public RTLOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
            {
                this.parent = parent;
            }

            protected override void HandleReadyComputation(Computation computation)
            {
                parent.CreateRightToLeftSynchronization(false, (SynchronizationComputation<TRight, TLeft>)computation);
            }
        }

        private class GuardedLTRDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private ObservingFunc<TLeft, TRight, bool> guard;

            public GuardedLTRDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, TRight, bool> guard)
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
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TLeft, TRight>(syncComputation, parent.CreateLeftToRightSynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private class GuardedLTROnlyDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private ObservingFunc<TLeft, bool> guard;

            public GuardedLTROnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, bool> guard)
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
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TLeft, TRight>(syncComputation, parent.CreateLeftToRightOnlySynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private class GuardedRTLDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private ObservingFunc<TLeft, TRight, bool> guard;

            public GuardedRTLDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TLeft, TRight, bool> guard)
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
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TRight, TLeft>(syncComputation, parent.CreateRightToLeftSynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }

        private class GuardedRTLOnlyDependency : OutputDependency
        {
            private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent;
            private ObservingFunc<TRight, bool> guard;

            public GuardedRTLOnlyDependency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent, ObservingFunc<TRight, bool> guard)
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
                        syncComputation.Dependencies.Add(new GuardedSynchronization<TRight, TLeft>(syncComputation, parent.CreateRightToLeftOnlySynchronization, tracker));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    internal class GuardedSynchronization<TLeft, TRight> : IDisposable
        where TLeft : class
        where TRight : class
    {
        public SynchronizationComputation<TLeft, TRight> Computation { get; set; }
        public Func<SynchronizationComputation<TLeft, TRight>, IDisposable> Func { get; set; }
        public IDisposable Current { get; set; }
        public INotifyValue<bool> Guard { get; set; }

        public GuardedSynchronization(SynchronizationComputation<TLeft, TRight> computation, Func<SynchronizationComputation<TLeft, TRight>, IDisposable> func, INotifyValue<bool> guard)
        {
            Computation = computation;
            Func = func;
            Guard = guard;

            if (guard.Value)
            {
                Current = func(computation);
                if (Current != null)
                {
                    Computation.Dependencies.Add(Current);
                }
            }

            Guard.ValueChanged += Guard_ValueChanged;
        }

        private void Guard_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (Current != null)
            {
                Computation.Dependencies.Remove(Current);
                Current.Dispose();
                Current = null;
            }
            if (Guard.Value)
            {
                Current = Func(Computation);
                if (Current != null)
                {
                    Computation.Dependencies.Add(Current);
                }
            }
        }

        public void Dispose()
        {
            Guard.ValueChanged -= Guard_ValueChanged;
            Guard.Dispose();
        }
    }
}
