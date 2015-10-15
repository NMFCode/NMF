using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    public class ExpressionCompileRewriter : ExpressionVisitor
    {
        private static ExpressionCompileRewriter instance = new ExpressionCompileRewriter();

        public static T Compile<T>(Expression<T> lambda)
        {
            if (lambda == null) return default(T);
            var newLambda = instance.Visit(lambda) as Expression<T>;
            return newLambda.Compile();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var rewriterAttributes = ReflectionHelper.GetCustomAttributes<ExpressionCompileRewriterAttribute>(node.Method, false);
            if (rewriterAttributes != null && rewriterAttributes.Length == 1)
            {
                var rewriteAtt = rewriterAttributes[0];
                
                var rewriter = rewriteAtt.InitializeProxyMethod(node.Method);
                var rewriterDelegate = ReflectionHelper.CreateDelegate<Func<MethodCallExpression, Expression>>(rewriter);
                return rewriterDelegate.Invoke(node);
            }
            return base.VisitMethodCall(node);
        }
    }
}
