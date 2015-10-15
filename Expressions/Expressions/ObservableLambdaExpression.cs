using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableLambdaExpression<T> : NotifyExpression<Expression<T>>
    {
        public ObservableLambdaExpression(Expression<T> value)
            : base(value) { }

        protected override Expression<T> GetValue()
        {
            return Value;
        }

        protected override void DetachCore() { }

        protected override void AttachCore() { }

        public override bool IsParameterFree
        {
            get { return true; }
        }

        public override INotifyExpression<Expression<T>> ApplyParameters(IDictionary<string, object> parameters)
        {
            var visitor = new ApplyParametersVisitor(parameters);
            return new ObservableLambdaExpression<T>((Expression<T>)visitor.Visit(Value));
        }
    }
}
