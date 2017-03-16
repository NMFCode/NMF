using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    public class RepositoryChangeNotificationSystem : INotifySystem
    {
        public IModelRepository Repository { get; private set; }
        private static MethodInfo genericCreateMethod;

        static RepositoryChangeNotificationSystem()
        {
            Expression<Func<RepositoryChangeNotificationSystem, INotifyExpression<string>>> dummy =
                n => n.CreateExpression<string>(null, null, null);
            genericCreateMethod = (dummy.Body as MethodCallExpression).Method.GetGenericMethodDefinition();
        }

        public RepositoryChangeNotificationSystem(IModelRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            Repository = repository;
        }

        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return genericCreateMethod.MakeGenericMethod(expression.Type).Invoke(this, new object[] { expression, parameters, parameterMappings }) as INotifyExpression;
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            if (parameterMappings != null)
            {
                var applyParameters = new ApplyParametersVisitor(parameterMappings);
                expression = applyParameters.Visit(expression);
            }

            if (parameters == null || (parameterMappings == null ? !parameters.Any() : parameters.Count() == parameterMappings.Count))
            {
                var lambda = Expression.Lambda<Func<T>>(expression);
                return new RepositoryAffectedNotifyValue<T>(Repository, lambda.Compile());
            }
            else
            {
                return new RepositoryAffectedNotifyFunc<T>(Repository, expression, parameters);
            }
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            if (parameterMappings != null)
            {
                var applyParameters = new ApplyParametersVisitor(parameterMappings);
                expression = applyParameters.Visit(expression);
            }

            if (parameters == null || (parameterMappings == null ? !parameters.Any() : parameters.Count() == parameterMappings.Count))
            {
                var lambda = Expression.Lambda<Func<T>>(expression);
                var setter = SetExpressionRewriter.CreateSetter(lambda);
                if (setter == null) throw new InvalidOperationException(string.Format("The lambda expression {0} could not be reversed.", expression));
                return new RepositoryAffectedReversableNotifyValue<T>(Repository, lambda.Compile(), setter.Compile());
            }
            else
            {
                return new RepositoryAffectedReversableNotifyFunc<T>(Repository, expression, parameters);
            }
        }

        public ISuccessorList CreateSuccessorList() => new MultiSuccessorList();
    }
}
