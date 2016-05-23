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
    internal class PromotionExpressionVisitor : ExpressionVisitor
    {
        public struct ParameterInfo
        {
            public List<string> Properties { get; set; }

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
            if (type.IsValueType || type == typeof(string)) return true;
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

        private static bool HasAttribute(MemberInfo member, Type attributeType, bool inherit)
        {
            var attributes = member.GetCustomAttributes(attributeType, inherit);
            return attributes != null && attributes.Length > 0;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var property = node.Member as PropertyInfo;
            if (property != null)
            {
                var para = node.Expression as ParameterExpression;
                if (para == null)
                {
                    if (!IsContainmentProperty(property))
                    {
                        ParameterExtraction par;
                        var id = node.ToString();
                        if (!parameters.TryGetValue(id, out par))
                        {
                            var parameter = Expression.Parameter(node.Type, "arg" + parameters.Count.ToString());
                            parameters.Add(node.ToString(), new ParameterExtraction(parameter, node));
                            return parameter;
                        }
                        else
                        {
                            return par.Parameter;
                        }
                    }
                    else
                    {
                        var visitor = new ParameterCollector();
                        visitor.Visit(node.Expression);
                        foreach (var par in visitor.Parameters)
                        {
                            RegisterProperty(property, par, true);
                        }
                    }
                }
                else
                {
                    RegisterProperty(property, para, false);
                }
            }
            return base.VisitMember(node);
        }

        private void RegisterProperty(PropertyInfo property, ParameterExpression parameter, bool needsContainment)
        {
            ParameterInfo properties;
            if (!parameterInfo.TryGetValue(parameter.Name, out properties))
            {
                properties = new ParameterInfo()
                {
                    Properties = new List<string>(),
                    NeedContainments = false
                };
                parameterInfo.Add(parameter.Name, properties);
            }
            properties.Properties.Add(property.Name);
            if (needsContainment && !properties.NeedContainments)
            {
                properties.NeedContainments = true;
                parameterInfo[parameter.Name] = properties;
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (IsImmutableMethod(node.Method))
            {
                var changed = false;
                var arguments = new Expression[node.Arguments.Count];
                var i = 0;
                foreach (var arg in node.Arguments)
                {
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
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
