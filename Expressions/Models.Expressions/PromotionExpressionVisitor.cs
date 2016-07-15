using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NMF.Utilities;
using System.Runtime.CompilerServices;
using NMF.Collections.Generic;

namespace NMF.Expressions
{
    internal class PromotionExpressionVisitor : ExpressionVisitorBase
    {
        public struct PropertyAccess
        {
            public PropertyAccess(ParameterExpression parameter, string propertyName, bool isCrossReference, bool isRecursive) : this()
            {
                Parameter = parameter;
                PropertyName = propertyName;
                IsCrossReference = isCrossReference;
                IsRecursive = isRecursive;
            }

            public ParameterExpression Parameter { get; private set; }

            public string PropertyName { get; private set; }

            public bool IsCrossReference { get; private set; }

            public bool IsRecursive { get; private set; }

            public bool IsNested { get; private set; }

            internal PropertyAccess ForParameter(ParameterExpression p)
            {
                return new PropertyAccess(p, PropertyName, IsCrossReference, true);
            }
        }

        public struct ParameterInfo
        {
            public ParameterInfo(List<string> properties, bool needsContainment)
            {
                NeedsContainment = needsContainment;
                Properties = properties;
            }

            public bool NeedsContainment { get; set; }

            public List<string> Properties { get; private set; }
        }

        private Dictionary<string, ParameterExtraction> parameterextractions = new Dictionary<string, ParameterExtraction>();
        private LooselyLinkedList<PropertyAccess> propertyAccesses = new LooselyLinkedList<PropertyAccess>();
        private LooselyLinkedList<ParameterExpression> parameterExpressions = new LooselyLinkedList<ParameterExpression>();
        
