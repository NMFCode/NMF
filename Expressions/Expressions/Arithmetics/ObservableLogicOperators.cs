using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Arithmetics
{
    internal class ObservableLogicAnd : ObservableBinaryExpressionBase<bool, bool, bool>
    {
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
        public ObservableLogicAndAlso(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        protected override void AttachCore()
        {
            Left.Attach();
            if (Left.Value)
            {
                Right.Attach();
            }
        }

        protected override void DetachCore()
        {
            Left.Detach();
            if (Right.IsAttached) Right.Detach();
        }

        protected override bool GetValue()
        {
            if (!Left.Value) return false;
            if (!Right.IsAttached) Right.Attach();
            return Right.Value;
        }

        protected override void LeftChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            if (Left.Value)
            {
                Right.Attach();
            }
            else
            {
                Right.Detach();
            }
            Refresh();
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicAndAlso(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal class ObservableLogicOrElse : ObservableBinaryExpressionBase<bool, bool, bool>
    {
        public ObservableLogicOrElse(INotifyExpression<bool> left, INotifyExpression<bool> right)
            : base(left, right) { }

        protected override void AttachCore()
        {
            Left.Attach();
            if (!Left.Value)
            {
                Right.Attach();
            }
        }

        protected override void DetachCore()
        {
            Left.Detach();
            if (Right.IsAttached) Right.Detach();
        }

        protected override bool GetValue()
        {
            if (Left.Value) return true;
            if (!Right.IsAttached) Right.Attach();
            return Right.Value;
        }

        protected override void LeftChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            if (Left.Value)
            {
                Right.Detach();
            }
            else
            {
                Right.Attach();
            }
            Refresh();
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLogicOrElse(Left.ApplyParameters(parameters), Right.ApplyParameters(parameters));
        }
    }

    internal sealed class ObservableLogicNot : ObservableUnaryExpressionBase<bool, bool>
    {
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
