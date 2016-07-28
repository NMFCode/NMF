using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    public class TreeExtensionNotifySystem : INotifySystem
    {
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotImplementedException();
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotImplementedException();
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotImplementedException();
        }
    }
}
