using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    /// <summary>
    /// Implements query optimization approaches
    /// </summary>
    public static class QueryOptimizer
    {
        /// <summary>
        /// Optimization of two consecutive expressions into one
        /// </summary>
        /// <typeparam name="TSource">Source type of firstExpression and Source Type of merged expression</typeparam>
        /// <typeparam name="TResult">Result type of firstExpression</typeparam>
        /// <typeparam name="TNewResult">Result type of secondExpression and Result Type of merged expression</typeparam>
        /// <param name="firstExpression">In the query evaluation earlier occurring expression</param>
        /// <param name="secondExpression">In the query evaluation appearing expression after firstExpression</param>
        /// <returns></returns>
        public static Expression Optimize<TSource, TResult, TNewResult>(Expression firstExpression, Expression secondExpression)
        {
            return MergeLambdaExpressions(firstExpression as LambdaExpression, secondExpression as LambdaExpression);
            //return MergeLambdaExpressionsWithVariables<TSource, TResult, TNewResult>(firstExpression as LambdaExpression, secondExpression as LambdaExpression);
        }

        /// <summary>
        /// Merging two expressions by inserting secondLambdaExpression into firstLambdaExpression
        /// </summary>
        /// <param name="firstLambdaExpression"></param>
        /// <param name="secondLambdaExpression"></param>
        /// <returns></returns>
        public static Expression MergeLambdaExpressions(LambdaExpression firstLambdaExpression, LambdaExpression secondLambdaExpression)
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
        public static Expression MergeLambdaExpressionsWithVariables<TSource, TResult, TNewResult>(LambdaExpression firstLambdaExpression, LambdaExpression secondLambdaExpression)
        {
            var introduceOptimizationVaribable = Expression.Parameter(typeof(TResult), "optimization_arg");
            var evaluateMethodInfo = typeof(ObservingFunc<TSource, TResult, TNewResult>).GetMethod("Evaluate", new[] { typeof(TSource), typeof(TResult) });

            var queryOptimizerVisitor = new QueryOptimizerVisitor(firstLambdaExpression, secondLambdaExpression)
                                        {
                                            OptimizationVaribable = introduceOptimizationVaribable
                                        };

            //TODO: Methode schwer verständlich. Kann man kleinere Untermethoden erstellen die beschreibenden Namen haben?

            //Lambda expression that contains the second expression as a parameter. After visit that expression new parameter is used
            var introduceOptimizationExpr = queryOptimizerVisitor.Visit(Expression.Lambda(firstLambdaExpression.Body, secondLambdaExpression.Parameters[0], introduceOptimizationVaribable));

            return Expression.Lambda(
                        Expression.Call(
                            Expression.Constant(
                                new ObservingFunc<TSource, TResult, TNewResult>(
                                    introduceOptimizationExpr as Expression<Func<TSource, TResult, TNewResult>>)
                            ), 
                            evaluateMethodInfo, 
                            secondLambdaExpression.Parameters[0], 
                            secondLambdaExpression.Body
                        ),
                        secondLambdaExpression.Parameters
                  );
        }
    }
}