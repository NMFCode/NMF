using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class GeneralizingNotifySystem : INotifySystem
    {
        private INotifySystem Inner { get; set; }

        public GeneralizingNotifySystem(INotifySystem inner)
        {
            Inner = inner;
        }

        public INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName)
        {
            return Inner.CreateLocal(inner, localVariable, out paramName);
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IDictionary<string, object> parameterMappings)
        {
            var generalizingVisitor = new GeneralizingExpressionVisitor();
            generalizingVisitor.Visit(expression);
            
            return Inner.CreateExpression<T>(expression, parameterMappings);
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IDictionary<string, object> parameterMappings)
        {
            return Inner.CreateReversableExpression<T>(expression, parameterMappings);
        }
    }
}
