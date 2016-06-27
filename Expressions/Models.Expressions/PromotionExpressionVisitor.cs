using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NMF.Expressions
{
    internal class PromotionExpressionVisitor : ExpressionVisitorBase
    {
        public struct ParameterInfo
        {
            public ParameterInfo(bool needsContainment) : this()
            {
                NeedContainments = needsContainment;
                Properties = new HashSet<string>();
            }

            public HashSet<string> Properties { get; set; }

            public bool NeedContainments { get; set; }
        }

        private Dictionary<string, ParameterExtraction> parameters = new Dictionary<string, ParameterExtraction>();
        private Dictionary<string, ParameterInfo> parameterInfo = new Dictionary<string, ParameterInfo>();
        
        public ICollection<ParameterExtraction> ExtractParameters
        {
            get
            {
                return parameters.Values;
            }
        }

        public IDictionary<string, ParameterInfo> ParameterInfos
        {
            get
            {
                return parameterInfo;
            }
        }

        private static bool IsImmutableMethod(MethodInfo method)
        {
            var immutableMethodAttributes = method.GetCustomAttributes(typeof(ImmutableMethodAttribute), true);
            if (immutableMethodAttributes != null && immutableMethodAttributes.Length > 0) return true;
            return IsOnlyImmutableTypesAsArguments(method) || IsCollectionMethod(method);
        }

        private static bool IsCollectionMethod(MethodInfo method)
        {
            var type = method.DeclaringType;
            if (!type.IsGenericType) return false;
            type = type.GetGenericTypeDefinition();
            return type == typeof(ICollection<>) || type == typeof(IList<>);
        }

        private static bool IsOnlyImmutableTypesAsArguments(MethodInfo method)
        {
            if (!method.IsStatic && !IsImmutableType(method.DeclaringType)) return false;
            var parameters = method.GetParameters();
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    if (parameter.IsOut) return false;
                    if (!IsImmutableType(parameter.ParameterType)) return false;
                }
            }
            return true;
        }

        private static bool IsImmutableType(Type type)
        {
            if (type.IsValueType || type == typeof(string) || type == typeof(Uri)) return true;
            var immutableObjectAttributes = type.GetCustomAttributes(typeof(ImmutableObjectAttribute), false);
            if (immutableObjectAttributes != null && immutableObjectAttributes.Length > 0)
            {
                var immutable = (ImmutableObjectAttribute)immutableObjectAttributes[0];
                return immutable.Immutable;
            }
            var generatedTypeAttributes = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);
            if (generatedTypeAttributes != null && generatedTypeAttributes.Length > 0)
            {
                // Compiler-generated classes (especially used for LINQ) are treated as immutable as they are
                // usually taking inputs as constructor arguments
                return true;
            }
            return false;
        }

        private static bool IsContainmentProperty(PropertyInfo property)
        {
            if (property.DeclaringType.IsValueType) return true;
            return HasAttribute(property, typeof(ContainmentAttribute), true);
        }

        protected virtual IEnumerable<ParameterExpression> GetContainmentParameterDependencies(Expression expression)
        {
            // Look up along property tree
            var current = expression as MemberExpression;
            if (current != null)
            {
                var currentProperty = current.Member as PropertyInfo;
                while (currentProperty != null && IsContainmentProperty(currentProperty))
                {
                    var parameter = current.Expression as ParameterExpression;
                    if (parameter != null)
                    {
                        return new ParameterExpression[] { parameter };
                    }
                    current = current.Expression as MemberExpression;
                    if (current == null) return null;
                    currentProperty = current.Member as PropertyInfo;
                }
            }
            return null;
        }

        private static bool HasAttribute(MemberInfo member, Type attributeType, bool inherit)
        {
            var attributes = member.GetCustomAttributes(attributeType, inherit);
            return attributes != null && attributes.Length > 0;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ParameterExtraction extraction;
            if (parameters.TryGetValue(node.ToString(), out extraction))
            {
                return extraction.Parameter;
            }
            var property = node.Member as PropertyInfo;
            if (property != null)
            {
                var para = node.Expression as ParameterExpression;
                if (para == null)
                {
                    var containmentParameterDependencies = GetContainmentParameterDependencies(node.Expression);
                    if (containmentParameterDependencies != null)
                    {
                        foreach (var par in containmentParameterDependencies)
                        {
                            RegisterProperty(property, par, true);
                        }
                    }
                    else
                    {
                        ParameterInfo pInfo;
                        var par = ExtractParameter(node.Expression, out pInfo);
                        pInfo.Properties.Add(property.Name);
                        return node.Update(par);
                    }
                }
                else
                {
                    RegisterProperty(property, para, false);
                }
            }
            return base.VisitMember(node);
        }

        private Expression ExtractParameter(Expression node, out ParameterInfo pInfo)
        {
            ParameterExtraction par;
            var id = node.ToString();
            if (!parameters.TryGetValue(id, out par))
            {
                var parameter = Expression.Parameter(node.Type, "promotion_arg_" + parameters.Count.ToString());
                par = new ParameterExtraction(parameter, node);
                parameters.Add(id, par);
            }
            if (!parameterInfo.TryGetValue(par.Parameter.Name, out pInfo))
            {
                pInfo = new ParameterInfo(false);
                parameterInfo.Add(par.Parameter.Name, pInfo);
            }
            return par.Parameter;
        }

        private void RegisterProperty(PropertyInfo property, ParameterExpression parameter, bool needsContainment)
        {
            ParameterInfo properties;
            if (!parameterInfo.TryGetValue(parameter.Name, out properties))
            {
                properties = new ParameterInfo(needsContainment);
                parameterInfo.Add(parameter.Name, properties);
            }
            else if (needsContainment && !properties.NeedContainments)
            {
                properties.NeedContainments = true;
                parameterInfo[parameter.Name] = properties;
            }
            properties.Properties.Add(property.Name);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!IsImmutableMethod(node.Method))
            {
                var argumentRemaps = ReflectionHelper.GetCustomAttributes<ArgumentApplicationAttribute>(node.Method, true);
                if (argumentRemaps != null && argumentRemaps.Length > 0)
                {
                    return VisitRemappedMethodCall(node, node.Method, argumentRemaps);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Missing attribute for method {0}. Was this intended?", node.Method);
                }
            }

            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var arg = node.Arguments[i];
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
            }
            var obj = Visit(node.Object);
            changed |= obj != node.Object;
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
        }

        private int FindParameter(MethodInfo method, System.Reflection.ParameterInfo[] parameters, string name)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name == name) return i;
            }
            throw new InvalidOperationException(string.Format("The function {0} does not contain a parameter named {1}.", method.Name, name));
        }

        private Expression VisitRemappedMethodCall(MethodCallExpression node, MethodInfo method, ArgumentApplicationAttribute[] remaps)
        {
            var parametersSaved = new Dictionary<string, ParameterExtraction>(parameters);
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var extractionCounts = new int[node.Arguments.Count + 1];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                extractionCounts[i] = ExtractParameters.Count;
                var arg = node.Arguments[i];
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
            }
            extractionCounts[node.Arguments.Count] = ExtractParameters.Count;
            for (int i = 0; i < remaps.Length; i++)
            {
                var remap = remaps[i];
                if (!remap.ArgumentIsFunction)
                {
                    try
                    {
                        if (extractionCounts[remap.FunctionIndexParameter] != extractionCounts[remap.FunctionIndexParameter + 1])
                        {
                            parameters = parametersSaved;
                            ParameterInfo pInfo;
                            var extraction = ExtractParameter(node, out pInfo);
                            return extraction;
                        }

                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException(string.Format("The method {0} does not have an argument with index {1}.", node.Method, remap.FunctionIndexParameter));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            var obj = Visit(node.Object);
            changed |= obj != node.Object;
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
        }

        private LambdaExpression FindLambdaExpression(MethodCallExpression node, int argumentIndex)
        {
            try
            {
                var expression = node.Arguments[argumentIndex];
                switch (expression.NodeType)
                {
                    case ExpressionType.Lambda:
                        return (LambdaExpression)expression;
                    case ExpressionType.Quote:
                        var quote = (UnaryExpression)expression;
                        while (quote.Operand.NodeType == ExpressionType.Quote)
                        {
                            quote = (UnaryExpression)quote.Operand;
                        }
                        return (LambdaExpression)quote.Operand;
                    default:
                        throw new InvalidOperationException(string.Format("The argument {0} is not a function.", expression));
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException(string.Format("Invalid argument specification: The function {0} does not have an argument with index {1}.", node.Method.Name, argumentIndex));
            }
        }
    }
}
