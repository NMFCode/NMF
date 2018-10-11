using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{

    internal sealed class ObservableIntOnesComplement : ObservableUnaryExpressionBase<int, int>
    {
        protected override string Format
        {
            get
            {
                return "~{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.OnesComplement;

        public ObservableIntOnesComplement(INotifyExpression<int> inner)
            : base(inner) { }

        protected override int GetValue()
        {
            return ~Target.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntOnesComplement(Target.ApplyParameters(parameters, trace));
        }
    }

    internal sealed class ObservableUIntOnesComplement : ObservableUnaryExpressionBase<uint, uint>
    {
        protected override string Format
        {
            get
            {
                return "~{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.OnesComplement;

        public ObservableUIntOnesComplement(INotifyExpression<uint> inner)
            : base(inner) { }

        protected override uint GetValue()
        {
            return ~Target.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntOnesComplement(Target.ApplyParameters(parameters, trace));
        }
    }

    internal sealed class ObservableLongOnesComplement : ObservableUnaryExpressionBase<long, long>
    {
        protected override string Format
        {
            get
            {
                return "~{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.OnesComplement;

        public ObservableLongOnesComplement(INotifyExpression<long> inner)
            : base(inner) { }

        protected override long GetValue()
        {
            return ~Target.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongOnesComplement(Target.ApplyParameters(parameters, trace));
        }
    }

    internal sealed class ObservableULongOnesComplement : ObservableUnaryExpressionBase<ulong, ulong>
    {
        protected override string Format
        {
            get
            {
                return "~{0}";
            }
        }

        public override ExpressionType NodeType => ExpressionType.OnesComplement;

        public ObservableULongOnesComplement(INotifyExpression<ulong> inner)
            : base(inner) { }

        protected override ulong GetValue()
        {
            return ~Target.Value;
        }

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongOnesComplement(Target.ApplyParameters(parameters, trace));
        }
    }
}
