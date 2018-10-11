using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntPlus : ObservableReversableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableIntPlus(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableLongPlus(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableUIntPlus(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableULongPlus(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableFloatPlus(INotifyExpression<float> left, INotifyExpression<float> right)
            : base(left, right) { }

        protected override float GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<float> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableFloatPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableDoublePlus(INotifyExpression<double> left, INotifyExpression<double> right)
            : base(left, right) { }

        protected override double GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<double> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableDoublePlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
        protected override string Format
        {
            get
            {
                return "({0} + {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Add;

        public ObservableStringPlus(INotifyExpression<string> left, INotifyExpression<string> right)
            : base(left, right) { }

        protected override string GetValue()
        {
            return Left.Value + Right.Value;
        }

        protected override INotifyExpression<string> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStringPlus(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
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
