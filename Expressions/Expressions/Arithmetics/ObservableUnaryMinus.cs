using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal sealed class ObservableUnaryIntMinus : ObservableUnaryReversableExpressionBase<int, int>
    {
        protected override string Format
        {
            get
            {
                return "-{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public ObservableUnaryIntMinus(INotifyExpression<int> inner)
            : base(inner) { }

        protected override int GetValue()
        {
            return -Target.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryIntMinus(Target.ApplyParameters(parameters, trace));
        }

        protected override void SetValue(INotifyReversableExpression<int> inner, int value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryLongMinus : ObservableUnaryReversableExpressionBase<long, long>
    {
        protected override string Format
        {
            get
            {
                return "-{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public ObservableUnaryLongMinus(INotifyExpression<long> inner)
            : base(inner) { }

        protected override long GetValue()
        {
            return -Target.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryLongMinus(Target.ApplyParameters(parameters, trace));
        }

        protected override void SetValue(INotifyReversableExpression<long> inner, long value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryFloatMinus : ObservableUnaryReversableExpressionBase<float, float>
    {
        protected override string Format
        {
            get
            {
                return "-{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public ObservableUnaryFloatMinus(INotifyExpression<float> inner)
            : base(inner) { }

        protected override float GetValue()
        {
            return -Target.Value;
        }

        protected override INotifyExpression<float> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryFloatMinus(Target.ApplyParameters(parameters, trace));
        }

        protected override void SetValue(INotifyReversableExpression<float> inner, float value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryDoubleMinus : ObservableUnaryReversableExpressionBase<double, double>
    {
        protected override string Format
        {
            get
            {
                return "-{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public ObservableUnaryDoubleMinus(INotifyExpression<double> inner)
            : base(inner) { }

        protected override double GetValue()
        {
            return -Target.Value;
        }

        protected override INotifyExpression<double> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryDoubleMinus(Target.ApplyParameters(parameters, trace));
        }

        protected override void SetValue(INotifyReversableExpression<double> inner, double value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryDecimalMinus : ObservableUnaryReversableExpressionBase<decimal, decimal>
    {
        protected override string Format
        {
            get
            {
                return "-{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Negate;

        public ObservableUnaryDecimalMinus(INotifyExpression<decimal> inner)
            : base(inner) { }

        protected override decimal GetValue()
        {
            return -Target.Value;
        }

        protected override INotifyExpression<decimal> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryDecimalMinus(Target.ApplyParameters(parameters, trace));
        }

        protected override void SetValue(INotifyReversableExpression<decimal> inner, decimal value)
        {
            inner.Value = -value;
        }
    }

}
