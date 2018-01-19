using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace NMF.Expressions
{
    public class ExpressionAnalyzer : ExpressionVisitorBase
    {
        protected ExpressionAnalyzer() { }

        public static bool IsStateless(LambdaExpression lambdaExpression)
        {
            var instance = new ExpressionAnalyzer();
            instance.Visit(lambdaExpression);
            return instance.isStateLess;
        }

        private bool isStateLess = true;

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

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!node.Method.DeclaringType.IsValueType)
            {
                var immutable = node.Method.GetCustomAttributes(typeof(ImmutableMethodAttribute), true);
                if (immutable == null || immutable.Length > 0)
                {
                    isStateLess = false;
                }
            }
            return base.VisitMethodCall(node);
        }
    }
}
