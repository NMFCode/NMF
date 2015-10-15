using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntBitwiseAnd : ObservableBinaryExpressionBase<int, int, int>
    {
        public ObservableIntBitwiseAnd(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value & Right.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableIntBitwiseAnd(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableUIntBitwiseAnd : ObservableBinaryExpressionBase<uint, uint, uint>
    {
        public ObservableUIntBitwiseAnd(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value & Right.Value;
        }

        public override INotifyExpression<uint> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUIntBitwiseAnd(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableLongBitwiseAnd : ObservableBinaryExpressionBase<long, long, long>
    {
        public ObservableLongBitwiseAnd(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value & Right.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLongBitwiseAnd(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableULongBitwiseAnd : ObservableBinaryExpressionBase<ulong, ulong, ulong>
    {
        public ObservableULongBitwiseAnd(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value & Right.Value;
        }

        public override INotifyExpression<ulong> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableULongBitwiseAnd(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }
}
