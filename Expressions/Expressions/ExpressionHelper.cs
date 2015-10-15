using System.Linq.Expressions;

namespace NMF.Expressions
{
    internal static class ExpressionHelper
    {
        public static Expression GetArg(MethodCallExpression node, int index)
        {
            if (node.Method.IsStatic)
            {
                return node.Arguments[index];
            }
            else
            {
                if (index == 0)
                {
                    return node.Object;
                }
                else
                {
                    return node.Arguments[index - 1];
                }
            }
        }
    }
}
