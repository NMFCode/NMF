using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a synchronization job to synchronize properties
    /// </summary>
    /// <typeparam name="TLeft">The LHS type of elements</typeparam>
    /// <typeparam name="TRight">The RHS type of elements</typeparam>
    /// <typeparam name="TValue">The value type of the property synchronization</typeparam>
    internal class PropertySynchronizationJob<TLeft, TRight, TValue> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private ObservingFunc<TLeft, TValue> leftFunc;
        private ObservingFunc<TRight, TValue> rightFunc;

        private Func<TLeft, TValue> leftGetter;
        private Func<TRight, TValue> rightGetter;
        private Action<TLeft, TValue> leftSetter;
        private Action<TRight, TValue> rightSetter;

        private bool isEarly;

        /// <summary>
        /// Creates a new property synchronization job
        /// </summary>
        /// <param name="leftSelector">The LHS selector</param>
        /// <param name="rightSelector">The RHS selector</param>
        /// <param name="isEarly">TRue, if the property synchronization should be executed immediately when the correspondence is established, otherwise false</param>
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

        /// <inheritdoc />
        public bool IsEarly
        {
            get
            {
                return isEarly;
            }
        }

        private IDisposable PerformTwoWay(TLeft left, TRight right, ISynchronizationContext context)
        {
            var leftEx3 = leftFunc.InvokeReversable(left);
            leftEx3.Successors.SetDummy();
            var rightEx3 = rightFunc.InvokeReversable(right);
            rightEx3.Successors.SetDummy();
            switch (context.Direction)
            {
                case SynchronizationDirection.CheckOnly:
                    return new IncrementalPropertyConsistencyCheck<TValue>(leftEx3, rightEx3, context);
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    rightEx3.Value = leftEx3.Value;
                    break;
                case SynchronizationDirection.LeftWins:
                    if (leftEx3.Value != null)
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
                    if (rightEx3.Value != null)
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
            var dependency = new BidirectionalPropertySynchronization<TValue>(leftEx3, rightEx3);
            return dependency;
        }

        private IDisposable PerformOneWay(TLeft left, TRight right, ISynchronizationContext context)
        {
            IDisposable dependency = null;
            switch (context.Direction)
            {
                case SynchronizationDirection.CheckOnly:
                    throw new NotSupportedException("Check only is not supported for partial change propagation.");
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    var leftEx1 = leftFunc.Observe(left);
                    leftEx1.Successors.SetDummy();
                    rightSetter(right, leftEx1.Value);
                    dependency = new PropertySynchronization<TValue>(leftEx1, val => rightSetter(right, val));
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                    var rightEx2 = rightFunc.Observe(right);
                    rightEx2.Successors.SetDummy();
                    leftSetter(left, rightEx2.Value);
                    dependency = new PropertySynchronization<TValue>(rightEx2, val => leftSetter(left, val));
                    break;
                case SynchronizationDirection.LeftWins:
                case SynchronizationDirection.RightWins:
                    TValue leftVal;
                    TValue rightVal;
                    if (context.Direction == SynchronizationDirection.LeftWins)
                    {
                        var leftEx4 = leftFunc.Observe(left);
                        leftEx4.Successors.SetDummy();
                        leftVal = leftEx4.Value;
                        rightVal = rightGetter(right);
                        dependency = new PropertySynchronization<TValue>(leftEx4, val => rightSetter(right, val));
                    }
                    else
                    {
                        var rightEx4 = rightFunc.Observe(right);
                        rightEx4.Successors.SetDummy();
                        leftVal = leftGetter(left);
                        rightVal = rightEx4.Value;
                        dependency = new PropertySynchronization<TValue>(rightEx4, val => leftSetter(left, val));
                    }
                    var test = context.Direction == SynchronizationDirection.LeftWins ?
                        typeof(TValue).IsValueType || leftVal != null :
                        !(typeof(TValue).IsValueType || rightVal != null);
                    if (test)
                    {
                        rightSetter(right, leftVal);
                    }
                    else
                    {
                        leftSetter(left, rightVal);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return dependency;
        }

        private void PerformNoChangePropagation(TLeft left, TRight right, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (direction)
            {
                case SynchronizationDirection.CheckOnly:
                    var leftValue = leftGetter(left);
                    var rightValue = rightGetter(right);
                    if (!EqualityComparer<TValue>.Default.Equals(leftValue, rightValue))
                    {
                        context.Inconsistencies.Add(new PropertyInequality<TLeft, TRight, TValue>(left, leftSetter, leftValue, right, rightSetter, rightValue));
                    }
                    break;
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

        /// <inheritdoc />
        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            var left = computation.Input;
            var right = computation.Opposite.Input;
            switch (context.ChangePropagation)
            {
                case Transformations.ChangePropagationMode.None:
                    PerformNoChangePropagation(left, right, direction, context);
                    return null;
                case Transformations.ChangePropagationMode.OneWay:
                    return PerformOneWay(left, right, context);
                case Transformations.ChangePropagationMode.TwoWay:
                    return PerformTwoWay(left, right, context);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
