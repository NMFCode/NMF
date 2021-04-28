using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incrementalization system that works by increasing the trees spanned by the inputs, such that all affected changes can be obtained through bubbled change notifications
    /// </summary>
    public class TreeExtensionNotifySystem : INotifySystem
    {
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

            var visitor = new TreeExtensionExpressionVisitor();
            visitor.Visit(modelFunc);
            var collectedParameterInfos = visitor.CollectParameterInfos();
            var parameterList = visitor.ListParameters();

            var types = new Type[parameterList.Count + 1];
            var args = new object[3 * parameterList.Count + 1];


            if (parameterMappings == null && modelFuncVisitor.ExtractParameters.Count > 0)
            {
                parameterMappings = new Dictionary<string, object>();
            }
            foreach (var ex in modelFuncVisitor.ExtractParameters)
            {
                parameterMappings.Add(ex.Parameter.Name, ModelNotifySystem.Instance.CreateExpression(ex.Value, parameters, parameterMappings));
            }

            var expressionLambda = Expression.Lambda(modelFunc, parameterList);
            var expressionCompiled = expressionLambda.Compile();

            args[0] = expressionCompiled;
            for (int i = 0; i < parameterList.Count; i++)
            {
                var parameter = parameterList[i];
                types[i] = parameter.Type;
                args[3 * i + 1] = CreateNotifyValue(parameter.Name, parameterMappings, parameter.Type);

                TreeExtensionExpressionVisitor.ParameterInfo parInfo;
                if (collectedParameterInfos.TryGetValue(parameter, out parInfo))
                {
                    args[3 * i + 3] = parInfo.Properties;
                    args[3 * i + 2] = parInfo.Anchors;
                }
                else
                {
                    args[3 * i + 3] = null;
                    args[3 * i + 2] = null;
                }
            }
            types[parameterList.Count] = returnType;

            var treeExtensionCallType = ObservableTreeExtensionCallTypes.Types[types.Length - 2].MakeGenericType(types);
            var constructor = treeExtensionCallType.GetConstructors()[0];
#if DEBUG
            if (constructor == null)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif
            return constructor.Invoke(args);
        }

        private object CreateNotifyValue(string name, IDictionary<string, object> parameterMappings, Type type)
        {
            object value;
            if (parameterMappings != null && parameterMappings.TryGetValue(name, out value))
            {
                if (value != null)
                {
                    if (value is INotifyExpression notifyValue) return notifyValue;
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

        /// <inheritdoc />
        public INotifyReversableExpression<T> CreateReversableExpression<T>(Expression expression, IEnumerable<ParameterExpression> parameters, IDictionary<string, object> parameterMappings)
        {
            throw new NotSupportedException();
        }
    }
}
