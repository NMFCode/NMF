using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

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

        public static Func<T, ITransformationContext, TResult> AddContextParameter<T, TResult>( Func<T, TResult> expression )
        {
            if(expression == null)
            {
                return null;
            }
            return ( it, context ) => expression( it );
        }
        public static Action<T, ITransformationContext, TResult> AddContextParameter<T, TResult>( Action<T, TResult> expression )
        {
            if(expression == null)
            {
                return null;
            }
            return ( it, context, value ) => expression( it, value );
        }

    }
}
