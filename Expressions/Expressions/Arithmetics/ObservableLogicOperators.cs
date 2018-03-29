using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableLogicAnd : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} & {1})";
            }
        }

        public ObservableLogicAnd(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.And; } }

        protected override bool GetValue()
        {
            return Left.Value && Right.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicAnd(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLogicOr : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} | {1})";
            }
        }

        public ObservableLogicOr(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.Or; } }

        protected override bool GetValue()
        {
            return Left.Value || Right.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicOr(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLogicXor : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} ^ {1})";
            }
        }

        public ObservableLogicXor(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        public override ExpressionType NodeType { get { return ExpressionType.ExclusiveOr; } }

        protected override bool GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicXor(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }
    }

    internal class ObservableLogicAndAlso : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} && {1})";
            }
        }
		
        public ObservableLogicAndAlso(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.AndAlso; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Left;
                if (Left.Value)
                    yield return Right;
            }
        }
        
        protected override bool GetValue()
        {
            if (!Left.Value)
                return false;
            return Right.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicAndAlso(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            IValueChangedNotificationResult<bool> leftChange = null;
            if (sources.Count >= 1 && sources[0].Source == Left)
                leftChange = sources[0] as IValueChangedNotificationResult<bool>;
            else if (sources.Count >= 2 && sources[1].Source == Left)
                leftChange = sources[1] as IValueChangedNotificationResult<bool>;
            
            if (leftChange != null)
            {
                if (leftChange.NewValue)
                    Right.Successors.Set(this);
                else
                    Right.Successors.Unset(this);
            }
            return base.Notify(sources);
        }
    }

    internal class ObservableLogicOrElse : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "({0} || {1})";
            }
        }
		
        public ObservableLogicOrElse(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.OrElse; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Left;
                if (!Left.Value)
                    yield return Right;
            }
        }

        protected override bool GetValue()
        {
            if (Left.Value)
                return true;
            return Right.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicOrElse(Left.ApplyParameters(parameters, trace), Right.ApplyParameters(parameters, trace));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            IValueChangedNotificationResult<bool> leftChange = null;
            if (sources.Count >= 1 && sources[0].Source == Left)
                leftChange = sources[0] as IValueChangedNotificationResult<bool>;
            else if (sources.Count >= 2 && sources[1].Source == Left)
                leftChange = sources[1] as IValueChangedNotificationResult<bool>;

            if (leftChange != null)
            {
                if (leftChange.NewValue)
                    Right.Successors.Unset(this);
                else
                    Right.Successors.Set(this);
            }
            return base.Notify(sources);
        }
    }

    internal sealed class ObservableLogicNot : ObservableUnaryExpressionBase<bool, bool>
    {
        protected override string Format
        {
            get
            {
                return "!{0}";
            }
        }

        public ObservableLogicNot(INotifyExpression<bool> inner)
            : base(inner) { }

        public override ExpressionType NodeType { get { return ExpressionType.Not; } }

        protected override bool GetValue()
        {
            return !Target.Value;
        }

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableLogicNot(Target.ApplyParameters(parameters, trace));
        }
    }

}
