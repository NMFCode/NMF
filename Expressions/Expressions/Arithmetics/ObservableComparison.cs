using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableGreatherThan<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} > {1})";
            }
        }

        public ObservableGreatherThan(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableGreatherThan(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.GreaterThan; } }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) > 0;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableGreatherThan<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableGreatherThanOrEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} >= {1})";
            }
        }

        public ObservableGreatherThanOrEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableGreatherThanOrEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.GreaterThanOrEqual; } }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) >= 0;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableGreatherThanOrEquals<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLessThan<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} < {1})";
            }
        }

        public ObservableLessThan(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableLessThan(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.LessThan; } }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) < 0;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLessThan<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLessThanOrEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} <= {1})";
            }
        }

        public ObservableLessThanOrEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableLessThanOrEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.LessThanOrEqual; } }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) <= 0;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLessThanOrEquals<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
