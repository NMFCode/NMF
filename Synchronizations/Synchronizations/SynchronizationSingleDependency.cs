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

        public IDisposable CreateLeftToRightDependency(bool allowRightToLeft, SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            IDisposable dependency = null;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticLTRDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    dependency = HandleOneWayLTRDependency(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowRightToLeft)
                    {
                        dependency = HandleTwoWayLTRDependency(syncComputation);
                    }
                    else
                    {
                        dependency = HandleOneWayLTRDependency(syncComputation);
                    }
                    break;
                default:
                    break;
            }
            if (dependency != null) syncComputation.Dependencies.Add(dependency);

            return dependency;
        }

        public IDisposable CreateRightToLeftDependency(bool allowLeftToRight, SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            IDisposable dependency = null;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticRTLDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    dependency = HandleOneWayRTLDependency(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowLeftToRight)
                    {
                        dependency = HandleTwoWayRTLDependency(syncComputation);
                    }
                    else
                    {
                        dependency = HandleOneWayRTLDependency(syncComputation);
                    }
                    break;
                default:
                    break;
            }
            if (dependency != null) syncComputation.Dependencies.Add(dependency);
            return dependency;
        }

        private IDisposable HandleTwoWayLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Opposite.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                left.Value, right.Value, leftSetter, rightSetter);
            return new TwoWayDependency(left, right, syncComputation, this);
        }

        private IDisposable HandleTwoWayRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Opposite.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                right.Value, left.Value, leftSetter, rightSetter);
            return new TwoWayDependency(left, right, syncComputation.Opposite, this);
        }

        private class TwoWayDependency : IDisposable
        {
            public INotifyReversableValue<TDepLeft> Left { get; private set; }
            public INotifyReversableValue<TDepRight> Right { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public TwoWayDependency(INotifyReversableValue<TDepLeft> left, INotifyReversableValue<TDepRight> right, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
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
                Left.Detach();
                Right.Detach();
            }
        }

        private IDisposable HandleOneWayLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
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
            return new OneWayLTRDependency(input, syncComputation, this);
        }

        private class OneWayLTRDependency : IDisposable
        {
            public INotifyValue<TDepLeft> Left { get; private set; }
            public SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public OneWayLTRDependency(INotifyValue<TDepLeft> left, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
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
                Left.Detach();
            }
        }

        private IDisposable HandleOneWayRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
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
                Right.Detach();
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

        public ITransformationRuleDependency CreateLeftToRightDependency()
        {
            return new LTRDependency(this);
        }

        public ITransformationRuleDependency CreateRightToLeftDependency()
        {
            return new RTLDependency(this);
        }

        public ITransformationRuleDependency CreateLeftToRightOnlyDependency()
        {
            return new LTROnlyDependency(this);
        }

        public ITransformationRuleDependency CreateRightToLeftOnlyDependency()
        {
            return new RTLOnlyDependency(this);
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
                parent.CreateLeftToRightDependency(true, (SynchronizationComputation<TLeft, TRight>)computation);
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
                parent.CreateLeftToRightDependency(false, (SynchronizationComputation<TLeft, TRight>)computation);
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
                parent.CreateRightToLeftDependency(true, (SynchronizationComputation<TRight, TLeft>)computation);
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
                parent.CreateRightToLeftDependency(false, (SynchronizationComputation<TRight, TLeft>)computation);
            }
        }
    }
}
