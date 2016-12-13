using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Models;
using NMF.Utilities;

namespace NMF.Expressions
{
    public class RecordingNotifySystem : INotifySystem
    {
        public INotifySystem Inner { get; private set; }
        public Configuration Configuration { get; private set; }
        public IncrementalizationStrategy DefaultStrategy { get; set; }

        public RecordingNotifySystem(INotifySystem inner)
        {
            Inner = inner ?? NotifySystem.DefaultSystem;
            Configuration = new Configuration();
            DefaultStrategy = IncrementalizationStrategy.InstructionLevel;
        }

        protected virtual IEnumerable<IncrementalizationStrategy> GetApplicableStrategies(Expression expression, IEnumerable<ParameterExpression> parameters)
        {
            yield return IncrementalizationStrategy.InstructionLevel;
            yield return IncrementalizationStrategy.ListenRepositoryChanges;
            yield return IncrementalizationStrategy.ArgumentPromotion;
            yield return IncrementalizationStrategy.UseAugmentation;
        }

        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, false);
            return Inner.CreateExpression(expression, parameters, parameterMappings);
        }

        protected virtual void RecordExpressionUsage(Expression expression, IEnumerable<ParameterExpression> parameters, bool reversible)
        {
            var expressionConfig = new MethodConfiguration()
            {
                MethodIdentifier = expression.ToString(),
                Strategy = DefaultStrategy
            };
            expressionConfig.AllowedStrategies.AddRange(GetApplicableStrategies(expression, parameters));
            Configuration.MethodConfigurations.Add(expressionConfig);
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, false);
            return Inner.CreateExpression<T>(expression, parameters, parameterMappings);
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, true);
            return Inner.CreateReversableExpression<T>(expression, parameters, parameterMappings);
        }

        public ISuccessorList CreateSuccessorList() => new MultiSuccessorList();
    }
}
