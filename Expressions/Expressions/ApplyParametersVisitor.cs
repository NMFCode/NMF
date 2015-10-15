using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ApplyParametersVisitor : ExpressionVisitor
    {
        private IDictionary<string, object> parameterMappings;

        public ApplyParametersVisitor(IDictionary<string, object> parameterMappings)
        {
            this.parameterMappings = parameterMappings;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object argument;
            if (parameterMappings.TryGetValue(node.Name, out argument))
            {
                return Expression.Constant(argument);
            }
            else
            {
                return node;
            }
        }
    }

    internal class ReplaceParametersVisitor : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, Expression> parameterValues;

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
