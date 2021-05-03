using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// An expression visitor that turns getter functions into setters
    /// </summary>
    public class SetExpressionRewriter : ExpressionVisitor
    {
        private static readonly Type[] actionTypes =
        {
            typeof(Action<>),
            typeof(Action<,>),
            typeof(Action<,,>),
            typeof(Action<,,,>),
            typeof(Action<,,,,>),
            typeof(Action<,,,,,>)
        };

        /// <summary>
        /// Create a setter for the given getter expression
        /// </summary>
        /// <typeparam name="TValue">The return type of the getter expression</typeparam>
        /// <param name="getter">The getter expression</param>
        /// <returns>An expression that corresponds to the setter of the given getter</returns>
        public static Expression<Action<TValue>> CreateSetter<TValue>(Expression<Func<TValue>> getter)
        {
            if (getter == null) throw new ArgumentNullException("getter");
            var p = Expression.Parameter(typeof(TValue));
            var visitor = new SetExpressionRewriter(p);
            var body = visitor.Visit(getter.Body);
            if (body == null) return null;
            return Expression.Lambda<Action<TValue>>(body, p);
        }

        /// <summary>
        /// Create a setter for the given getter expression
        /// </summary>
        /// <typeparam name="T">The type of the first parameter</typeparam>
        /// <typeparam name="TValue">The return type of the getter expression</typeparam>
        /// <param name="getter">The getter expression</param>
        /// <returns>An expression that corresponds to the setter of the given getter</returns>
        public static Expression<Action<T, TValue>> CreateSetter<T, TValue>(Expression<Func<T, TValue>> getter)
        {
            if (getter == null) throw new ArgumentNullException("getter");
            var p = Expression.Parameter(typeof(TValue));
            var visitor = new SetExpressionRewriter(p);
            var body = visitor.Visit(getter.Body);
            if (body == null) return null;
            return Expression.Lambda<Action<T, TValue>>(body, getter.Parameters[0], p);
        }

        /// <summary>
        /// Create a setter for the given getter expression
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="TValue">The return type of the getter expression</typeparam>
        /// <param name="getter">The getter expression</param>
        /// <returns>An expression that corresponds to the setter of the given getter</returns>
        public static Expression<Action<T1, T2, TValue>> CreateSetter<T1, T2, TValue>( Expression<Func<T1, T2, TValue>> getter )
        {
            if(getter == null) throw new ArgumentNullException( "getter" );
            var p = Expression.Parameter( typeof( TValue ) );
            var visitor = new SetExpressionRewriter( p );
            var body = visitor.Visit( getter.Body );
            if(body == null) return null;
            return Expression.Lambda<Action<T1, T2, TValue>>( body, getter.Parameters[0], getter.Parameters[1], p );
        }

        /// <summary>
        /// Create a setter for the given getter expression
        /// </summary>
        /// <param name="getter">The getter expression</param>
        /// <returns>An expression that corresponds to the setter of the given getter</returns>
        public static LambdaExpression CreateSetter(LambdaExpression getter)
        {
            if (getter == null) throw new ArgumentNullException("getter");
            var valueParameter = Expression.Parameter(getter.ReturnType);
            var visitor = new SetExpressionRewriter(valueParameter);
            var body = visitor.Visit(getter.Body);
            if (body == null) return null;
            var parameters = new ParameterExpression[getter.Parameters.Count + 1];
            getter.Parameters.CopyTo(parameters, 0);
            parameters[parameters.Length - 1] = valueParameter;
            var typeParameters = new Type[getter.Parameters.Count + 1];
            for (int i = 0; i < getter.Parameters.Count; i++)
            {
                typeParameters[i] = getter.Parameters[i].Type;
            }
            typeParameters[typeParameters.Length - 1] = getter.ReturnType;
            var delegateType = actionTypes[getter.Parameters.Count].MakeGenericType(typeParameters);
            return Expression.Lambda(delegateType, body, parameters);
        }

        /// <summary>
        /// Gets the value of the rewrite
        /// </summary>
        public Expression Value { get; set; }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="value">The expression that is going to be inverted</param>
        public SetExpressionRewriter(Expression value)
        {
            if (value == null) throw new ArgumentNullException("value");

            this.Value = value;
        }

        /// <inheritdoc />
        public override Expression Visit(Expression node)
        {
            if (node == null) return null;
            return base.Visit(node);
        }

        /// <inheritdoc />
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Method != null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        return VisitImplementedBinary(node, "op_Subtraction", "op_Subtraction", Expression.Subtract);
                    case ExpressionType.AddChecked:
                        return VisitImplementedBinary(node, "op_Subtraction", "op_Subtraction", Expression.SubtractChecked);
                    case ExpressionType.Divide:
                        return VisitImplementedBinary(node, "op_Division", "op_Multiply", Expression.Multiply);
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        return VisitImplementedBinary(node, "op_Division", "op_Division", Expression.Divide);
                    case ExpressionType.Subtract:
                        return VisitImplementedBinary(node, "op_Subtraction", "op_Addition", Expression.Add);
                    case ExpressionType.SubtractChecked:
                        return VisitImplementedBinary(node, "op_Subtraction", "op_Addition", Expression.AddChecked);
                    default:
                        return null;
                }
            }
            var leftConstant = node.Left != null && node.Left.NodeType == ExpressionType.Constant;
            var rightConstant = node.Right != null && node.Right.NodeType == ExpressionType.Constant;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    if (leftConstant)
                    {
                        Value = Expression.Subtract(Value, node.Left);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.Subtract(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                case ExpressionType.AddChecked:
                    if (leftConstant)
                    {
                        Value = Expression.SubtractChecked(Value, node.Left);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.SubtractChecked(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                case ExpressionType.Divide:
                    if (leftConstant)
                    {
                        Value = Expression.Divide(node.Left, Value);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.MultiplyChecked(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                case ExpressionType.Equal:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    if (leftConstant)
                    {
                        Value = Expression.Divide(Value, node.Left);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.Divide(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Subtract:
                    if (leftConstant)
                    {
                        Value = Expression.Subtract(node.Left, Value);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.Add(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                case ExpressionType.SubtractChecked:
                    if (leftConstant)
                    {
                        Value = Expression.SubtractChecked(node.Left, Value);
                        return Visit(node.Right);
                    }
                    if (rightConstant)
                    {
                        Value = Expression.AddChecked(Value, node.Right);
                        return Visit(node.Left);
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        private Expression VisitImplementedBinary(BinaryExpression node, string reverseOperatorLeftConstant, string reverseOperatorRightConstant, Func<Expression, Expression, MethodInfo, BinaryExpression> expressionCreator)
        {
            var leftConstant = node.Left != null && node.Left.NodeType == ExpressionType.Constant;
            var rightConstant = node.Right != null && node.Right.NodeType == ExpressionType.Constant;

            if (leftConstant)
            {
                var reverseOp = ReflectionHelper.GetMethod(node.Method.DeclaringType, reverseOperatorLeftConstant,
                    new Type[] {
                        node.Type,
                        node.Left.Type
                    });

                if (reverseOp != null)
                {
                    Value = expressionCreator(node.Left, Value, reverseOp);
                    return Visit(node.Right);
                }
            }

            if (rightConstant)
            {
                var reverseOp = ReflectionHelper.GetMethod(node.Method.DeclaringType, reverseOperatorRightConstant,
                    new Type[] {
                        node.Type,
                        node.Right.Type
                    });

                if (reverseOp != null)
                {
                    Value = expressionCreator(Value, node.Right, reverseOp);
                    return Visit(node.Left);
                }
            }

            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var current = Value;
            var truePart = Visit(node.IfTrue);
            Value = current;
            var falsePart = Visit(node.IfFalse);
            if (truePart != null)
            {
                if (falsePart != null)
                {
                    return Expression.IfThenElse(node.Test, truePart, falsePart);
                }
                else
                {
                    return Expression.IfThen(node.Test, truePart);
                }
            }
            else
            {
                if (falsePart != null)
                {
                    return Expression.IfThen(Expression.Not(node.Test), falsePart);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <inheritdoc />
        protected override Expression VisitDefault(DefaultExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitIndex(IndexExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitListInit(ListInitExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            if(node.Member is PropertyInfo property)
            {
                if(!property.CanWrite || !property.SetMethod.IsPublic)
                {
                    return null;
                }
            }
            if (ReflectionHelper.IsAssignableFrom(node.Type, Value.Type))
            {
                return Expression.Assign(node, Value);
            }
            else
            {
                return Expression.Assign(node, Expression.Convert(Value, node.Type));
            }
        }

        /// <inheritdoc />
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var attributes = node.Method.GetCustomAttributes(typeof(SetExpressionRewriterAttribute), false);
            if (attributes != null)
            {
                if(attributes.FirstOrDefault() is SetExpressionRewriterAttribute proxyAttribute)
                {
                    MethodInfo proxyMethod;
                    if(!proxyAttribute.InitializeProxyMethod( node.Method, new Type[] { typeof( MethodCallExpression ), typeof( SetExpressionRewriter ) }, out proxyMethod ))
                    {
                        throw new InvalidOperationException( $"The given expression rewriter method for method {node.Method.Name} has the wrong signature. It must accept parameters of type MethodCallExpression and SetExpressionRewriter." );
                    }
                    else if(proxyMethod != null && proxyMethod.IsStatic && proxyMethod.ReturnType == typeof( Expression ))
                    {
                        var func = ReflectionHelper.CreateDelegate<Func<MethodCallExpression, SetExpressionRewriter, Expression>>( proxyMethod );
                        return func( node, this );
                    }
                }
            }
            attributes = node.Method.GetCustomAttributes(typeof(LensPutAttribute), false);
            if (attributes != null && node.Method.ReturnType != typeof(void) && Value != null)
            {
                if(attributes.FirstOrDefault() is LensPutAttribute lensPutAttribute)
                {
                    var typeList = new List<Type>( node.Method.GetParameters().Select( p => p.ParameterType ) );
                    if(!node.Method.IsStatic)
                    {
                        typeList.Insert( 0, node.Object.Type );
                    }
                    typeList.Add( node.Method.ReturnType );
                    if(!lensPutAttribute.InitializeProxyMethod( node.Method, typeList.ToArray(), out MethodInfo lensMethod ))
                    {
                        typeList.RemoveAt( 0 );
                        if(node.Method.IsStatic || !lensPutAttribute.InitializeProxyMethod( node.Method, typeList.ToArray(), out lensMethod ))
                        {
                            throw new InvalidOperationException( $"The lens put method for method {node.Method.Name} has the wrong signature. It must accept the same parameter types as the original method plus the return type of the original method." );
                        }
                        else if(lensMethod != null && !lensMethod.IsStatic)
                        {
                            var args = new List<Expression>( node.Arguments );
                            args.Add( Value );
                            var call = Expression.Call( node.Object, lensMethod, args );
                            if(lensMethod.ReturnType == typeof( void ))
                            {
                                Value = null;
                            }
                            else
                            {
                                Value = call;
                            }
                            return call;
                        }
                    }
                    else if(lensMethod != null && lensMethod.IsStatic)
                    {
                        var args = new List<Expression>( node.Arguments );
                        args.Add( Value );
                        if(!node.Method.IsStatic)
                        {
                            args.Insert( 0, node.Object );
                        }
                        var call = Expression.Call( lensMethod, args );
                        if(lensMethod.ReturnType == typeof( void ))
                        {
                            Value = null;
                            return call;
                        }
                        else
                        {
                            Value = call;
                            if(node.Method.IsStatic)
                            {
                                return Visit( node.Arguments[0] );
                            }
                            else
                            {
                                return Visit( node.Object );
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitNew(NewExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return null;
        }

        /// <inheritdoc />
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node == null || node.Method != null) return null;
            switch (node.NodeType)
            {
                case ExpressionType.Convert:
                    Value = Expression.Convert(Value, node.Operand.Type);
                    return Visit(node.Operand);
                case ExpressionType.ConvertChecked:
                    Value = Expression.ConvertChecked(Value, node.Operand.Type);
                    return Visit(node.Operand);
                case ExpressionType.Decrement:
                    Value = Expression.Increment(Value);
                    return Visit(node.Operand);
                case ExpressionType.Increment:
                    Value = Expression.Decrement(Value);
                    return Visit(node.Operand);
                case ExpressionType.Negate:
                    Value = Expression.Negate(Value);
                    return Visit(node.Operand);
                case ExpressionType.NegateChecked:
                    Value = Expression.NegateChecked(Value);
                    return Visit(node.Operand);
                case ExpressionType.Not:
                    Value = Expression.Not(Value);
                    return Visit(node.Operand);
                case ExpressionType.OnesComplement:
                    Value = Expression.OnesComplement(Value);
                    return Visit(node.Operand);
                case ExpressionType.Quote:
                case ExpressionType.Unbox:
                    return Visit(node.Operand);
                case ExpressionType.TypeAs:
                    return Visit( node.Operand );
                default:
                    break;
            }
            return null;
        }
    }
}
