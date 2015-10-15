using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableTypeExpression : NotifyExpression<bool>
    {
        public INotifyExpression<object> Inner { get; private set; }

        public Type TypeOperand { get; private set; }

        public bool ExactMatch { get; private set; }

        public ObservableTypeExpression(INotifyExpression<object> inner, Type typeOperand, bool exactMatch)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (typeOperand == null) throw new ArgumentNullException("typeOperand");

            Inner = inner;
            ExactMatch = exactMatch;
            TypeOperand = typeOperand;

            inner.ValueChanged += InnerValueChanged;
        }

        void InnerValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public override bool CanBeConstant
        {
            get
            {
                return Inner.CanBeConstant;
            }
        }

        public override ExpressionType NodeType
        {
            get
            {
                if (ExactMatch) return ExpressionType.TypeEqual;
                return ExpressionType.TypeIs;
            }
        }

        protected override bool GetValue()
        {
            if (!ExactMatch)
            {
                return Inner.Value == null || ReflectionHelper.IsInstanceOf(TypeOperand, Inner.Value);
            }
            else
            {
                return Inner.Value == null || Inner.Value.GetType() == TypeOperand;
            }
        }

        protected override void DetachCore()
        {
            Inner.Detach();
        }

        protected override void AttachCore()
        {
            Inner.Attach();
        }

        public override bool IsParameterFree
        {
            get { return Inner.IsParameterFree; }
        }

        public override INotifyExpression<bool> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTypeExpression(Inner.ApplyParameters(parameters), TypeOperand, ExactMatch);
        }
    }
}
