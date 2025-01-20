using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    /// <summary>
    /// Implements query optimization approaches
    /// </summary>
    internal class ProjectionMergeQueryOptimizer : IQueryOptimizer
    {
        private static Type[] promotionMethodCallTypes =
        {
            typeof(ObservingFunc<,>),
            typeof(ObservingFunc<,,>),
            typeof(ObservingFunc<,,,>),
            typeof(ObservingFunc<,,,,>),
            typeof(ObservingFunc<,,,,,>),
            typeof(ObservingFunc<,,,,,,>),
            typeof(ObservingFunc<,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,,,,,>),
            typeof(ObservingFunc<,,,,,,,,,,,,,,,>)
        };


        /// <summary>
        /// Optimization of two consecutive expressions into one
        /// </summary>
        /// <typeparam name="TSource">Source type of firstExpression and Source Type of merged expression</typeparam>
        /// <typeparam name="TResult">Result type of firstExpression</typeparam>
        /// <typeparam name="TNewResult">Result type of secondExpression and Result Type of merged expression</typeparam>
        /// <param name="firstExpression">In the query evaluation earlier occurring expression</param>
        /// <param name="secondExpression">In the query evaluation appearing expression after firstExpression</param>
        /// <returns></returns>
        public Expression Optimize<TSource, TResult, TNewResult>(Expression firstExpression, Expression secondExpression)
        {
           return MergeLambdaExpressions(firstExpression as LambdaExpression, secondExpression as LambdaExpression);
        }

        /// <summary>
        /// Merging two expressions by inserting secondLambdaExpression into firstLambdaExpression
        /// </summary>
        /// <param name="firstLambdaExpression"></param>
        /// <param name="secondLambdaExpression"></param>
        /// <returns></returns>
        public Expression MergeLambdaExpressions(LambdaExpression firstLambdaExpression, LambdaExpression secondLambdaExpression)
        {
            var queryOptimizerVisitor = new QueryOptimizerVisitor(firstLambdaExpression, secondLambdaExpression);
            var builtExpression = queryOptimizerVisitor.Visit(firstLambdaExpression) as LambdaExpression;    
            return Expression.Lambda(builtExpression.Body, secondLambdaExpression.Parameters);
        }

        /// <summary>
        /// Merging two expressions by introducing a variable that represents the second expression
        /// </summary>
        /// <typeparam name="TSource">Parameter type of firstLambdaExpression and Parameter type of merged expression</typeparam>
        /// <typeparam name="TResult">Result type of firstLambdaExpression</typeparam>
        /// <typeparam name="TNewResult">Result type of secondLambdaExpression and Result Type of merged expression</typeparam>
        /// <param name="firstLambdaExpression"></param>
        /// <param name="secondLambdaExpression"></param>
        /// <returns></returns>
        public Expression MergeLambdaExpressionsWithVariables<TSource, TResult, TNewResult>(LambdaExpression firstLambdaExpression, LambdaExpression secondLambdaExpression)
        {
            var introduceOptimizationVaribable = Expression.Parameter(typeof(TResult), "optimization_arg");
            var evaluateMethodArgs = new Expression[secondLambdaExpression.Parameters.Count + 1];
            var queryOptimizerVisitor = new QueryOptimizerVisitor(firstLambdaExpression, secondLambdaExpression)
                                        {
                                            OptimizationVaribable = introduceOptimizationVaribable
                                        };

            var parametersWithOptimizationVariable = new ParameterExpression[secondLambdaExpression.Parameters.Count + 1];
            for (int i = 0; i < secondLambdaExpression.Parameters.Count; i++)
                parametersWithOptimizationVariable[i] = secondLambdaExpression.Parameters[i];
            
            parametersWithOptimizationVariable[secondLambdaExpression.Parameters.Count] = introduceOptimizationVaribable;

            var introduceOptimizationExpr = queryOptimizerVisitor.Visit(Expression.Lambda(firstLambdaExpression.Body, parametersWithOptimizationVariable));
           
            for (int i = 0; i < secondLambdaExpression.Parameters.Count; i++)
            {
                evaluateMethodArgs[i] = secondLambdaExpression.Parameters[i];
            }
            evaluateMethodArgs[secondLambdaExpression.Parameters.Count] = secondLambdaExpression.Body;

            Type[] genericTypeArgumentsOfExprOfFunc = introduceOptimizationExpr.GetType().GetGenericArguments()[0].GenericTypeArguments;
            Type[] genericTypeArgumentsOfExprOfFuncWithoutResultType = new Type[genericTypeArgumentsOfExprOfFunc.Length - 1];
            Array.Copy(genericTypeArgumentsOfExprOfFunc, 0, genericTypeArgumentsOfExprOfFuncWithoutResultType, 0, genericTypeArgumentsOfExprOfFunc.Length - 1);


            var observingFunc = CreateObservingFunc(genericTypeArgumentsOfExprOfFunc, introduceOptimizationExpr);
            var evaluateMethodInfo = observingFunc.GetType().GetMethod("Evaluate", genericTypeArgumentsOfExprOfFuncWithoutResultType);

            return Expression.Lambda(
                        Expression.Call(
                            Expression.Constant(
                                observingFunc
                            ),
                            evaluateMethodInfo,
                            evaluateMethodArgs
                        ),
                        secondLambdaExpression.Parameters
                  );
        }

        public T CastExamp1<T>(object input)
        {
            return (T)input;
        }

        private object CreateObservingFunc(Type[] types, dynamic introduceOptimizationExpr)
        {
            var promotionMethodCallType = promotionMethodCallTypes[types.Length-2].MakeGenericType(types);
            var constructor = promotionMethodCallType.GetConstructors()[0];

            return constructor.Invoke(new[] { introduceOptimizationExpr });
        }

        public IOptimizableEnumerableExpression OptimizeExpression<TResult>(IOptimizableEnumerableExpression<TResult> expression)
        {
            

            if(expression.PrevExpression != null)
                return expression.AsOptimized2<TResult>(this);
            return expression;
        }

        public IEnumerableExpression Optimize<TResult>(ISQO expression)
        {
            if (expression is IOptimizableEnumerableExpression opt && expression.OptSource is IOptimizableEnumerableExpression optSource)
            {
#if DEBUG
                VisitForDebugging(opt.OptSelectorExpression);
#endif
                optSource.PrevExpression = opt.OptSelectorExpression;
                var test = optSource.AsOptimized2<TResult>(this);
                return Optimize<TResult>(test);
            }

            return (IEnumerableExpression)expression;
        }

        public IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TSource, TResult, TOptimizedResult>(SelectExpression<TSource, TResult> expression)
        {
#if DEBUG
            VisitForDebugging(expression.SelectorExpression);
#endif
            var mergedSelectorExpression = this.Optimize<TSource, TResult, TOptimizedResult>(expression.PrevExpression, expression.SelectorExpression) as Expression<Func<TSource, TOptimizedResult>>;
            return new SelectExpression<TSource, TOptimizedResult>(expression.Source, mergedSelectorExpression, null);
        }

        public IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TSource, TIntermediate, TResult, TOptimizedResult>(SelectManyExpression<TSource, TIntermediate, TResult> expression)
        {
#if DEBUG
            VisitForDebugging(expression.ResultSelector);
#endif
            if (expression.PrevExpression == null)
                throw new ArgumentNullException("expression.PrevExpression");

             var mergedSelectorExpression = this.Optimize<TSource, TResult, TOptimizedResult>(expression.PrevExpression,expression.ResultSelector) as Expression<Func< TSource, TIntermediate, TOptimizedResult >>;
             return new SelectManyExpression<TSource, TIntermediate, TOptimizedResult>(expression.Source, expression.FuncExpression, null, mergedSelectorExpression, null);           
        }

        public IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TOuter, TInner, TKey, TResult, TOptimizedResult>(JoinExpression<TOuter, TInner, TKey, TResult> expression)
        {
#if DEBUG
            VisitForDebugging(expression.ResultSelector);
#endif
            if (expression.PrevExpression == null)
                throw new ArgumentNullException("expression.PrevExpression");

            var mergedSelectorExpression = this.Optimize<TOuter, TResult, TOptimizedResult>(expression.PrevExpression, expression.ResultSelector) as Expression<Func<TOuter, TInner, TOptimizedResult>>;
            return new JoinExpression<TOuter, TInner, TKey, TOptimizedResult>(expression.Source, expression.Inner, expression.OuterKeySelector, null, expression.InnerKeySelector, null, mergedSelectorExpression, null, expression.Comparer);
        }

        public IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TOuter, TInner, TKey, TResult, TOptimizedResult>(GroupJoinExpression<TOuter, TInner, TKey, TResult> expression)
        {
#if DEBUG
            VisitForDebugging(expression.ResultSelector);
#endif
            if (expression.PrevExpression == null)
                throw new ArgumentNullException("expression.PrevExpression");

            var mergedSelectorExpression = this.Optimize<TOuter, TResult, TOptimizedResult>(expression.PrevExpression, expression.ResultSelector) as Expression<Func<TOuter, IEnumerable<TInner>, TOptimizedResult>>;
            return new GroupJoinExpression<TOuter, TInner, TKey, TOptimizedResult>(expression.Source, expression.Inner, expression.OuterKeySelector, null, expression.InnerKeySelector, null, mergedSelectorExpression, null, expression.Comparer);           
        }

        private void VisitForDebugging(Expression expression)
        {
            //Ausgabe überprüfen
            DebugVisitor debugVisitor = new DebugVisitor();
            debugVisitor.Visit(expression);
        }

    }
}