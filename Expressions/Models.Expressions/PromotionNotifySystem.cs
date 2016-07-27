using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NMF.Models;

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
            var modelFuncVisitor = new ModelFuncExpressionVisitor();
            var modelFunc = modelFuncVisitor.Visit(expression);

            if (modelFuncVisitor.ExtractParameters.Count == 0)
            {
                var promotionVisitor = new PromotionExpressionVisitor();
                var promotionExpression = promotionVisitor.Visit(modelFunc);
                var collectedParameterInfos = promotionVisitor.CollectParameterInfos();
                var parametersNew = promotionVisitor.UsedParameters.Distinct().ToList();

                var newExpression = Expression.Lambda(promotionExpression, parametersNew);
                var newExpressionCompiled = newExpression.Compile();

                var types = new Type[parametersNew.Count + 1];
                var args = new object[3 * parametersNew.Count + 1];
                args[0] = newExpressionCompiled;
                for (int i = 0; i < parametersNew.Count; i++)
                {
                    var parameter = parametersNew[i];
                    types[i] = parameter.Type;
                    var extraction = promotionVisitor.ExtractParameters.FirstOrDefault(ex => ex.Parameter == parameter);
                    if (extraction.Parameter == null)
                    {
                        // The parameter is an original parameter
                        args[3 * i + 1] = CreateNotifyValue(parameter.Name, parameterMappings, parameter.Type);
                    }
                    else
                    {
                        var val = extraction.Value;
                        if (!typeof(IModelElement).IsAssignableFrom(val.Type) && !typeof(IEnumerableExpression).IsAssignableFrom(val.Type) && !typeof(INotifyEnumerable).IsAssignableFrom(val.Type))
                        {
                            throw new NotSupportedException("Currently, only extraction of parameters typed as model elements or notifiable collections is supported.");
                        }
                        args[3 * i + 1] = InstructionLevelNotifySystem.Instance.CreateExpression(val, parameters, parameterMappings);
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
                return constructor.Invoke(args);
            }
            else
            {
                /*
                var baseParameters = parameters.ToList();
                var types = new Type[baseParameters.Count + parametersNew.Count + 1];
                var args = new object[baseParameters.Count + 3 * parametersNew.Count + 1];
                args[args.Length - 1] = newExpressionCompiled;
                for (int i = 0; i < baseParameters.Count; i++)
                {
                }
                for (int i = 0; i < parametersNew.Count; i++)
                {
                    var parameter = parametersNew[i];
                    types[i] = parameter.Type;
                    var extraction = promotionVisitor.ExtractParameters.FirstOrDefault(ex => ex.Parameter == parameter);
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
                */
                throw new NotImplementedException();
            }
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
                var parameterType = typeof(ObservableParameter<>).MakeGenericType(type);
                return Activator.CreateInstance(parameterType, new object[] { name });
            }
        }

        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotSupportedException("Reversable expressions are currently not supported.");
        }
    }
}
