using NMF.Collections.Generic;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ModelExpressionVisitorBase<TState> : ExpressionVisitorBase
    {
        public abstract class PropertyChainNode
        {
            public abstract ParameterExpression Parameter { get; }

            public abstract bool ContainsCrossReference();

            public abstract PropertyChainNode Rebase(PropertyChainNode newBase);

            public abstract Type Type { get; }

            public abstract PropertyChainNode Base { get; }
        }

        public class ParameterReference : PropertyChainNode
        {
            private ParameterExpression parameter;

            public ParameterReference(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }

            public override PropertyChainNode Base
            {
                get
                {
                    return null;
                }
            }

            public override ParameterExpression Parameter
            {
                get
                {
                    return parameter;
                }
            }

            public override Type Type
            {
                get
                {
                    return parameter.Type;
                }
            }

            public override bool ContainsCrossReference()
            {
                return false;
            }

            public override PropertyChainNode Rebase(PropertyChainNode newBase)
            {
                return newBase;
            }
        }

        public class PropertyAccess : PropertyChainNode
        {
            public PropertyAccess(PropertyChainNode baseNode, PropertyInfo property, bool isCrossReference)
            {
                this.baseNode = baseNode;
                Property = property;
                IsCrossReference = isCrossReference;
            }

            private PropertyChainNode baseNode;

            public override PropertyChainNode Base
            {
                get
                {
                    return baseNode;
                }
            }

            public override ParameterExpression Parameter
            {
                get
                {
                    return Base.Parameter;
                }
            }

            public PropertyInfo Property { get; private set; }

            public string PropertyName
            {
                get
                {
                    return Property.Name;
                }
            }

            public bool IsCrossReference { get; private set; }

            public bool IsNested { get; private set; }

            public override Type Type
            {
                get
                {
                    var collectionType = FindEnumerableExpressionType(Property.PropertyType);
                    if (collectionType != null)
                    {
                        return collectionType.GetGenericArguments()[0];
                    }
                    else
                    {
                        return Property.PropertyType;
                    }
                }
            }

            public Type Anchor
            {
                get
                {
                    var anchorAttributes = Property.GetCustomAttributes(typeof(AnchorAttribute), true);
                    if (anchorAttributes != null && anchorAttributes.Length > 0)
                    {
                        return ((AnchorAttribute)anchorAttributes[0]).AnchorType;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public override bool ContainsCrossReference()
            {
                if (IsCrossReference)
                {
                    var anchor = Anchor;
                    if (anchor != null)
                    {
                        var current = Base;
                        while (current != null)
                        {
                            if (anchor.IsAssignableFrom(current.Type))
                            {
                                return Base.ContainsCrossReference();
                            }
                            current = current.Base;
                        }
                    }
                    return true;
                }
                return Base.ContainsCrossReference();
            }

            public override PropertyChainNode Rebase(PropertyChainNode newBase)
            {
                return new PropertyAccess(Base.Rebase(newBase), Property, IsCrossReference);
            }
        }

        public List<ParameterExpression> ListParameters()
        {
            return propertyAccesses.Select(a => a.Parameter).Distinct().ToList();
        }
        
        private readonly LooselyLinkedList<PropertyChainNode> propertyAccesses = new LooselyLinkedList<PropertyChainNode>();

        protected ICollection<PropertyChainNode> PropertyAccesses
        {
            get
            {
                return propertyAccesses;
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
            var property = node.Member as PropertyInfo;
            if (property != null)
            {
                var newExpression = Visit(node.Expression);
                var isContainment = IsContainmentProperty(property);
                var isCrossReference = !isContainment && typeof(IModelElement).IsAssignableFrom(property.PropertyType);
                foreach (var propertyAccess in propertyAccesses.Nodes)
                {
                    var parameter = propertyAccess.Value.Parameter;
                    if (propertyAccess.Value.ContainsCrossReference() && node.Expression != parameter)
                    {
                        Expression returnVal;
                        if (ResetForCrossReference(node.Expression, property, isCrossReference, out returnVal))
                        {
                            return returnVal;
                        }
                    }
                    else
                    {
                        propertyAccess.Value = new PropertyAccess(propertyAccess.Value, property, isCrossReference);
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

        protected abstract bool ResetForCrossReference(Expression targetExpression, PropertyInfo property, bool isCrossReference, out Expression returnValue);

        protected abstract bool ResetForLambdaExpression(TState state, MethodCallExpression methodCall, LambdaExpression lambda, out Expression returnValue);

        protected virtual bool IsModelProperty(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            return typeof(IModelElement).IsAssignableFrom(property.DeclaringType);
        }


        private static bool IsGenericEnumerableExpression(Type type)
        {
            return type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerableExpression<>);
        }

        private static Type FindEnumerableExpressionType(Type type)
        {
            // Could be generic enumerable expression itself
            if (IsGenericEnumerableExpression(type))
            {
                return type;
            }
            // Otherwise, IEnumerableExpression is one of its implemented interfaces
            return type.GetInterfaces().FirstOrDefault(IsGenericEnumerableExpression);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var test = Visit(node.Test);
            var testPropertyAccesses = propertyAccesses.First.Next;
            propertyAccesses.Clear();
            var ifTrue = Visit(node.IfTrue);
            var truePropertyAccesses = propertyAccesses.First.Next;
            var ifFalse = Visit(node.IfFalse);
            if (truePropertyAccesses != null) propertyAccesses.AddFirst(truePropertyAccesses);
            if (testPropertyAccesses != null) propertyAccesses.AddFirst(testPropertyAccesses);
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
            propertyAccesses.Clear();
            var left = Visit(node.Left);
            var leftPropertyAccesses = propertyAccesses.First.Next;
            propertyAccesses.Clear();
            var conversion = node.Conversion != null ? Visit(node.Conversion) : null;
            if (leftPropertyAccesses != null) propertyAccesses.AddFirst(leftPropertyAccesses);
            if (rightPropertyAccesses != null) propertyAccesses.AddFirst(rightPropertyAccesses);
            if (left != node.Left || right != node.Right || conversion != node.Conversion)
            {
                return node.Update(left, node.Conversion, right);
            }
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            propertyAccesses.Add(new ParameterReference(node));
            return base.VisitParameter(node);
        }

        protected abstract TState SaveState();

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var state = SaveState();
            var remaps = ReflectionHelper.GetCustomAttributes<ParameterDataflowAttribute>(node.Method, true);
            var method = node.Method;
            var methodParameters = method.GetParameters();
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var propertyAccessArray = new LooselyLinkedListNode<PropertyChainNode>[node.Arguments.Count];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var arg = node.Arguments[i];
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
                propertyAccessArray[i] = propertyAccesses.First.Next;
                propertyAccesses.Clear();
            }
            Expression obj = node.Object;
            if (obj != null)
            {
                obj = Visit(node.Object);
                changed |= obj != node.Object;
            }
            var objectPropertyAccesses = propertyAccesses.First.Next;
            propertyAccesses.Clear();
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
                        var dummy = LooselyLinkedListNode<PropertyChainNode>.CreateDummyFor(accesses);
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
                                    LooselyLinkedListNode<PropertyChainNode> dependentParameters;
                                    if (dependency == ParameterDataflowAttribute.TargetObjectIndex)
                                    {
                                        dependentParameters = objectPropertyAccesses;
                                    }
                                    else
                                    {
                                        dependentParameters = propertyAccessArray[dependency];
                                    }
                                    foreach (var p in dependentParameters.FromHere)
                                    {
                                        var newNode = new LooselyLinkedListNode<PropertyChainNode>(access.Rebase(p));
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
                        propertyAccessArray[i] = dummy.Next;
                    }
                    Expression returnValue;
                    if (ResetForLambdaExpression(state, node, lambda, out returnValue))
                    {
                        return returnValue;
                    }
                }
                defaultList[i] = i;
            }
            if (objectPropertyAccesses != null)
            {
                propertyAccesses.AddFirst(objectPropertyAccesses);
            }
            for (int i = node.Arguments.Count - 1; i >= 0; i--)
            {
                if (propertyAccessArray[i] != null)
                {
                    propertyAccesses.AddFirst(propertyAccessArray[i]);
                }
            }
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
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
    }
}
