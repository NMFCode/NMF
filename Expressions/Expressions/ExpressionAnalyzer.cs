using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an analyzer to analyzer whether a lambda expression is stateless
    /// </summary>
    public class ExpressionAnalyzer : ExpressionVisitorBase
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected ExpressionAnalyzer() { }

        /// <summary>
        /// Determines whether the given lambda expression is stateless
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression</param>
        /// <returns>True, if stateless, otherwise False</returns>
        public static bool IsStateless(LambdaExpression lambdaExpression)
        {
            var instance = new ExpressionAnalyzer();
            instance.Visit(lambdaExpression);
            return instance.isStateLess;
        }

        private bool isStateLess = true;

        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            if (!node.Member.DeclaringType.IsValueType)
            {
                var generated = node.Member.DeclaringType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);
                if (generated == null || generated.Length == 0)
                {
                    isStateLess = false;
                }
            }
            return base.VisitMember(node);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!node.Method.DeclaringType.IsValueType)
            {
                var immutable = node.Method.GetCustomAttributes(typeof(ImmutableMethodAttribute), true);
                if (immutable == null || immutable.Length == 0)
                {
                    isStateLess = false;
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
