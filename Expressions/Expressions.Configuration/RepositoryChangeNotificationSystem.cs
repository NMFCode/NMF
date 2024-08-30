using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incrementalization system that recomputes all expressions upon any changes in the repository
    /// </summary>
    public class RepositoryChangeNotificationSystem : INotifySystem
    {
        /// <summary>
        /// Gets the model repository that is observed for changes
        /// </summary>
        public IModelRepository Repository { get; private set; }
        private static readonly MethodInfo genericCreateMethod;

        static RepositoryChangeNotificationSystem()
        {
            Expression<Func<RepositoryChangeNotificationSystem, INotifyExpression<string>>> dummy =
                n => n.CreateExpression<string>(null, null, null);
            genericCreateMethod = (dummy.Body as MethodCallExpression).Method.GetGenericMethodDefinition();
        }

        /// <summary>
        /// Creates a new incrementalization system listening to the changes of the given model repository
        /// </summary>
        /// <param name="repository"></param>
        public RepositoryChangeNotificationSystem(IModelRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            Repository = repository;
        }

        /// <inheritdoc />
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return genericCreateMethod.MakeGenericMethod(expression.Type).Invoke(this, new object[] { expression, parameters, parameterMappings }) as INotifyExpression;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
    }
}
