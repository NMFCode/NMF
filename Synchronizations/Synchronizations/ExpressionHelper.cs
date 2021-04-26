using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Synchronizations
{
    internal static class ExpressionHelper
    {
        public static Expression<Func<T, ITransformationContext, TResult>> AddContextParameter<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression == null)
            {
                return null;
            }
            return Expression.Lambda<Func<T, ITransformationContext, TResult>>( expression.Body, expression.Parameters[0], Expression.Parameter( typeof( ITransformationContext ), "_transformation_context_" ) );
        }
        public static Expression<Action<T, ITransformationContext, TResult>> AddContextParameter<T, TResult>( Expression<Action<T, TResult>> expression )
        {
            if(expression == null)
            {
                return null;
            }
            return Expression.Lambda<Action<T, ITransformationContext, TResult>>( expression.Body, expression.Parameters[0], Expression.Parameter( typeof( ITransformationContext ), "_transformation_context_" ), expression.Parameters[1] );
        }
    }
}
