using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class TreeExtensionExpressionVisitor : ModelExpressionVisitorBase<object>
    {
        protected override bool ResetForCrossReference(Expression targetExpression, PropertyInfo property, bool isCrossReference, out Expression returnValue)
        {
            returnValue = null;
            return false;
        }

        protected override bool ResetForLambdaExpression(object state, MethodCallExpression methodCall, LambdaExpression lambda, out Expression returnValue)
        {
            returnValue = null;
            return false;
        }

        protected override object SaveState()
        {
            return null;
        }
    }
}