        public ICollection<ParameterExtraction> ExtractParameters
        {
            get
            {
                return parameterextractions.Values;
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

        private static bool HasAttribute(MemberInfo member, Type attributeType, bool inherit)
        {
            var attributes = member.GetCustomAttributes(attributeType, inherit);
            return attributes != null && attributes.Length > 0;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ParameterExtraction extraction;
            if (parameterextractions.TryGetValue(node.ToString(), out extraction))
            {
                return extraction.Parameter;
            }
            var property = node.Member as PropertyInfo;
            if (property != null)
            {
                var newExpression = Visit(node.Expression);
                var isContainment = IsContainmentProperty(property);
                var isCrossReference = !isContainment && typeof(IModelElement).IsAssignableFrom(property.PropertyType);
                foreach (var parameter in parameterExpressions)
                {
                    if (IsCrossreferenced(parameter) && node.Expression != parameter)
                    {
                        var extract = ExtractParameter(node.Expression);
                        propertyAccesses.Clear();
                        parameterExpressions.Clear();
                        parameterExpressions.Add(extract);
                        propertyAccesses.Add(new PropertyAccess(extract, property.Name, isCrossReference, false));
                        return Expression.MakeMemberAccess(extract, property);
                    }
                    else
                    {
                        propertyAccesses.Add(new PropertyAccess(parameter, property.Name, isCrossReference, false));
                    }
                }
                if (newExpression != node.Expression)
                {
                    return node.Update(newExpression);
                }
                return node;
            }
            else
            {
                return base.VisitMember(node);
            }
        }

        private bool IsCrossreferenced(ParameterExpression parameter)
        {
            foreach (var access in propertyAccesses)
            {
                if (access.Parameter == parameter && access.IsCrossReference)
                {
                    return true;
                }
            }
            return false;
        }

        private ParameterExpression ExtractParameter(Expression node)
        {
            ParameterExtraction par;
            var id = node.ToString();
            if (!parameterextractions.TryGetValue(id, out par))
            {
                var parameter = Expression.Parameter(node.Type, "promotion_arg_" + parameterextractions.Count.ToString());
                par = new ParameterExtraction(parameter, node);
                parameterextractions.Add(id, par);
            }
            return par.Parameter;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var test = Visit(node.Test);
            var testPropertyAccesses = propertyAccesses.First.Next;
            var testParameters = parameterExpressions.First.Next;
            propertyAccesses.Clear();
            parameterExpressions.Clear();
            var ifTrue = Visit(node.IfTrue);
            var truePropertyAccesses = propertyAccesses.First.Next;
            var trueParameters = parameterExpressions.First.Next;
            var ifFalse = Visit(node.IfFalse);
            if (truePropertyAccesses != null) propertyAccesses.AddFirst(truePropertyAccesses);
            if (testPropertyAccesses != null) propertyAccesses.AddFirst(testPropertyAccesses);
            if (trueParameters != null) parameterExpressions.AddFirst(trueParameters);
            if (testParameters != null) parameterExpressions.AddFirst(testParameters);
            if (test != node.Test || ifTrue != node.IfTrue || ifFalse != node.IfFalse)
            {
                return node.Update(test, ifTrue, ifFalse);
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var right = Visit(node.Right);
            var rightPropertyAccesses = propertyAccesses.First.Next;
            var rightParameters = parameterExpressions.First.Next;
            propertyAccesses.Clear();
            parameterExpressions.Clear();
            var left = Visit(node.Left);
            var leftPropertyAccesses = propertyAccesses.First.Next;
            var leftParameters = parameterExpressions.First.Next;
            propertyAccesses.Clear();
            parameterExpressions.Clear();
            var conversion = node.Conversion != null ? Visit(node.Conversion) : null;
            if (leftPropertyAccesses != null) propertyAccesses.AddFirst(leftPropertyAccesses);
            if (rightPropertyAccesses != null) propertyAccesses.AddFirst(rightPropertyAccesses);
            if (leftParameters != null) parameterExpressions.AddFirst(leftParameters);
            if (rightParameters != null) parameterExpressions.AddFirst(rightParameters);
            if (left != node.Left || right != node.Right || conversion != node.Conversion)
            {
                return node.Update(left, node.Conversion, right);
            }
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            parameterExpressions.Add(node);
            return base.VisitParameter(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var extractionsSaved = new Dictionary<string, ParameterExtraction>(parameterextractions);
            var remaps = ReflectionHelper.GetCustomAttributes<ParameterDataflowAttribute>(node.Method, true);
            var method = node.Method;
            var parametersSaved = new Dictionary<string, ParameterExtraction>(parameterextractions);
            var methodParameters = method.GetParameters();
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var propertyAccessArray = new LooselyLinkedListNode<PropertyAccess>[node.Arguments.Count];
            var parameterUsageArray = new LooselyLinkedListNode<ParameterExpression>[node.Arguments.Count];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var arg = node.Arguments[i];
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
                propertyAccessArray[i] = propertyAccesses.First.Next;
                propertyAccesses.Clear();
                parameterUsageArray[i] = parameterExpressions.First.Next;
                parameterExpressions.Clear();
            }
            Expression obj = node.Object;
            if (obj != null)
            {
                obj = Visit(node.Object);
                changed |= obj != node.Object;
            }
            var objectPropertyAccesses = propertyAccesses.First.Next;
            var objectParameters = parameterExpressions.First.Next;
            propertyAccesses.Clear();
            parameterExpressions.Clear();
            var defaultList = Enumerable.Range(0, node.Arguments.Count).ToList();
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var accesses = propertyAccessArray[i];
                if (accesses == null) continue;
                defaultList[i] = -2;
                var lambda = FindLambdaExpression(node.Arguments[i]);
                if (lambda != null)
                {
                    var remapsforLambda = remaps.Where(a => a.FunctionIndex == i);
                    var argumentMaps = new List<int>[lambda.Parameters.Count];
                    foreach (var remap in remapsforLambda)
                    {
                        List<int> remapForArgument = argumentMaps[remap.SourceIndex];
                        if (remapForArgument == null)
                        {
                            remapForArgument = new List<int>();
                            argumentMaps[remap.SourceIndex] = remapForArgument;
                        }
                        remapForArgument.Add(remap.FunctionParameterIndex);
                    }
                    for (int j = 0; j < lambda.Parameters.Count; j++)
                    {
                        var parameter = lambda.Parameters[j];
                        var dependencies = argumentMaps[j] ?? defaultList;
                        var dummy = LooselyLinkedListNode<PropertyAccess>.CreateDummyFor(accesses);
                        var current = dummy;
                        while (current.Next != null)
                        {
                            if (current.Next.Value.Parameter == parameter)
                            {
                                var access = current.Next.Value;
                                current.CutNext();
                                foreach (var dependency in dependencies)
                                {
                                    if (dependency == -2) continue;
                                    LooselyLinkedListNode<ParameterExpression> dependentParameters;
                                    if (dependency == ParameterDataflowAttribute.TargetObjectIndex)
                                    {
                                        dependentParameters = objectParameters;
                                    }
                                    else
                                    {
                                        dependentParameters = parameterUsageArray[dependency];
                                    }
                                    foreach (var p in dependentParameters.FromHere)
                                    {
                                        var newNode = new LooselyLinkedListNode<PropertyAccess>(access.ForParameter(p));
                                        propertyAccesses.AddAfter(current, newNode);
                                        current = newNode;
                                    }
                                }
                            }
                            else
                            {
                                current = current.Next;
                            }
                        }
                        accesses = dummy.Next;
                        propertyAccessArray[i] = accesses;
                    }
                    // Check extractions
                    if (!CheckConflictingExtractions(extractionsSaved, lambda))
                    {
                        parameterextractions = extractionsSaved;
                        var extraction = ExtractParameter(node);
                        parameterExpressions.Add(extraction);
                        return extraction;
                    }
                }
                defaultList[i] = i;
            }
            if (objectPropertyAccesses != null)
            {
                propertyAccesses.AddFirst(objectPropertyAccesses);
            }
            if (objectParameters != null)
            {
                parameterExpressions.AddFirst(objectParameters);
            }
            for (int i = node.Arguments.Count - 1; i >= 0 ; i--)
            {
                if (propertyAccessArray[i] != null)
                {
                    propertyAccesses.AddFirst(propertyAccessArray[i]);
                }
                if (parameterUsageArray[i] != null)
                {
                    parameterExpressions.AddFirst(parameterUsageArray[i]);
                }
            }
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
        }

        private bool CheckConflictingExtractions(Dictionary<string, ParameterExtraction> extractionsSaved, LambdaExpression lambda)
        {
            if (parameterextractions.Count > extractionsSaved.Count)
            {
                foreach (var extraction in parameterextractions)
                {
                    if (!extractionsSaved.ContainsKey(extraction.Key))
                    {
                        var parametersUsed = new ParameterCollector();
                        parametersUsed.Visit(extraction.Value.Value);
                        foreach (var parameter in lambda.Parameters)
                        {
                            if (parametersUsed.Parameters.Contains(parameter))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private int FindParameter(MethodInfo method, System.Reflection.ParameterInfo[] parameters, string name)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name == name) return i;
            }
            throw new InvalidOperationException(string.Format("The function {0} does not contain a parameter named {1}.", method.Name, name));
        }

        private LambdaExpression FindLambdaExpression(Expression expression)
        {
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
                    return null;
            }
        }

        public Dictionary<ParameterExpression, ParameterInfo> CollectParameterInfos()
        {
            var dict = new Dictionary<ParameterExpression, ParameterInfo>();
            foreach (var access in propertyAccesses)
            {
                ParameterInfo parameterInfo;
                if (!dict.TryGetValue(access.Parameter, out parameterInfo))
                {
                    parameterInfo = new ParameterInfo(new List<string>(), false);
                    dict.Add(access.Parameter, parameterInfo);
                }
                if (!parameterInfo.NeedsContainment && access.IsRecursive)
                {
                    parameterInfo.NeedsContainment = true;
                    dict[access.Parameter] = parameterInfo;
                }
                parameterInfo.Properties.Add(access.PropertyName);
            }
            return dict;
        }
    }
}
