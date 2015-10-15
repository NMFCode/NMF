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
        private Expression VisitImplementedOperator(BinaryExpression node, string reverseOperator)
        {
            var leftSubtract = node.Method.DeclaringType.GetRuntimeMethod(reverseOperator, new Type[] { node.Type, node.Left.Type });
            MethodInfo rightSubtract;
            if (node.Left.Type == node.Right.Type)
            {
                rightSubtract = leftSubtract;
            }
            else
            {
                rightSubtract = node.Method.DeclaringType.GetRuntimeMethod(reverseOperator, new Type[] { node.Type, node.Right.Type });
            }
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


        private Type GetLeastGeneralCommonType(Type type1, Type type2)
        {
            var type1Info = type1.GetTypeInfo();
            var type2Info = type2.GetTypeInfo();
            if (type1Info.IsInterface)
            {
                if (type2Info.ImplementedInterfaces.Contains(type1)) return type1;
                if (type2Info.IsInterface)
                {
                    if (type1Info.ImplementedInterfaces.Contains(type2)) return type2;
                }
                return typeof(object);
            }
            else
            {
                if (type2Info.IsInterface)
                {
                    if (type1Info.ImplementedInterfaces.Contains(type2)) return type2;
                    return typeof(object);
                }
                TypeInfo current = type1Info;
                List<TypeInfo> types = new List<TypeInfo>();
                while (true)
                {
                    types.Add(current);
                    var baseType = current.BaseType;
                    if (baseType != null)
                    {
                        current = baseType.GetTypeInfo();
                    }
                    else
                    {
                        break;
                    }
                }
                current = type2Info;
                while (!types.Contains(current))
                {
                    current = current.BaseType.GetTypeInfo();
                }
                return current.AsType();
            }
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(node.Expression.Type.GetTypeInfo()) || IsObservableFuncType(node.Expression.Type, node.Arguments.Count))
            {
                var method = node.Expression.Type.GetRuntimeMethods().FirstOrDefault(m => m.Name == "Invoke");
                return VisitMethodCall(Expression.Call(node.Expression, method, node.Arguments));
            }
            throw new InvalidOperationException("Unclear what to invoke.");
        }

        private bool IsObservableFuncType(Type type, int arguments)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            return type.GetGenericTypeDefinition() == funcTypes[arguments];
        }
    }
}
