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
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    yield break;
                case ExpressionType.MemberAccess:
                    var me = (MemberExpression)expression;
                    if (me.Expression == null || me.Expression.NodeType == ExpressionType.Parameter)
                    {
                        yield break;
                    }
                    break;
                case ExpressionType.New:
                    var ne = (NewExpression)expression;
                    if (ne.Arguments.All(arg => arg.NodeType == ExpressionType.Parameter))
                    {
                        yield break;
                    }
                    break;
                default:
                    break;
            }
            yield return IncrementalizationStrategy.ListenRepositoryChanges;
            yield return IncrementalizationStrategy.ArgumentPromotion;
            var finder = new AnchorFinder();
            finder.Visit(expression);
            if (finder.AnchorFound)
            {
                yield return IncrementalizationStrategy.UseAugmentation;
            }
        }

        private class AnchorFinder : ExpressionVisitor
        {
            public bool AnchorFound { get; private set; }

            protected override Expression VisitMember(MemberExpression node)
            {
                Visit(node.Expression);
                var anchors = node.Member.GetCustomAttributes(typeof(AnchorAttribute), true);
                AnchorFound |= (anchors != null && anchors.Length > 0);
                return node;
            }
        }

        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, false);
            return Inner.CreateExpression(expression, parameters, parameterMappings);
        }

        protected virtual void RecordExpressionUsage(Expression expression, IEnumerable<ParameterExpression> parameters, bool reversible)
        {
            var methodIdentifier = expression.ToString();
            if (!Configuration.MethodConfigurations.Any(c => c.MethodIdentifier == methodIdentifier))
            {
                var expressionConfig = new MethodConfiguration()
                {
                    MethodIdentifier = methodIdentifier,
                    Strategy = DefaultStrategy
                };
                expressionConfig.AllowedStrategies.AddRange(GetApplicableStrategies(expression, parameters));
                Configuration.MethodConfigurations.Add(expressionConfig);
            }
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
    }
}
