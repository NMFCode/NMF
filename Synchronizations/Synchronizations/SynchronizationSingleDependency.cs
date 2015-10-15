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

        public void HandleLeftToRightDependency(Computation computation, bool allowRightToLeft)
        {
            var syncComputation = computation as SynchronizationComputation<TLeft, TRight>;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticLTRDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    HandleOneWayLTRDependency(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowRightToLeft)
                    {
                        HandleTwoWayLTRDependency(syncComputation);
                    }
                    else
                    {
                        HandleOneWayLTRDependency(syncComputation);
                    }
                    break;
                default:
                    break;
            }
        }

        public void HandleRightToLeftDependency(Computation computation, bool allowLeftToRight)
        {
            var syncComputation = computation as SynchronizationComputation<TRight, TLeft>;
            switch (syncComputation.SynchronizationContext.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    HandleStaticRTLDependency(syncComputation);
                    break;
                case ChangePropagationMode.OneWay:
                    HandleOneWayRTLDependency(syncComputation);
                    break;
                case ChangePropagationMode.TwoWay:
                    if (allowLeftToRight)
                    {
                        HandleTwoWayRTLDependency(syncComputation);
                    }
                    else
                    {
                        HandleOneWayRTLDependency(syncComputation);
                    }
                    break;
                default:
                    break;
            }
        }

        private void HandleTwoWayLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Opposite.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallLTRTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                left.Value, right.Value, leftSetter, rightSetter);
            left.ValueChanged += (o, e) => CallLTRTransformationForInput(syncComputation, SynchronizationDirection.LeftToRightForced,
                left.Value, right.Value, leftSetter, rightSetter);
            right.ValueChanged += (o, e) => CallRTLTransformationForInput(syncComputation.Opposite, SynchronizationDirection.RightToLeftForced,
                right.Value, left.Value, leftSetter, rightSetter);
        }

        private void HandleTwoWayRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var left = leftFunc.InvokeReversable(syncComputation.Opposite.Input);
            var right = rightFunc.InvokeReversable(syncComputation.Input);
            Action<TLeft, TDepLeft> leftSetter = (l, val) => left.Value = val;
            Action<TRight, TDepRight> rightSetter = (r, val) => right.Value = val;
            CallRTLTransformationForInput(syncComputation, syncComputation.SynchronizationContext.Direction,
                right.Value, left.Value, leftSetter, rightSetter);
            right.ValueChanged += (o, e) => CallRTLTransformationForInput(syncComputation, SynchronizationDirection.RightToLeftForced,
                right.Value, left.Value, leftSetter, rightSetter);
            left.ValueChanged += (o, e) => CallLTRTransformationForInput(syncComputation.Opposite, SynchronizationDirection.LeftToRightForced,
                left.Value, right.Value, leftSetter, rightSetter);
        }

        private void HandleOneWayLTRDependency(SynchronizationComputation<TLeft, TRight> syncComputation)
        {
            var input = leftFunc.Observe(syncComputation.Input);
            var target = rightFunc.InvokeReversable(syncComputation.Opposite.Input);
            Action<TRight, TDepRight> rightSetter = (right, val) => target.Value = val;
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
                input.Value, target.Value, leftSetter, rightSetter);
            input.ValueChanged += (o, e) => CallLTRTransformationForInput(syncComputation,
                SynchronizationDirection.LeftToRightForced, input.Value, target.Value, leftSetter, rightSetter);
        }

        private void HandleOneWayRTLDependency(SynchronizationComputation<TRight, TLeft> syncComputation)
        {
            var input = rightFunc.Observe(syncComputation.Input);
            var target = leftFunc.InvokeReversable(syncComputation.Opposite.Input);
            Action<TLeft, TDepLeft> leftSetter = (left, val) => target.Value = val;
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
                input.Value, target.Value, leftSetter, rightSetter);
            input.ValueChanged += (o, e) => CallRTLTransformationForInput(syncComputation, SynchronizationDirection.RightToLeftForced,
                input.Value, target.Value, leftSetter, rightSetter);
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
                parent.HandleLeftToRightDependency(computation, true);
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
                parent.HandleLeftToRightDependency(computation, false);
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
                parent.HandleRightToLeftDependency(computation, true);
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
                parent.HandleRightToLeftDependency(computation, false);
            }
        }
    }
}
