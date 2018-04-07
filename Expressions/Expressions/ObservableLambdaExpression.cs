﻿using System;
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

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        protected override INotifyExpression<Expression<T>> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var visitor = new ApplyLambdaParametersVisitor(parameters, trace);
            return new ObservableLambdaExpression<T>((Expression<T>)visitor.Visit(Value));
        }

        private class ApplyLambdaParametersVisitor : ExpressionVisitorBase
        {
            private IDictionary<string, object> parameterMappings;
            private IDictionary<INotifiable, INotifiable> trace;

            public ApplyLambdaParametersVisitor(IDictionary<string, object> parameterMappings, IDictionary<INotifiable, INotifiable> trace)
            {
                this.parameterMappings = parameterMappings;
                this.trace = trace;
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
                        if (expression != null && expression.IsConstant)
                        {
                            return Expression.Constant(expression.ValueObject, node.Type);
                        }
                        else
                        {
                            var notifyValueType = typeof(INotifyValue<>).MakeGenericType(node.Type);
                            if (notifyValueType.IsInstanceOfType(argument))
                            {
                                return Expression.MakeMemberAccess(Expression.Constant(argument, notifyValueType), ReflectionHelper.GetProperty(notifyValueType, "Value"));
                            }
                        }
                    }
                }
                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                var expressionVal = node.Value as INotifyExpression;
                if (expressionVal != null)
                {
                    return Expression.Constant(expressionVal.ApplyParameters(parameterMappings, trace), node.Type);
                }
                return node;
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

            protected override Expression VisitConstant(ConstantExpression node)
            {
                var expressionVal = node.Value as INotifyExpression;
                if (expressionVal != null && !expressionVal.IsParameterFree)
                {
                    FoundExternalParameter = true;
                }
                return node;

            }
        }
    }
}
