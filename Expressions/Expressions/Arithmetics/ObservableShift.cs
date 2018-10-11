using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{

    internal class ObservableIntRightShift : ObservableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} >> {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.RightShift;

        public ObservableIntRightShift(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value >> Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntRightShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableIntLeftShift : ObservableBinaryExpressionBase<int, int, int>
    {
        protected override string Format
        {
            get
            {
                return "({0} << {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.LeftShift;

        public ObservableIntLeftShift(INotifyExpression<int> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override int GetValue()
        {
            return Left.Value << Right.Value;
        }

        protected override INotifyExpression<int> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableIntLeftShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableUIntRightShift : ObservableBinaryExpressionBase<uint, int, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} >> {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.RightShift;

        public ObservableUIntRightShift(INotifyExpression<uint> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value >> Right.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntRightShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableUIntLeftShift : ObservableBinaryExpressionBase<uint, int, uint>
    {
        protected override string Format
        {
            get
            {
                return "({0} << {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.LeftShift;

        public ObservableUIntLeftShift(INotifyExpression<uint> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override uint GetValue()
        {
            return Left.Value << Right.Value;
        }

        protected override INotifyExpression<uint> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUIntLeftShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }


    internal class ObservableLongRightShift : ObservableBinaryExpressionBase<long, int, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} >> {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.RightShift;

        public ObservableLongRightShift(INotifyExpression<long> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value >> Right.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongRightShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLongLeftShift : ObservableBinaryExpressionBase<long, int, long>
    {
        protected override string Format
        {
            get
            {
                return "({0} << {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.LeftShift;

        public ObservableLongLeftShift(INotifyExpression<long> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override long GetValue()
        {
            return Left.Value << Right.Value;
        }

        protected override INotifyExpression<long> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLongLeftShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableULongRightShift : ObservableBinaryExpressionBase<ulong, int, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} >> {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.RightShift;

        public ObservableULongRightShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value >> Right.Value;
        }

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongRightShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableULongLeftShift : ObservableBinaryExpressionBase<ulong, int, ulong>
    {
        protected override string Format
        {
            get
            {
                return "({0} << {1})";
            }
        }

        public override ExpressionType NodeType => ExpressionType.LeftShift;

        public ObservableULongLeftShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
            : base(left, right) { }

        protected override ulong GetValue()
        {
            return Left.Value << Right.Value;
        }

        protected override INotifyExpression<ulong> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableULongLeftShift(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }
}
