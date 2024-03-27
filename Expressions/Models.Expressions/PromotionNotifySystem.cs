using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NMF.Models;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incrementalization system based on argument promotion
    /// </summary>
    public class PromotionNotifySystem : INotifySystem
    {
        private static readonly Type[] promotionMethodCallTypes =
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

        /// <inheritdoc />
        public INotifyExpression CreateExpression(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return (INotifyExpression)CreateExpressionInternal(expression, parameters, parameterMappings, expression.Type);
        }

        /// <inheritdoc />
        public INotifyExpression<T> CreateExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            return (INotifyExpression<T>)CreateExpressionInternal(expression, parameters, parameterMappings, typeof(T));
        }

        private object CreateExpressionInternal(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings, Type returnType)
        {
            var modelFuncVisitor = new ModelFuncExpressionVisitor();
            var modelFunc = modelFuncVisitor.Visit(expression);

            var promotionVisitor = new PromotionExpressionVisitor();
            var promotionExpression = promotionVisitor.Visit(modelFunc);
            var collectedParameterInfos = promotionVisitor.CollectParameterInfos();
            var parametersNew = promotionVisitor.ListParameters();

            var newExpression = Expression.Lambda(promotionExpression, parametersNew);
            var newExpressionCompiled = newExpression.Compile();

            var types = new Type[parametersNew.Count + 1];
            var args = new object[3 * parametersNew.Count + 1];
            args[0] = newExpressionCompiled;
            
            if (parameterMappings == null && modelFuncVisitor.ExtractParameters.Count > 0)
            {
                parameterMappings = new Dictionary<string, object>();
            }
            foreach (var ex in modelFuncVisitor.ExtractParameters)
            {
#pragma warning disable S2259 // Null pointers should not be dereferenced
                parameterMappings.Add(ex.Parameter.Name, ModelNotifySystem.Instance.CreateExpression(ex.Value, parameters, parameterMappings));
#pragma warning restore S2259 // Null pointers should not be dereferenced
            }

            for (int i = 0; i < parametersNew.Count; i++)
            {
                var parameter = parametersNew[i];
                
                var extraction = promotionVisitor.ExtractParameters.FirstOrDefault(ex => ex.Parameter == parameter);
                if (extraction.Parameter == null)
                {
                    // The parameter is an original parameter
                    args[3 * i + 1] = PromotionNotifySystem.CreateNotifyValue(parameter.Name, parameterMappings, parameter.Type);
                    types[i] = parameter.Type;
                }
                else
                {
                    var val = extraction.Value;
                    if (!typeof(IModelElement).IsAssignableFrom(val.Type) && !typeof(IEnumerableExpression).IsAssignableFrom(val.Type) && !typeof(INotifyEnumerable).IsAssignableFrom(val.Type))
                    {
                        throw new NotSupportedException("Currently, only extraction of parameters typed as model elements or notifiable collections is supported.");
                    }
                    args[3 * i + 1] = ModelNotifySystem.Instance.CreateExpression(val, parameters, parameterMappings);
                    types[i] = val.Type;
                }
                PromotionExpressionVisitor.ParameterInfo parInfo;
                if (collectedParameterInfos.TryGetValue(parameter, out parInfo))
                {
                    args[3 * i + 2] = parInfo.Properties;
                    args[3 * i + 3] = parInfo.NeedsContainment;
                }
                else
                {
                    args[3 * i + 2] = null;
                    args[3 * i + 3] = false;
                }
            }
            types[types.Length - 1] = returnType;

            var promotionMethodCallType = promotionMethodCallTypes[types.Length - 2].MakeGenericType(types);
            var constructor = promotionMethodCallType.GetConstructors()[0];
#if DEBUG
            if (constructor == null)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif
#pragma warning disable S2259 // Null pointers should not be dereferenced
            return constructor.Invoke(args);
#pragma warning restore S2259 // Null pointers should not be dereferenced
        }

        private static object CreateNotifyValue(string name, IDictionary<string, object> parameterMappings, Type type)
        {
            if (parameterMappings != null && parameterMappings.TryGetValue(name, out object value))
            {
#pragma warning disable S4201 // Null checks should not be used with "is"
                if (value != null && value is INotifyExpression notifyValue) return notifyValue;
#pragma warning restore S4201 // Null checks should not be used with "is"

                var constantType = typeof(ObservableConstant<>).MakeGenericType(type);
                return Activator.CreateInstance(constantType, value);
            }
            else
            {
                var parameterType = typeof(ObservableParameter<>).MakeGenericType(type);
                return Activator.CreateInstance(parameterType, name);
            }
        }

        /// <inheritdoc />
        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotSupportedException("Reversable expressions are currently not supported.");
        }
    }
}
