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

        private void PerformTwoWay(TLeft left, TRight right, ISynchronizationContext context, Queue<IDisposable> dependencies)
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
            dependencies.Enqueue(new BidirectionalPropertySynchronization<TValue>(leftEx3, rightEx3));
        }

        private void PerformOneWay(TLeft left, TRight right, ISynchronizationContext context, Queue<IDisposable> dependencies)
        {
            switch (context.Direction)
            {
                case SynchronizationDirection.LeftToRight:
                case SynchronizationDirection.LeftToRightForced:
                    var leftEx1 = leftFunc.Observe(left);
                    rightSetter(right, leftEx1.Value);
                    dependencies.Enqueue(new PropertySynchronization<TValue>(leftEx1, val => rightSetter(right, val)));
                    break;
                case SynchronizationDirection.RightToLeft:
                case SynchronizationDirection.RightToLeftForced:
                    var rightEx2 = rightFunc.Observe(right);
                    leftSetter(left, rightEx2.Value);
                    dependencies.Enqueue(new PropertySynchronization<TValue>(rightEx2, val => leftSetter(left, val)));
                    break;
                case SynchronizationDirection.LeftWins:
                case SynchronizationDirection.RightWins:
                    TValue leftVal;
                    TValue rightVal;
                    if (context.Direction == SynchronizationDirection.LeftWins)
                    {
                        var leftEx4 = leftFunc.Observe(left);
                        leftVal = leftEx4.Value;
                        rightVal = rightGetter(right);
                        dependencies.Enqueue(new PropertySynchronization<TValue>(leftEx4, val => rightSetter(right, val)));
                    }
                    else
                    {
                        var rightEx4 = rightFunc.Observe(right);
                        leftVal = leftGetter(left);
                        rightVal = rightEx4.Value;
                        dependencies.Enqueue(new PropertySynchronization<TValue>(rightEx4, val => leftSetter(left, val)));
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

        public void Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            var left = computation.Input;
            var right = computation.Opposite.Input;
            switch (context.ChangePropagation)
            {
                case NMF.Transformations.ChangePropagationMode.None:
                    PerformNoChangePropagation(left, right, direction);
                    break;
                case NMF.Transformations.ChangePropagationMode.OneWay:
                    PerformOneWay(left, right, context, computation.Dependencies);
                    break;
                case NMF.Transformations.ChangePropagationMode.TwoWay:
                    PerformTwoWay(left, right, context, computation.Dependencies);
                    return;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    internal class PropertySynchronization<T> : IDisposable
    {
        public INotifyValue<T> Source { get; private set; }
        public Action<T> Target { get; private set; }

        public PropertySynchronization(INotifyValue<T> source, Action<T> target)
        {
            Source = source;
            Target = target;

            Source.ValueChanged += Source_ValueChanged;
        }

        private void Source_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Target(Source.Value);
        }

        public void Dispose()
        {
            Source.ValueChanged -= Source_ValueChanged;
        }
    }

    internal class BidirectionalPropertySynchronization<T> : IDisposable
    {
        public INotifyReversableValue<T> Source1 { get; private set; }
        public INotifyReversableValue<T> Source2 { get; private set; }

        public BidirectionalPropertySynchronization(INotifyReversableValue<T> source1, INotifyReversableValue<T> source2)
        {
            Source1 = source1;
            Source2 = source2;

            Source1.ValueChanged += Source1_ValueChanged;
            Source2.ValueChanged += Source2_ValueChanged;
        }

        private void Source2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Source1.Value = Source2.Value;
        }

        private void Source1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Source2.Value = Source1.Value;
        }

        public void Dispose()
        {
            Source1.ValueChanged -= Source1_ValueChanged;
            Source2.ValueChanged -= Source2_ValueChanged;
        }
    }
}
