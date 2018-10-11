using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntBitwiseOr : ObservableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} | {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Or;

        public ObservableIntBitwiseOr(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value | Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntBitwiseOr(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableUIntBitwiseOr : ObservableBinaryExpressionBase<uint, uint, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} | {1})";
            }
        }

        public ObservableUIntBitwiseOr(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value | Right.Value;
        }

        public override ExpressionType NodeType => ExpressionType.Or;

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntBitwiseOr(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLongBitwiseOr : ObservableBinaryExpressionBase<long, long, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} | {1})";
            }
        }

        public ObservableLongBitwiseOr(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value | Right.Value;
        }

        public override ExpressionType NodeType => ExpressionType.Or;

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongBitwiseOr(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableULongBitwiseOr : ObservableBinaryExpressionBase<ulong, ulong, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} | {1})";
            }
        }

        public ObservableULongBitwiseOr(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value | Right.Value;
        }

        public override ExpressionType NodeType => ExpressionType.Or;

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongBitwiseOr(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
