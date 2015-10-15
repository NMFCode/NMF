using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return EqualityComparer<T>.Default.Equals(Left.Value, Right.Value);
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableEquals<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableNotEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableNotEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableNotEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return !EqualityComparer<T>.Default.Equals(Left.Value, Right.Value);
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableNotEquals<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }


}
