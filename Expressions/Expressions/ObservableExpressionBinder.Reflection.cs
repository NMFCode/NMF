using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal partial class ObservableExpressionBinder
    {
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            throw new NotSupportedException("Dynamic expressions are not supported");
        }

        private Expression VisitImplementedOperator(BinaryExpression node, string reverseOperator)
        {
            var leftSubtract = node.Method.DeclaringType.GetMethod(reverseOperator, BindingFlags.Public | BindingFlags.Static, null, new Type[] { node.Type, node.Left.Type }, null);
            var rightSubtract = node.Method.DeclaringType.GetMethod(reverseOperator, BindingFlags.Static | BindingFlags.Public, null, new Type[] { node.Type, node.Right.Type }, null);
            if (leftSubtract != null || rightSubtract != null)
            {
                return Activator.CreateInstance(typeof(ObservableReversableBinaryExpression<,,>).MakeGenericType(node.Left.Type, node.Right.Type, node.Type),
                    node, this, leftSubtract, rightSubtract) as Expression;
            }
            else
            {
                return VisitImplementedBinary(node);
            }
        }

        private Expression VisitImplementedOperator(BinaryExpression node, string leftReverseOperator, string rightReverseOperator)
        {
            var leftSubtract = node.Method.DeclaringType.GetMethod(leftReverseOperator, BindingFlags.Public | BindingFlags.Static, null, new Type[] { node.Left.Type, node.Type }, null);
            var rightSubtract = node.Method.DeclaringType.GetMethod(rightReverseOperator, BindingFlags.Static | BindingFlags.Public, null, new Type[] { node.Right.Type, node.Type }, null);
            if (leftSubtract != null || rightSubtract != null)
            {
                return Activator.CreateInstance(typeof(ObservableReversableBinaryExpression2<,,>).MakeGenericType(node.Left.Type, node.Right.Type, node.Type),
                    node, this, leftSubtract, rightSubtract) as Expression;
            }
            else
            {
                return VisitImplementedBinary(node);
            }
        }

        private Type GetLeastGeneralCommonType(Type type1, Type type2)
        {
            if (type1.IsInterface)
            {
                if (type2.GetInterfaces().Contains(type1)) return type1;
                if (type2.IsInterface)
                {
                    if (type1.GetInterfaces().Contains(type2)) return type2;
                }
                return typeof(object);
            }
            else
            {
                if (type2.IsInterface)
                {
                    if (type1.GetInterfaces().Contains(type2)) return type2;
                    return typeof(object);
                }
                Type current = type1;
                List<Type> types = new List<Type>();
                while (current != null)
                {
                    types.Add(current);
                    current = current.BaseType;
                }
                current = type2;
                while (!types.Contains(current))
                {
                    current = current.BaseType;
                }
                return current;
            }
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            if (typeof(Delegate).IsAssignableFrom(node.Expression.Type) || IsObservableFuncType(node.Expression.Type, node.Arguments.Count))
            {
                return VisitMethodCall(Expression.Call(node.Expression, node.Expression.Type.GetMethod("Invoke"), node.Arguments));
            }
            throw new InvalidOperationException("Unclear what to invoke.");
        }

        private bool IsObservableFuncType(Type type, int arguments)
        {
            if (!type.IsGenericType) return false;
            return type.GetGenericTypeDefinition() == ObservableExpressionTypes.ObservingFunc[arguments];
        }
    }
}
