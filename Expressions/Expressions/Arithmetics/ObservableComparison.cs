using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableGreatherThan<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableGreatherThan(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableGreatherThan(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) > 0;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableGreatherThan<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableGreatherThanOrEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableGreatherThanOrEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableGreatherThanOrEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) >= 0;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableGreatherThanOrEquals<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableLessThan<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableLessThan(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableLessThan(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) < 0;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLessThan<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableLessThanOrEquals<T> : ObservableBinaryExpressionBase<T, T, bool>
    {
        public ObservableLessThanOrEquals(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T>(node.Left), binder.VisitObservable<T>(node.Right)) { }

        public ObservableLessThanOrEquals(INotifyExpression<T> left, INotifyExpression<T> right)
            : base(left, right) { }

        protected override bool GetValue()
        {
            return Comparer<T>.Default.Compare(Left.Value, Right.Value) <= 0;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLessThanOrEquals<T>(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }
}
