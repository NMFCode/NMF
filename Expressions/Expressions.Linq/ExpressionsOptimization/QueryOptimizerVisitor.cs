using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NMF.Expressions
{
    public class QueryOptimizerVisitor: ExpressionVisitorBase
    {
        private readonly LambdaExpression _firstLambdaSelectorExpression;
        private readonly LambdaExpression _secondLambdaSelectorExpression;

        public ParameterExpression OptimizationVaribable { get; set; }

        public QueryOptimizerVisitor(LambdaExpression firstLambdaSelectorExpression, LambdaExpression secondLambdaSelectorExpression)
        {
            _firstLambdaSelectorExpression = firstLambdaSelectorExpression;
            _secondLambdaSelectorExpression = secondLambdaSelectorExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {

            var param = _firstLambdaSelectorExpression.Parameters[0];

            if (node.ToString().Equals(param.ToString()))
            {              
                if(OptimizationVaribable != null)
                    return base.VisitParameter(OptimizationVaribable);

                //anstelle des Parameters, die zweite LambdaExpression einfügen
                return _secondLambdaSelectorExpression.Body;
            }
            return base.VisitParameter(node);
        }

    }
}