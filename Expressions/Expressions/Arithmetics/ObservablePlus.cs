using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntPlus : ObservableReversableBinaryExpressionBase<int, int, int>
    {
        public ObservableIntPlus(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableIntPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<int> left, int right, int result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<int> right, int left, int result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableLongPlus : ObservableReversableBinaryExpressionBase<long, long, long>
    {
        public ObservableLongPlus(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLongPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<long> left, long right, long result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<long> right, long left, long result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableUIntPlus : ObservableReversableBinaryExpressionBase<uint, uint, uint>
    {
        public ObservableUIntPlus(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<uint> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUIntPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<uint> left, uint right, uint result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<uint> right, uint left, uint result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableULongPlus : ObservableReversableBinaryExpressionBase<ulong, ulong, ulong>
    {
        public ObservableULongPlus(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<ulong> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableULongPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<ulong> left, ulong right, ulong result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<ulong> right, ulong left, ulong result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableFloatPlus : ObservableReversableBinaryExpressionBase<float, float, float>
    {
        public ObservableFloatPlus(INotifyExpression<float> left, INotifyExpression<float> right)
            : base(left, right) { }

        protected override float GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<float> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableFloatPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<float> left, float right, float result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<float> right, float left, float result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableDoublePlus : ObservableReversableBinaryExpressionBase<double, double, double>
    {
        public ObservableDoublePlus(INotifyExpression<double> left, INotifyExpression<double> right)
            : base(left, right) { }

        protected override double GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<double> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableDoublePlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<double> left, double right, double result)
        {
            if (left.Value + right != result)
            {
                left.Value = result - right;
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<double> right, double left, double result)
        {
            if (left + right.Value != result)
            {
                right.Value = result - left;
            }
        }
    }

    internal class ObservableStringPlus : ObservableReversableBinaryExpressionBase<string, string, string>
    {
        public ObservableStringPlus(INotifyExpression<string> left, INotifyExpression<string> right)
            : base(left, right) { }

        protected override string GetValue()
        {
            return Left.Value + Right.Value;
        }

        public override INotifyExpression<string> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStringPlus(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        protected override void SetLeftValue(INotifyReversableExpression<string> left, string right, string result)
        {
            if (string.IsNullOrEmpty(right))
            {
                left.Value = result;
            }
            else if (left.Value + right != result && result != null && result.EndsWith(right))
            {
                left.Value = result.Substring(0, result.Length - right.Length);
            }
        }

        protected override void SetRightValue(INotifyReversableExpression<string> right, string left, string result)
        {
            if (string.IsNullOrEmpty(left))
            {
                right.Value = result;
            }
            else if (left + right.Value != result && result != null && result.StartsWith(left))
            {
                right.Value = result.Substring(left.Length);
            }
        }
    }
}
