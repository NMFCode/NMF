using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableIntModulo : ObservableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableIntModulo(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLongModulo : ObservableBinaryExpressionBase<long, long, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableLongModulo(INotifyExpression<long> left, INotifyExpression<long> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableUIntModulo : ObservableBinaryExpressionBase<uint, uint, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableUIntModulo(INotifyExpression<uint> left, INotifyExpression<uint> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }


    internal class ObservableULongModulo : ObservableBinaryExpressionBase<ulong, ulong, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableULongModulo(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableFloatModulo : ObservableBinaryExpressionBase<float, float, float>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableFloatModulo(INotifyExpression<float> left, INotifyExpression<float> right)
            : base(left, right) { }

        protected override float GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<float> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableFloatModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableDoubleModulo : ObservableBinaryExpressionBase<double, double, double>
    {
        protected override string Format
        {
            get
            {
                return "({0} % {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.Modulo;

        public ObservableDoubleModulo(INotifyExpression<double> left, INotifyExpression<double> right)
            : base(left, right) { }

        protected override double GetValue()
        {
            return Left.Value % Right.Value;
        }

        protected override INotifyExpression<double> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableDoubleModulo(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
