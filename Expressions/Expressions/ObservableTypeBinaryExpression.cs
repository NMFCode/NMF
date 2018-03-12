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
        }

        public override bool CanBeConstant
        {
            get { return Inner.CanBeConstant; }
        }

        public override ExpressionType NodeType
        {
            get
            {
                if (ExactMatch) return ExpressionType.TypeEqual;
                return ExpressionType.TypeIs;
            }
        }

        public override bool IsParameterFree
        {
            get { return Inner.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get { yield return Inner; }
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

        protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableTypeExpression(Inner.ApplyParameters(parameters, trace), TypeOperand, ExactMatch);
        }
    }
}
