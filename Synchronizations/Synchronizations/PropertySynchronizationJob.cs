using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public class PropertySynchronizationJob<TLeft, TRight, TValue> : ISynchronizationJob<TLeft, TRight>
    {
        private ObservingFunc<TLeft, TValue> leftFunc;
        private ObservingFunc<TRight, TValue> rightFunc;

        private Func<TLeft, TValue> leftGetter;
        private Func<TRight, TValue> rightGetter;
        private Action<TLeft, TValue> leftSetter;
        private Action<TRight, TValue> rightSetter;

        private bool isEarly;

        public PropertySynchronizationJob(Expression<Func<TLeft, TValue>> leftSelector, Expression<Func<TRight, TValue>> rightSelector, bool isEarly)
        {
            if (leftSelector == null) throw new ArgumentNullException("leftSelector");
            if (rightSelector == null) throw new ArgumentNullException("rightSelector");

            leftFunc = new ObservingFunc<TLeft, TValue>(leftSelector);
            rightFunc = new ObservingFunc<TRight, TValue>(rightSelector);

            leftGetter = leftSelector.Compile();
            rightGetter = rightSelector.Compile();

            var leftSetterExpression = SetExpressionRewriter.CreateSetter(leftSelector);
            if (leftSetterExpression != null)
            {
                leftSetter = leftSetterExpression.Compile();
            }
            else
            {
                throw new ArgumentException("The expression is read-only", "leftSelector");
            }
            var rightSetterExpression = SetExpressionRewriter.CreateSetter(rightSelector);
            if (rightSetterExpression != null)
            {
                rightSetter = rightSetterExpression.Compile();
            }
            else
            {
                throw new ArgumentException("The expression is read-only", "rightSelector");
            }

            this.isEarly = isEarly;
        }

        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }

        public void Perform(TLeft left, TRight right, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case NMF.Transformations.ChangePropagationMode.None:
                    PerformNoChangePropagation(left, right, direction);
                    break;
                case NMF.Transformations.ChangePropagationMode.OneWay:
                    PerformOneWay(left, right, context);
                    break;
                case NMF.Transformations.ChangePropagationMode.TwoWay:
                    PerformTwoWay(left, right, context);
                    return;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void PerformTwoWay(TLeft left, TRight right, ISynchronizationContext context)
        {
            var leftEx3 = leftFunc.InvokeReversable(left);
            var rightEx3 = rightFunc.InvokeReversable(right);
            switch (context.Direction)
            {
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    rightEx3.Value = leftEx3.Value;
                    break;
                case SynchronizationDirection.LeftWins:
                    if (typeof(TValue).IsValueType || leftEx3.Value != null)
                    {
                        rightEx3.Value = leftEx3.Value;
                    }
                    else
                    {
                        leftEx3.Value = rightEx3.Value;
                    }
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                    leftEx3.Value = rightEx3.Value;
                    break;
                case SynchronizationDirection.RightWins:
                    if (typeof(TValue).IsValueType || rightEx3.Value != null)
                    {
                        leftEx3.Value = rightEx3.Value;
                    }
                    else
                    {
                        rightEx3.Value = leftEx3.Value;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            leftEx3.ValueChanged += (o, e) => rightEx3.Value = leftEx3.Value;
            rightEx3.ValueChanged += (o, e) => leftEx3.Value = rightEx3.Value;
        }

        private void PerformOneWay(TLeft left, TRight right, ISynchronizationContext context)
        {
            switch (context.Direction)
            {
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    var leftEx1 = leftFunc.Observe(left);
                    var rightEx1 = rightFunc.InvokeReversable(right);
                    rightEx1.Value = leftEx1.Value;
                    leftEx1.ValueChanged += (o, e) => rightEx1.Value = leftEx1.Value;
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                    var leftEx2 = leftFunc.InvokeReversable(left);
                    var rightEx2 = rightFunc.Observe(right);
                    leftEx2.Value = rightEx2.Value;
                    rightEx2.ValueChanged += (o, e) => leftEx2.Value = rightEx2.Value;
                    break;
                case SynchronizationDirection.LeftWins:
                case SynchronizationDirection.RightWins:
                    var leftEx4 = leftFunc.InvokeReversable(left);
                    var rightEx4 = rightFunc.InvokeReversable(right);
                    var test = context.Direction == SynchronizationDirection.LeftWins ?
                        typeof(TValue).IsValueType || leftEx4.Value != null :
                        !typeof(TValue).IsValueType && rightEx4.Value == null;
                    if (test)
                    {
                        rightEx4.Value = leftEx4.Value;
                    }
                    else
                    {
                        leftEx4.Value = rightEx4.Value;
                    }
                    if (context.Direction == SynchronizationDirection.LeftWins)
                    {
                        leftEx4.ValueChanged += (o, e) => rightEx4.Value = leftEx4.Value;
                    }
                    else
                    {
                        rightEx4.ValueChanged += (o, e) => leftEx4.Value = rightEx4.Value;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void PerformNoChangePropagation(TLeft left, TRight right, SynchronizationDirection direction)
        {
            switch (direction)
            {
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    rightSetter(right, leftGetter(left));
                    break;
                case SynchronizationDirection.LeftWins:
                    var val1 = leftGetter(left);
                    if (val1 != null)
                    {
                        rightSetter(right, val1);
                    }
                    else
                    {
                        leftSetter(left, rightGetter(right));
                    }
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                    leftSetter(left, rightGetter(right));
                    break;
                case SynchronizationDirection.RightWins:
                    var val2 = rightGetter(right);
                    if (val2 != null)
                    {
                        leftSetter(left, val2);
                    }
                    else
                    {
                        rightSetter(right, leftGetter(left));
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
