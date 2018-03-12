using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableIntArrayIndex<T> : ObservableBinaryExpressionBase<T[], int, T>
    {
        protected override string Format
        {
            get
            {
                return "{0}[{1}]";
            }
        }

        public ObservableIntArrayIndex(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T[]>(node.Left), binder.VisitObservable<int>(node.Right)) { }

        public ObservableIntArrayIndex(INotifyExpression<T[]> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override T GetValue()
        {
            return Left.Value[Right.Value];
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntArrayIndex<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLongArrayIndex<T> : ObservableBinaryExpressionBase<T[], long, T>
    {
        protected override string Format
        {
            get
            {
                return "{0}[{1}]";
            }
        }

        public ObservableLongArrayIndex(BinaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<T[]>(node.Left), binder.VisitObservable<long>(node.Right)) { }

        public ObservableLongArrayIndex(INotifyExpression<T[]> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override T GetValue()
        {
            return Left.Value[Right.Value];
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongArrayIndex<T>(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
