﻿using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ModelExpressionVisitor : ObservableExpressionBinder
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

        private static readonly Type[] functionTypes =
        {
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>)
        };

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var proxyTypes = node.Method.GetCustomAttributes(typeof(ObservableProxyAttribute), false);
            var dependencies = node.Method.GetCustomAttributes(typeof(ParameterDependencyAttribute), false);

            if (node.Method.IsStatic && (proxyTypes == null || proxyTypes.Length == 0) && (dependencies != null && dependencies.Length > 0))
            {
                var parameters = node.Method.GetParameters();
                if (parameters == null) throw new InvalidOperationException("Method has no parameter metadata");

                var arguments = new object[node.Arguments.Count * 3 + 1];
                var parameterIndices = new Dictionary<string, int>();
                var types = new Type[parameters.Length + 1];
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!typeof(IModelElement).IsAssignableFrom(parameters[i].ParameterType))
                    {
                        throw new InvalidOperationException($"The incrementalization of method {node.Method.Name} using parameter dependencies requires that the parameter is a model element.");
                    }
                    parameterIndices.Add(parameters[i].Name, i);
                    types[i] = parameters[i].ParameterType;
                }
                types[parameters.Length] = node.Method.ReturnType;

                var collections = new bool[node.Arguments.Count];
                var dependencyNames = new List<string>[node.Arguments.Count];
                foreach (ParameterDependencyAttribute dep in dependencies)
                {
                    try
                    {
                        var index = parameterIndices[dep.Parameter];
                        if (dependencyNames[index] == null)
                        {
                            dependencyNames[index] = new List<string>();
                        }
                        dependencyNames[index].Add(dep.Member);
                        collections[index] |= dep.IsNestedMember;
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new InvalidOperationException($"The parameter {dep.Parameter} does not exist for method {node.Method.Name}.");
                    }
                }

                arguments[0] = node.Method.CreateDelegate(functionTypes[node.Arguments.Count - 1].MakeGenericType(types));
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    arguments[3 * i + 1] = Visit(node.Arguments[i]);
                    arguments[3 * i + 2] = dependencyNames[i];
                    arguments[3 * i + 3] = collections[i];
                }

                return Activator.CreateInstance(promotionMethodCallTypes[node.Arguments.Count - 1].MakeGenericType(types), arguments) as Expression;
            }
            return base.VisitMethodCall(node);
        }
    }
}
