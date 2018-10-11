using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntBitwiseAnd : ObservableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} & {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.And;

        public ObservableIntBitwiseAnd(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value & Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntBitwiseAnd(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableUIntBitwiseAnd : ObservableBinaryExpressionBase<uint, uint, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} & {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.And;

        public ObservableUIntBitwiseAnd(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value & Right.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntBitwiseAnd(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLongBitwiseAnd : ObservableBinaryExpressionBase<long, long, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} & {1})";
            }
        }
        public ObservableLongBitwiseAnd(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value & Right.Value;
        }


        public override ExpressionType NodeType => ExpressionType.And;

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongBitwiseAnd(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableULongBitwiseAnd : ObservableBinaryExpressionBase<ulong, ulong, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} & {1})";
            }
        }

        public ObservableULongBitwiseAnd(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value & Right.Value;
        }


        public override ExpressionType NodeType => ExpressionType.And;

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongBitwiseAnd(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
