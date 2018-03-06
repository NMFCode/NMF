using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} == {1})";
            }
        }

        public ObservableEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return EqualityComparer<T>.Default.Equals(Left.Value, Right.Value);
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableEquals<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Equal;
            }
        }
    }

    internal class ObservableNotEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} != {1})";
            }
        }

        public ObservableNotEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableNotEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return !EqualityComparer<T>.Default.Equals(Left.Value, Right.Value);
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableNotEquals<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.NotEqual;
            }
        }
    }


}
