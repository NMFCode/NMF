using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{

    internal class ObservableIntRightShift : ObservableBinaryExpressionBase<int, int, int>
    {
        public ObservableIntRightShift(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value >> Right.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableIntRightShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableIntLeftShift : ObservableBinaryExpressionBase<int, int, int>
    {
        public ObservableIntLeftShift(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value << Right.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableIntLeftShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableUIntRightShift : ObservableBinaryExpressionBase<uint, int, uint>
    {
        public ObservableUIntRightShift(INotifyExpression<uint> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value >> Right.Value;
        }

        public override INotifyExpression<uint> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUIntRightShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableUIntLeftShift : ObservableBinaryExpressionBase<uint, int, uint>
    {
        public ObservableUIntLeftShift(INotifyExpression<uint> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value << Right.Value;
        }

        public override INotifyExpression<uint> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUIntLeftShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }


    internal class ObservableLongRightShift : ObservableBinaryExpressionBase<long, int, long>
    {
        public ObservableLongRightShift(INotifyExpression<long> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value >> Right.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLongRightShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableLongLeftShift : ObservableBinaryExpressionBase<long, int, long>
    {
        public ObservableLongLeftShift(INotifyExpression<long> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value << Right.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLongLeftShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableULongRightShift : ObservableBinaryExpressionBase<ulong, int, ulong>
    {
        public ObservableULongRightShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value >> Right.Value;
        }

        public override INotifyExpression<ulong> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableULongRightShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableULongLeftShift : ObservableBinaryExpressionBase<ulong, int, ulong>
    {
        public ObservableULongLeftShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value << Right.Value;
        }

        public override INotifyExpression<ulong> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableULongLeftShift(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }
}
