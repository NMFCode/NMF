using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    internal class ParameterCollector : ExpressionVisitor
    {
        public HashSet<ParameterExpression> Parameters { get; private set; }

        public ParameterCollector()
        {
            Parameters = new HashSet<ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Parameters.Add(node);
            return node;
        }
    }
}
