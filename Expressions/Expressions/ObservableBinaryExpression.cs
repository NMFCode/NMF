using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    internal abstract class ObservableBinaryExpressionBase<TLeft, TRight, TResult> : NotifyExpression<TResult>
    {
        public ObservableBinaryExpressionBase(INotifyExpression<TLeft> left, INotifyExpression<TRight> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            Left = left;
            Right = right;
        }

        protected abstract string Format { get; }

        public string Expression => string.Format(Format, Left.ToString(), Right.ToString()) + "{" + (Value != null ? Value.ToString() : "(null)") + "}";

        public override bool CanBeConstant
        {
            get
            {
                return Left.IsConstant && Right.IsConstant;
            }
        }

        public INotifyExpression<TLeft> Left { get; private set; }

        public INotifyExpression<TRight> Right { get; private set; }

        public override bool IsParameterFree
        {
            get { return Left.IsParameterFree && Right.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Left;
                yield return Right;
            }
        }
    }

    internal abstract class ObservableReversableBinaryExpressionBase<TLeft, TRight, TResult> : ObservableBinaryExpressionBase<TLeft, TRight, TResult>, INotifyReversableExpression<TResult>
    {
        protected ObservableReversableBinaryExpressionBase(INotifyExpression<TLeft> left, INotifyExpression<TRight> right)
            : base(left, right) { }

        public new TResult Value
        {
#pragma warning disable S4275 // Getters and setters should access the expected fields
            get
            {
                return base.Value;
            }
            set
#pragma warning restore S4275 // Getters and setters should access the expected fields
            {
                if (!EqualityComparer<TResult>.Default.Equals(Value, value))
                {
                    if(Left is INotifyReversableExpression<TLeft> leftReversable && leftReversable.IsReversable && Right.CanBeConstant)
                    {
                        SetLeftValue( leftReversable, Right.Value, value );
                        return;
                    }
                    if(Right is INotifyReversableExpression<TRight> rightReversable && rightReversable.IsReversable && Left.CanBeConstant)
                    {
                        SetRightValue( rightReversable, Left.Value, value );
                        return;
                    }
                    throw new InvalidOperationException("Binary expressions can only be reversed if one operand is reversable and the other one is constant.");
                }
            }
        }

        protected abstract void SetLeftValue(INotifyReversableExpression<TLeft> left, TRight right, TResult result);

        protected abstract void SetRightValue(INotifyReversableExpression<TRight> right, TLeft left, TResult result);

        public bool IsReversable
        {
            get
            {
                if (Successors.HasSuccessors)
                {
                    if(Left is INotifyReversableExpression<TLeft> leftReversable)
                    {
                        return leftReversable.CanBeConstant && Right.CanBeConstant;
                    }
                    if(Right is INotifyReversableExpression<TRight> rightReversable)
                    {
                        return rightReversable.CanBeConstant && Left.CanBeConstant;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    internal sealed class ObservableBinaryExpression<TLeft, TRight, TResult> : ObservableBinaryExpressionBase<TLeft, TRight, TResult>
    {
        protected override string Format
        {
            get
            {
                return Implementation.ToString() + "({0}, {1})";
            }
        }

        public ObservableBinaryExpression(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<TLeft>(node.Left), binder.VisitObservable<TRight>(node.Right), ReflectionHelper.CreateDelegate<Func<TLeft, TRight, TResult>>(node.Method)) { }

        public ObservableBinaryExpression(INotifyExpression<TLeft> left, INotifyExpression<TRight> right, Func<TLeft, TRight, TResult> implementation)
            : base(left, right)
        {
            Implementation = implementation;
        }

        public Func<TLeft, TRight, TResult> Implementation { get; private set; }

        protected override TResult GetValue()
        {
            return Implementation(Left.Value, Right.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableBinaryExpression<TLeft, TRight, TResult>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace), Implementation);
        }
    }

    internal sealed class ObservableReversableBinaryExpression<TLeft, TRight, TResult> : ObservableReversableBinaryExpressionBase<TLeft, TRight, TResult>
    {
        protected override string Format
        {
            get
            {
                return Implementation.ToString() + "({0}, {1})";
            }
        }

        private readonly Func<TResult, TRight, TLeft> rightReverser;
        private readonly Func<TResult, TLeft, TRight> leftReverser;

        public ObservableReversableBinaryExpression(BinaryExpression node, ObservableExpressionBinder binder, MethodInfo leftReverser, MethodInfo rightReverser)
            : this(
                binder.VisitObservable<TLeft>(node.Left),
                binder.VisitObservable<TRight>(node.Right),
                ReflectionHelper.CreateDelegate<Func<TLeft, TRight, TResult>>(node.Method),
                rightReverser != null ? ReflectionHelper.CreateDelegate<Func<TResult, TRight, TLeft>>(rightReverser) : null,
                leftReverser != null ? ReflectionHelper.CreateDelegate<Func<TResult, TLeft, TRight>>(leftReverser) : null) { }

        public ObservableReversableBinaryExpression(INotifyExpression<TLeft> left,
            INotifyExpression<TRight> right,
            Func<TLeft, TRight, TResult> implementation,
            Func<TResult, TRight, TLeft> rightReverser,
            Func<TResult, TLeft, TRight> leftReverser)
            : base(left, right)
        {
            Implementation = implementation;

            this.leftReverser = leftReverser;
            this.rightReverser = rightReverser;
        }

        public Func<TLeft, TRight, TResult> Implementation { get; private set; }

        protected override TResult GetValue()
        {
            return Implementation(Left.Value, Right.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableReversableBinaryExpression<TLeft, TRight, TResult>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace), Implementation, rightReverser, leftReverser);
        }

        protected override void SetLeftValue(INotifyReversableExpression<TLeft> left, TRight right, TResult result)
        {
            if (rightReverser != null)
            {
                left.Value = rightReverser(result, right);
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<TRight> right, TLeft left, TResult result)
        {
            if (leftReverser != null)
            {
                right.Value = leftReverser(result, left);
            }
        }
    }

    internal sealed class ObservableReversableBinaryExpression2<TLeft, TRight, TResult> : ObservableReversableBinaryExpressionBase<TLeft, TRight, TResult>
    {
        protected override string Format
        {
            get
            {
                return Implementation.ToString() + "({0}, {1})";
            }
        }

        private readonly Func<TRight, TResult, TLeft> rightReverser;
        private readonly Func<TLeft, TResult, TRight> leftReverser;

        public ObservableReversableBinaryExpression2(BinaryExpression node, ObservableExpressionBinder binder, MethodInfo leftReverser, MethodInfo rightReverser)
            : this(
                binder.VisitObservable<TLeft>(node.Left),
                binder.VisitObservable<TRight>(node.Right),
                ReflectionHelper.CreateDelegate<Func<TLeft, TRight, TResult>>(node.Method),
                rightReverser != null ? ReflectionHelper.CreateDelegate<Func<TRight, TResult, TLeft>>(rightReverser) : null,
                leftReverser != null ? ReflectionHelper.CreateDelegate<Func<TLeft, TResult, TRight>>(leftReverser) : null)
        { }

        public ObservableReversableBinaryExpression2(INotifyExpression<TLeft> left,
            INotifyExpression<TRight> right,
            Func<TLeft, TRight, TResult> implementation,
            Func<TRight, TResult, TLeft> rightReverser,
            Func<TLeft, TResult, TRight> leftReverser)
            : base(left, right)
        {
            Implementation = implementation;

            this.leftReverser = leftReverser;
            this.rightReverser = rightReverser;
        }

        public Func<TLeft, TRight, TResult> Implementation { get; private set; }

        protected override TResult GetValue()
        {
            return Implementation(Left.Value, Right.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableReversableBinaryExpression2<TLeft, TRight, TResult>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace), Implementation, rightReverser, leftReverser);
        }

        protected override void SetLeftValue(INotifyReversableExpression<TLeft> left, TRight right, TResult result)
        {
            if (rightReverser != null)
            {
                left.Value = rightReverser(right, result);
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<TRight> right, TLeft left, TResult result)
        {
            if (leftReverser != null)
            {
                right.Value = leftReverser(left, result);
            }
        }
    }

    internal class ObservableNullableCoalesceExpression<T> : ObservableBinaryExpressionBase<T?, T, T>
        where T : struct
    {
        protected override string Format
        {
            get
            {
                return "({0} ?? {1})";
            }
        }

        public ObservableNullableCoalesceExpression(BinaryExpression expression, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T?>(expression.Left), binder.VisitObservable<T>(expression.Right)) { }

        public ObservableNullableCoalesceExpression(INotifyExpression<T?> left, INotifyExpression<T> right)
            : base(left, right) { }


        protected override T GetValue()
        {
            return Left.Value ?? Right.Value;
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableNullableCoalesceExpression<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }

    }

    internal class ObservableCoalesceExpression<T> : ObservableBinaryExpressionBase<T, T, T>
        where T : class
    {
        protected override string Format
        {
            get
            {
                return "({0} ?? {1})";
            }
        }

        public ObservableCoalesceExpression(BinaryExpression expression, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(expression.Left), binder.VisitObservable<T>(expression.Right)) { }

        public ObservableCoalesceExpression(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }


        protected override T GetValue()
        {
            return Left.Value ?? Right.Value;
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableCoalesceExpression<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableReversableCoalesceExpression<T> : ObservableCoalesceExpression<T>, INotifyReversableExpression<T> where T : class
    {
        public ObservableReversableCoalesceExpression(INotifyReversableExpression<T> left, INotifyReversableExpression<T> right)
            : base(left, right) { }

        public bool IsReversable
        {
            get
            {
                return ((INotifyReversableExpression<T>)Left).IsReversable || ((INotifyReversableExpression<T>)Right).IsReversable;
            }
        }

        T INotifyReversableValue<T>.Value
        {
            get => Value; set
            {
                try
                {
                    ((INotifyReversableExpression<T>)Left).Value = value;
                }
                catch
                {
                    ((INotifyReversableExpression<T>)Right).Value = value;
                }
            }
        }
    }
}
