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

        protected override bool GetValue()
        {
            return Left.Value && Right.Value;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicAnd(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
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

        protected override bool GetValue()
        {
            return Left.Value || Right.Value;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicOr(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
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

        protected override bool GetValue()
        {
            return Left.Value ^ Right.Value;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicXor(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
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

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicAndAlso(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            ValueChangedNotificationResult<bool> leftChange = null;
            if (sources.Count >= 1 && sources[0].Source == Left)
                leftChange = sources[0] as ValueChangedNotificationResult<bool>;
            else if (sources.Count >= 2 && sources[1].Source == Left)
                leftChange = sources[1] as ValueChangedNotificationResult<bool>;
            
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

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicOrElse(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            ValueChangedNotificationResult<bool> leftChange = null;
            if (sources.Count >= 1 && sources[0].Source == Left)
                leftChange = sources[0] as ValueChangedNotificationResult<bool>;
            else if (sources.Count >= 2 && sources[1].Source == Left)
                leftChange = sources[1] as ValueChangedNotificationResult<bool>;

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

        protected override bool GetValue()
        {
            return !Target.Value;
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicNot(Target.ApplyParameters(parameters));
        }
    }

}
