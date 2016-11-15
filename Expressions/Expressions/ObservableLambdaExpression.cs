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
            get
            {
                var checker = new CheckLambdaParametersVisitor();
                checker.Lambda = Value;
                checker.Visit(Value.Body);
                return !checker.FoundExternalParameter;
            }
        }

        public override INotifyExpression<Expression<T>> ApplyParameters(IDictionary<string, object> parameters)
        {
            var visitor = new ApplyLambdaParametersVisitor(parameters);
            return new ObservableLambdaExpression<T>((Expression<T>)visitor.Visit(Value));
        }

        private class ApplyLambdaParametersVisitor : ExpressionVisitorBase
        {
            private IDictionary<string, object> parameterMappings;

            public ApplyLambdaParametersVisitor(IDictionary<string, object> parameterMappings)
            {
                this.parameterMappings = parameterMappings;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                object argument;
                if (parameterMappings.TryGetValue(node.Name, out argument))
                {
                    if (ReflectionHelper.IsInstanceOf(node.Type, argument))
                    {
                        return Expression.Constant(argument, node.Type);
                    }
                    else
                    {
                        INotifyExpression expression = argument as INotifyExpression;
                        if (expression.IsConstant)
                        {
                            return Expression.Constant(expression.ValueObject, node.Type);
                        }
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    return node;
                }
            }
        }

        private class CheckLambdaParametersVisitor : ExpressionVisitorBase
        {
            public LambdaExpression Lambda { get; set; }
            public bool FoundExternalParameter { get; private set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!Lambda.Parameters.Contains(node))
                {
                    FoundExternalParameter = true;
                }
                return node;
            }
        }
    }
}
