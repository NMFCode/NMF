using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    class ParameterReplacingVisitor : ExpressionVisitor
    {
        private PropertyPath path;
        private ParameterExpression replacement;

        protected override Expression VisitMember(MemberExpression node)
        {
            var current = node;
            var currentPath = path;

            while (currentPath.Parent != null && current != null)
            {
                if (currentPath.Member != node.Member)
                {
                    return node;
                }
                currentPath = currentPath.Parent;
                current = current.Expression as MemberExpression;
            }

            return replacement;
        }
    }
}
