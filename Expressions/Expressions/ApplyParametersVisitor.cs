using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a vistor implementation that applies values to an expression tree
    /// </summary>
    public class ApplyParametersVisitor : ExpressionVisitorBase
    {
        private readonly IDictionary<string, object> parameterMappings;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parameterMappings">A dictionary with mappings for parameters based on parameter names</param>
        public ApplyParametersVisitor(IDictionary<string, object> parameterMappings)
        {
            this.parameterMappings = parameterMappings;
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
        {
            object argument;
            if (parameterMappings.TryGetValue(node.Name, out argument))
            {
                if (ReflectionHelper.IsInstanceOf(node.Type, argument))
                {
                    return Expression.Constant(argument);
                }
                else if (argument is Expression)
                {
                    return (Expression)argument;
                }
                else
                {
                    var notifyValueType = typeof(INotifyValue<>).MakeGenericType(node.Type);
                    if (ReflectionHelper.IsInstanceOf(notifyValueType, argument))
                    {
                        throw new NotImplementedException();
                    }
                    throw new InvalidOperationException(string.Format("The provided value {0} for parameter {1} is not valid.", argument, node.Type));
                }
            }
            else
            {
                return node;
            }
        }
    }

    internal class ReplaceParametersVisitor : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, Expression> parameterValues;

        public ReplaceParametersVisitor(IDictionary<ParameterExpression, Expression> parameterValues)
        {
            if (parameterValues == null) throw new ArgumentNullException("parameterValues");
            this.parameterValues = parameterValues;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression exp;
            if (parameterValues.TryGetValue(node, out exp))
            {
                return exp;
            }
            else
            {
                return node;
            }
        }
    }
}
