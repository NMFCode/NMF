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
            var leftChange = sources.FirstOrDefault(c => c.Source == Left) as ValueChangedNotificationResult<bool>;
            if (leftChange != null)
            {
                if (leftChange.NewValue)
                    Right.Successors.Add(this);
                else
                    Right.Successors.Remove(this);
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
            var leftChange = sources.FirstOrDefault(c => c.Source == Left) as ValueChangedNotificationResult<bool>;
            if (leftChange != null)
            {
                if (leftChange.NewValue)
                    Right.Successors.Remove(this);
                else
                    Right.Successors.Add(this);
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
