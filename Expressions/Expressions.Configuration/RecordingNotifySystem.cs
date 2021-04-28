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
    /// <summary>
    /// Denotes an incrementalization system that records any requests
    /// </summary>
    public class RecordingNotifySystem : INotifySystem
    {
        /// <summary>
        /// The inner incrementalization system
        /// </summary>
        public INotifySystem Inner { get; private set; }

        /// <summary>
        /// The recorded configuration
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// The default strategy to apply
        /// </summary>
        public IncrementalizationStrategy DefaultStrategy { get; set; }

        /// <summary>
        /// Creates a new recording incrementalization system
        /// </summary>
        /// <param name="inner">The inner incrementalization system</param>
        public RecordingNotifySystem(INotifySystem inner)
        {
            Inner = inner ?? NotifySystem.DefaultSystem;
            Configuration = new Configuration();
            DefaultStrategy = IncrementalizationStrategy.InstructionLevel;
        }

        /// <summary>
        /// Calculates the strategies applicable to incrementalize the given expression
        /// </summary>
        /// <param name="expression">The expression that shall be incrementalized</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>A colection of viable incrementalization strategies</returns>
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

        /// <inheritdoc />
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, false);
            return Inner.CreateExpression(expression, parameters, parameterMappings);
        }

        /// <summary>
        /// Records the usage of the given expression
        /// </summary>
        /// <param name="expression">The expression that was used</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="reversible">True, if the expression should be incrementalized reversable</param>
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

        /// <inheritdoc />
        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, false);
            return Inner.CreateExpression<T>(expression, parameters, parameterMappings);
        }

        /// <inheritdoc />
        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            RecordExpressionUsage(expression, parameters, true);
            return Inner.CreateReversableExpression<T>(expression, parameters, parameterMappings);
        }
    }
}
