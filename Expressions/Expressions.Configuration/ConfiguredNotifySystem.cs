using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a notify system that incrementalizes functions based on a configuration
    /// </summary>
    public class ConfiguredNotifySystem : INotifySystem
    {
        private readonly Dictionary<string, IncrementalizationStrategy> strategies = new Dictionary<string, IncrementalizationStrategy>();
        private readonly IncrementalizationStrategy defaultStrategy;

        private static readonly ModelNotifySystem instructionLevel = ModelNotifySystem.Instance;
        private readonly RepositoryChangeNotificationSystem repositoryChange;
        private readonly TreeExtensionNotifySystem augmentation = new TreeExtensionNotifySystem();
        private readonly PromotionNotifySystem promotion = new PromotionNotifySystem();

        /// <summary>
        /// Creates a new configured notify system
        /// </summary>
        /// <param name="repository">The model repository</param>
        /// <param name="configuration">The configuration</param>
        /// <param name="defaultStrategy">The default strategy to chose if there is no configuration entry</param>
        public ConfiguredNotifySystem(IModelRepository repository, Configuration configuration, IncrementalizationStrategy defaultStrategy = IncrementalizationStrategy.InstructionLevel)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            repositoryChange = new RepositoryChangeNotificationSystem(repository);
            foreach (var methodConf in configuration.MethodConfigurations)
            {
                strategies.Add(methodConf.MethodIdentifier, methodConf.Strategy);
            }
            this.defaultStrategy = defaultStrategy;
        }

        /// <inheritdoc />
        public INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName)
        {
            return instructionLevel.CreateLocal<T, TVar>(inner, localVariable, out paramName);
        }

        /// <inheritdoc />
        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateExpression<T>(expression, parameters, parameterMappings);
        }

        private INotifySystem FindResponsible(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            IncrementalizationStrategy strategy;
            var expressionId = expression.ToString();
            if (!strategies.TryGetValue(expressionId, out strategy))
            {
                strategy = defaultStrategy;
            }
            switch (strategy)
            {
                case IncrementalizationStrategy.InstructionLevel:
                    return instructionLevel;
                case IncrementalizationStrategy.ArgumentPromotion:
                    return promotion;
                case IncrementalizationStrategy.ListenRepositoryChanges:
                    return repositoryChange;
                case IncrementalizationStrategy.UseAugmentation:
                    return augmentation;
                default:
                    throw new InvalidOperationException(string.Format("The selected incrementalization strategy for expression {0} is not valid", expressionId));
            }
        }

        /// <inheritdoc />
        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateReversableExpression<T>(expression, parameters, parameterMappings);
        }

        /// <inheritdoc />
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateExpression(expression, parameters, parameterMappings);
        }
    }
}
