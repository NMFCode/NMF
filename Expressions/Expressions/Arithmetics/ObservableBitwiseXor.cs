using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntBitwiseXor : ObservableReversableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} ^ {1})";
            }
        }

        public ObservableIntBitwiseXor(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableIntBitwiseXor(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<int> left, int right, int result)
        {
            left.Value = right ^ result;
        }

        protected override void SetRightValue(INotifyReversableExpression<int> right, int left, int result)
        {
            right.Value = left ^ result;
        }
    }

    internal class ObservableUIntBitwiseXor : ObservableReversableBinaryExpressionBase<uint, uint, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} ^ {1})";
            }
        }

        public ObservableUIntBitwiseXor(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        public override INotifyExpression<uint> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUIntBitwiseXor(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<uint> left, uint right, uint result)
        {
            left.Value = right ^ result;
        }

        protected override void SetRightValue(INotifyReversableExpression<uint> right, uint left, uint result)
        {
            right.Value = left ^ result;
        }
    }

    internal class ObservableLongBitwiseXor : ObservableReversableBinaryExpressionBase<long, long, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} ^ {1})";
            }
        }

        public ObservableLongBitwiseXor(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLongBitwiseXor(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<long> left, long right, long result)
        {
            left.Value = right ^ result;
        }

        protected override void SetRightValue(INotifyReversableExpression<long> right, long left, long result)
        {
            right.Value = left ^ result;
        }
    }

    internal class ObservableULongBitwiseXor : ObservableReversableBinaryExpressionBase<ulong, ulong, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} ^ {1})";
            }
        }

        public ObservableULongBitwiseXor(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        public override INotifyExpression<ulong> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableULongBitwiseXor(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<ulong> left, ulong right, ulong result)
        {
            left.Value = right ^ result;
        }

        protected override void SetRightValue(INotifyReversableExpression<ulong> right, ulong left, ulong result)
        {
            right.Value = left ^ result;
        }
    }
}
