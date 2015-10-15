using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal sealed class ObservableUnaryIntMinus : ObservableUnaryReversableExpressionBase<int, int>
    {
        public ObservableUnaryIntMinus(INotifyExpression<int> inner)
            : base(inner) { }

        protected override int GetValue()
        {
            return -Target.Value;
        }

        public override INotifyExpression<int> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryIntMinus(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<int> inner, int value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryLongMinus : ObservableUnaryReversableExpressionBase<long, long>
    {
        public ObservableUnaryLongMinus(INotifyExpression<long> inner)
            : base(inner) { }

        protected override long GetValue()
        {
            return -Target.Value;
        }

        public override INotifyExpression<long> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryLongMinus(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<long> inner, long value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryFloatMinus : ObservableUnaryReversableExpressionBase<float, float>
    {
        public ObservableUnaryFloatMinus(INotifyExpression<float> inner)
            : base(inner) { }

        protected override float GetValue()
        {
            return -Target.Value;
        }

        public override INotifyExpression<float> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryFloatMinus(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<float> inner, float value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryDoubleMinus : ObservableUnaryReversableExpressionBase<double, double>
    {
        public ObservableUnaryDoubleMinus(INotifyExpression<double> inner)
            : base(inner) { }

        protected override double GetValue()
        {
            return -Target.Value;
        }

        public override INotifyExpression<double> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryDoubleMinus(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<double> inner, double value)
        {
            inner.Value = -value;
        }
    }

    internal sealed class ObservableUnaryDecimalMinus : ObservableUnaryReversableExpressionBase<decimal, decimal>
    {
        public ObservableUnaryDecimalMinus(INotifyExpression<decimal> inner)
            : base(inner) { }

        protected override decimal GetValue()
        {
            return -Target.Value;
        }

        public override INotifyExpression<decimal> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryDecimalMinus(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<decimal> inner, decimal value)
        {
            inner.Value = -value;
        }
    }

}
