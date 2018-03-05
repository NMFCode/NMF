using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    public sealed class QueryOptimizer<TSource, TResult, TNewResult>
    {
        private readonly LambdaExpression _firstLambdaSelectorExpression;
        private readonly LambdaExpression _secondLambdaSelectorExpression;

        private readonly QueryOptimizerVisitor _queryOptimizerVisitor;

        //Needed for V3
        private readonly ParameterExpression _optimizationVaribable = Expression.Parameter(typeof(TResult), "optimization_arg");
        private ObservingFunc<TSource, TResult, TNewResult> _intermediateSelectorExpression;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstSelectorExpression"></param>
        /// <param name="secondSelectorExpression">insert into firstSelectorExpression</param>
        public QueryOptimizer(Expression firstSelectorExpression, Expression secondSelectorExpression)
        {
            _firstLambdaSelectorExpression = firstSelectorExpression as LambdaExpression;
            _secondLambdaSelectorExpression = secondSelectorExpression as LambdaExpression;
            _queryOptimizerVisitor = new QueryOptimizerVisitor(_firstLambdaSelectorExpression, _secondLambdaSelectorExpression);
        }

        public Expression Optimize()
        {
            return MergeLambdaExpressions();
            //return MergeLambdaExpressionsWithVariables();
        }


        public Expression MergeLambdaExpressions()
        {
            var builtExpression = _queryOptimizerVisitor.Visit(_firstLambdaSelectorExpression) as LambdaExpression;
            return Expression.Lambda(builtExpression.Body, _secondLambdaSelectorExpression.Parameters);            
        }

        public Expression MergeLambdaExpressionsWithVariables()
        {
            _queryOptimizerVisitor.OptimizationVaribable = _optimizationVaribable;
            //TODO: Methode schwer verständlich. Kann man kleinere Untermethoden erstellen die beschreibenden Namen haben?

            var p = Expression.Lambda(_firstLambdaSelectorExpression.Body, _secondLambdaSelectorExpression.Parameters[0], _optimizationVaribable);
            var builtLambdaExpression = _queryOptimizerVisitor.Visit(p);

            this._intermediateSelectorExpression = new ObservingFunc<TSource, TResult, TNewResult>(builtLambdaExpression as Expression<Func<TSource, TResult, TNewResult>>);

            var methodInfo = typeof(ObservingFunc<TSource, TResult, TNewResult>).GetMethod("Evaluate", new[] { typeof(TSource), typeof(TResult) });
            var constantExpr = Expression.Constant(_intermediateSelectorExpression);

            var newLamdaExpression = Expression.Lambda(
                Expression.Call(constantExpr, methodInfo, this._secondLambdaSelectorExpression.Parameters[0], _secondLambdaSelectorExpression.Body),
                this._secondLambdaSelectorExpression.Parameters
                );

            return newLamdaExpression;
        }


    }
}