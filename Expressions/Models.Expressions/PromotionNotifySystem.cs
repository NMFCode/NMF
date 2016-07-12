using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    public class PromotionNotifySystem : INotifySystem
    {
        private static Type[] promotionMethodCallTypes =
        {
            typeof(ObservablePromotionMethodCall<,>),
            typeof(ObservablePromotionMethodCall<,,>),
            typeof(ObservablePromotionMethodCall<,,,>),
            typeof(ObservablePromotionMethodCall<,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,,,,,>),
            typeof(ObservablePromotionMethodCall<,,,,,,,,,,,,,,,>)
        };

        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return (INotifyExpression)CreateExpressionInternal(expression, parameters, parameterMappings, expression.Type);
        }

        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return (INotifyExpression<T>)CreateExpressionInternal(expression, parameters, parameterMappings, typeof(T));
        }

        private object CreateExpressionInternal(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings, Type returnType)
        {
            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(expression);
            var parameterCollector = new ParameterCollector();
            parameterCollector.Visit(visited);
            var parametersNew = parameterCollector.Parameters.ToList();

            var newExpression = Expression.Lambda(visited, parametersNew);
            var newExpressionCompiled = newExpression.Compile();

            var types = new Type[parametersNew.Count + 1];
            var args = new object[3 * parametersNew.Count + 1];
            args[0] = newExpressionCompiled;
            for (int i = 0; i < parametersNew.Count; i++)
            {
                var parameter = parametersNew[i];
                types[i] = parameter.Type;
                var extraction = visitor.ExtractParameters.FirstOrDefault(ex => ex.Parameter == parameter);
                if (extraction.Parameter == null)
                {
                    // The parameter is an original parameter
                    args[3 * i + 1] = CreateNotifyValue(parameter.Name, parameterMappings, parameter.Type);
                }
                else
                {
                    args[3 * i + 1] = InstructionLevelNotifySystem.Instance.CreateExpression(extraction.Value, parameters, parameterMappings);
                }
                PromotionExpressionVisitor.ParameterInfo parInfo;
                if (visitor.ParameterInfos.TryGetValue(parameter.Name, out parInfo))
                {
                    args[3 * i + 2] = parInfo.Properties;
                    args[3 * i + 3] = parInfo.NeedContainments;
                }
                else
                {
                    args[3 * i + 2] = null;
                    args[3 * i + 3] = false;
                }
            }
            types[types.Length - 1] = returnType;

            var promotionMethodCallType = promotionMethodCallTypes[types.Length - 2].MakeGenericType(types);
            return Activator.CreateInstance(promotionMethodCallType, args);
        }

        private object CreateNotifyValue(string name, IDictionary<string, object> parameterMappings, Type type)
        {
            object value;
            if (parameterMappings != null && parameterMappings.TryGetValue(name, out value))
            {
                if (value != null)
                {
                    var notifyValue = value as INotifyExpression;
                    if (notifyValue != null) return notifyValue;
                }
                var constantType = typeof(ObservableConstant<>).MakeGenericType(type);
                return Activator.CreateInstance(constantType, value);
            }
            else
            {
                var parameterType = typeof(ObservableParameter<>);
                return Activator.CreateInstance(parameterType, new object[] { name });
            }
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotSupportedException("Reversable expressions are currently not supported.");
        }
    }
}
