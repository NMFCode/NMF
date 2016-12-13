using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    public class ConfiguredNotifySystem : INotifySystem
    {
        private Dictionary<string, IncrementalizationStrategy> strategies = new Dictionary<string, IncrementalizationStrategy>();
        private IncrementalizationStrategy defaultStrategy;

        private static InstructionLevelNotifySystem instructionLevel = InstructionLevelNotifySystem.Instance;
        private RepositoryChangeNotificationSystem repositoryChange;
        private TreeExtensionNotifySystem augmentation = new TreeExtensionNotifySystem();
        private PromotionNotifySystem promotion = new PromotionNotifySystem();

        public ConfiguredNotifySystem(IModelRepository repository, Configuration configuration, IncrementalizationStrategy defaultStrategy = IncrementalizationStrategy.InstructionLevel)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (configuration == null) throw new ArgumentNullException("configuration");

            repositoryChange = new RepositoryChangeNotificationSystem(repository);
            foreach (var methodConf in configuration.MethodConfigurations)
            {
                strategies.Add(methodConf.MethodIdentifier, methodConf.Strategy);
            }
            this.defaultStrategy = defaultStrategy;
        }

        public INotifyExpression<T> CreateLocal<T, TVar>(INotifyExpression<T> inner, INotifyExpression<TVar> localVariable, out string paramName)
        {
            return instructionLevel.CreateLocal<T, TVar>(inner, localVariable, out paramName);
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateExpression<T>(expression, parameters, parameterMappings);
        }

        private INotifySystem FindResponsible(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
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

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateReversableExpression<T>(expression, parameters, parameterMappings);
        }

        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return FindResponsible(expression).CreateExpression(expression, parameters, parameterMappings);
        }

        public ISuccessorList CreateSuccessorList() => new MultiSuccessorList();
    }
}
