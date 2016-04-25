using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableBinaryExpressionBase<TLeft, TRight, TResult> : NotifyExpression<TResult>
    {
        public ObservableBinaryExpressionBase(INotifyExpression<TLeft> left, INotifyExpression<TRight> right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            Left = left;
            Right = right;

            Right.ValueChanged += RightChanged;
            Left.ValueChanged += LeftChanged;
        }

        protected virtual void RightChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        protected virtual void LeftChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public override bool CanBeConstant
        {
            get
            {
                return Left.IsConstant && Right.IsConstant;
            }
        }

        public INotifyExpression<TLeft> Left { get; private set; }

        public INotifyExpression<TRight> Right { get; private set; }

        protected override void DetachCore()
        {
            Left.Detach();
            Right.Detach();
        }

        protected override void AttachCore()
        {
            Left.Attach();
            Right.Attach();
        }

        public override bool IsParameterFree
        {
            get { return Left.IsParameterFree && Right.IsParameterFree; }
        }
    }

    internal abstract class ObservableReversableBinaryExpressionBase<TLeft, TRight, TResult> : ObservableBinaryExpressionBase<TLeft, TRight, TResult>, INotifyReversableExpression<TResult>
    {
        protected ObservableReversableBinaryExpressionBase(INotifyExpression<TLeft> left, INotifyExpression<TRight> right)
            : base(left, right) { }

        public new TResult Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (!EqualityComparer<TResult>.Default.Equals(Value, value))
                {
                    var leftReversable = Left as INotifyReversableExpression<TLeft>;
                    if (leftReversable != null && leftReversable.IsReversable && Right.CanBeConstant)
                    {
                        SetLeftValue(leftReversable, Right.Value, value);
                        return;
                    }
                    var rightReversable = Right as INotifyReversableExpression<TRight>;
                    if (rightReversable != null && rightReversable.IsReversable && Right.CanBeConstant)
                    {
                        SetRightValue(rightReversable, Left.Value, value);
                        return;
                    }
                    throw new InvalidOperationException("Binary expressions can only be reversed if one operand is reversable and the other one is constant.");
                }
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        protected abstract void SetLeftValue(INotifyReversableExpression<TLeft> left, TRight right, TResult result);

        protected abstract void SetRightValue(INotifyReversableExpression<TRight> right, TLeft left, TResult result);

        public bool IsReversable
        {
            get
            {
                if (IsAttached)
                {
                    var leftReversable = Left as INotifyReversableExpression<TLeft>;
                    if (leftReversable != null)
                    {
                        return leftReversable.CanBeConstant && Right.CanBeConstant;
                    }
                    var rightReversable = Right as INotifyReversableExpression<TRight>;
                    if (rightReversable != null)
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

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableBinaryExpression<TLeft, TRight, TResult>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters), Implementation);
        }
    }

    internal sealed class ObservableReversableBinaryExpression<TLeft, TRight, TResult> : ObservableReversableBinaryExpressionBase<TLeft, TRight, TResult>
    {
        private Func<TResult, TRight, TLeft> rightReverser;
        private Func<TResult, TLeft, TRight> leftReverser;

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

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableReversableBinaryExpression<TLeft, TRight, TResult>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters), Implementation, rightReverser, leftReverser);
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

    internal sealed class ObservableCoalesceExpression<T> : ObservableBinaryExpressionBase<T, T, T>
        where T : class
    {
        public ObservableCoalesceExpression(BinaryExpression expression, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(expression.Left), binder.VisitObservable<T>(expression.Right)) { }

        public ObservableCoalesceExpression(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }


        protected override T GetValue()
        {
            return Left.Value ?? Right.Value;
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableCoalesceExpression<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }
}
