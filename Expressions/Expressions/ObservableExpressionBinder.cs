using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMF.Expressions.Arithmetics;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;

namespace NMF.Expressions
{
    internal partial class ObservableExpressionBinder : ExpressionVisitor
    {
        private bool compress;
        private readonly Dictionary<string, object> parameters;

        private static readonly MethodInfo memberBindingCreateProperty = ReflectionHelper.GetFunc<MemberAssignment, ObservableExpressionBinder, INotifyExpression<object>, ObservableMemberBinding<object>>((node, binder, target) => CreateProperty<object, object>(node, binder, target)).GetGenericMethodDefinition();

        public ObservableExpressionBinder(bool compress = false, IDictionary<string, object> parameterMappings = null)
        {
            this.compress = compress;
            this.parameters = parameterMappings != null ? new Dictionary<string, object>(parameterMappings) : new Dictionary<string, object>();
        }

        public bool Compress
        {
            get
            {
                return compress;
            }
            set
            {
                compress = value;
            }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Method != null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        if (node.Type == typeof(string) && node.Left.Type == typeof(string) && node.Right.Type == typeof(string))
                        {
                            return new ObservableStringPlus(VisitObservable<string>(node.Left), VisitObservable<string>(node.Right));
                        }
                        return VisitImplementedOperator(node, "op_Subtraction");
                    case ExpressionType.Divide:
                        return VisitImplementedOperator(node, "op_Division", "op_Multiply");
                    case ExpressionType.ExclusiveOr:
                        break;
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        return VisitImplementedOperator(node, "op_Division");
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        return VisitImplementedOperator(node, "op_Subtraction", "op_Addition");
                    default:
                        return VisitImplementedBinary(node);
                }
            }
            switch (node.NodeType)
            {
                case ExpressionType.And:
                    if (node.Type == typeof(bool))
                    {
                        return new ObservableLogicAnd(VisitObservable<bool>(node.Left), VisitObservable<bool>(node.Right));
                    }
                    else if (node.Type == typeof(int))
                    {
                        return new ObservableIntBitwiseAnd(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(uint))
                    {
                        return new ObservableUIntBitwiseAnd(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
                    }
                    else if (node.Type == typeof(long))
                    {
                        return new ObservableLongBitwiseAnd(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
                    }
                    else if (node.Type == typeof(ulong))
                    {
                        return new ObservableULongBitwiseAnd(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
                    }
                    break;
                case ExpressionType.AndAlso:
                    return VisitAndAlso(node);
                case ExpressionType.ExclusiveOr:
                    if (node.Type == typeof(bool))
                    {
                        return new ObservableLogicXor(VisitObservable<bool>(node.Left), VisitObservable<bool>(node.Right));
                    }
                    else if (node.Type == typeof(int))
                    {
                        return new ObservableIntBitwiseXor(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(uint))
                    {
                        return new ObservableUIntBitwiseXor(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
                    }
                    else if (node.Type == typeof(long))
                    {
                        return new ObservableLongBitwiseXor(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
                    }
                    else if (node.Type == typeof(ulong))
                    {
                        return new ObservableULongBitwiseXor(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
                    }
                    break;
                case ExpressionType.Or:
                    if (node.Type == typeof(bool))
                    {
                        return new ObservableLogicOr(VisitObservable<bool>(node.Left), VisitObservable<bool>(node.Right));
                    }
                    else if (node.Type == typeof(int))
                    {
                        return new ObservableIntBitwiseOr(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(uint))
                    {
                        return new ObservableUIntBitwiseOr(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
                    }
                    else if (node.Type == typeof(long))
                    {
                        return new ObservableLongBitwiseOr(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
                    }
                    else if (node.Type == typeof(ulong))
                    {
                        return new ObservableULongBitwiseOr(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
                    }
                    break;
                case ExpressionType.OrElse:
                    var left = VisitObservable<bool>(node.Left);
                    var oldCompress = compress;
                    compress = false;
                    var right = VisitObservable<bool>(node.Right);
                    compress = oldCompress;
                    return new ObservableLogicOrElse(left, right);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return VisitAdd(node);
                case ExpressionType.ArrayIndex:
                    if (node.Right.Type == typeof(int))
                    {
                        return Activator.CreateInstance(typeof(ObservableIntArrayIndex<>).MakeGenericType(node.Type), node, this) as Expression;
                    }
                    if (node.Right.Type == typeof(long))
                    {
                        return Activator.CreateInstance(typeof(ObservableLongArrayIndex<>).MakeGenericType(node.Type), node, this) as Expression;
                    }
                    break;
                case ExpressionType.Coalesce:
                    return Activator.CreateInstance(typeof(ObservableCoalesceExpression<>).MakeGenericType(node.Type), node, this) as Expression;
                case ExpressionType.Divide:
                    return VisitDivide(node);
                case ExpressionType.Equal:
                    return CreateExpression(typeof(ObservableEquals<>), node);
                case ExpressionType.GreaterThan:
                    return CreateExpression(typeof(ObservableGreatherThan<>), node);
                case ExpressionType.GreaterThanOrEqual:
                    return CreateExpression(typeof(ObservableGreatherThanOrEquals<>), node);
                case ExpressionType.LeftShift:
                    if (node.Type == typeof(int))
                    {
                        return new ObservableIntLeftShift(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(uint))
                    {
                        return new ObservableUIntLeftShift(VisitObservable<uint>(node.Left), VisitObservable<int>(node.Right));
                    }
                    if (node.Type == typeof(long))
                    {
                        return new ObservableLongLeftShift(VisitObservable<long>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(ulong))
                    {
                        return new ObservableULongLeftShift(VisitObservable<ulong>(node.Left), VisitObservable<int>(node.Right));
                    }
                    break;
                case ExpressionType.LessThan:
                    return CreateExpression(typeof(ObservableLessThan<>), node);
                case ExpressionType.LessThanOrEqual:
                    return CreateExpression(typeof(ObservableLessThanOrEquals<>), node);
                case ExpressionType.Modulo:
                    return VisitModulo(node);
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return VisitMultiply(node);
                case ExpressionType.NotEqual:
                    return CreateExpression(typeof(ObservableNotEquals<>), node);
                case ExpressionType.Power:
                    break;
                case ExpressionType.RightShift:
                    if (node.Type == typeof(int))
                    {
                        return new ObservableIntRightShift(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(uint))
                    {
                        return new ObservableUIntRightShift(VisitObservable<uint>(node.Left), VisitObservable<int>(node.Right));
                    }
                    if (node.Type == typeof(long))
                    {
                        return new ObservableLongRightShift(VisitObservable<long>(node.Left), VisitObservable<int>(node.Right));
                    }
                    else if (node.Type == typeof(ulong))
                    {
                        return new ObservableULongRightShift(VisitObservable<ulong>(node.Left), VisitObservable<int>(node.Right));
                    }
                    break;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return VisitSubtract(node);
                default:
                    break;
            }
            throw new NotSupportedException();
        }

        private Expression VisitImplementedBinary(BinaryExpression node)
        {
            return Activator.CreateInstance(typeof(ObservableBinaryExpression<,,>).MakeGenericType(node.Left.Type, node.Right.Type, node.Type),
                node, this) as Expression;
        }

        private Expression VisitModulo(BinaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntModulo(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
            }
            else if (node.Type == typeof(long))
            {
                return new ObservableLongModulo(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
            }
            else if (node.Type == typeof(uint))
            {
                return new ObservableUIntModulo(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
            }
            else if (node.Type == typeof(ulong))
            {
                return new ObservableULongModulo(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
            }
            else if (node.Type == typeof(float))
            {
                return new ObservableFloatModulo(VisitObservable<float>(node.Left), VisitObservable<float>(node.Right));
            }
            else if (node.Type == typeof(double))
            {
                return new ObservableDoubleModulo(VisitObservable<double>(node.Left), VisitObservable<double>(node.Right));
            }
            throw new NotSupportedException();
        }

        private Expression VisitSubtract(BinaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntMinus(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
            }
            else if (node.Type == typeof(long))
            {
                return new ObservableLongMinus(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
            }
            else if (node.Type == typeof(uint))
            {
                return new ObservableUIntMinus(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
            }
            else if (node.Type == typeof(ulong))
            {
                return new ObservableULongMinus(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
            }
            else if (node.Type == typeof(float))
            {
                return new ObservableFloatMinus(VisitObservable<float>(node.Left), VisitObservable<float>(node.Right));
            }
            else if (node.Type == typeof(double))
            {
                return new ObservableDoubleMinus(VisitObservable<double>(node.Left), VisitObservable<double>(node.Right));
            }
            throw new NotSupportedException();
        }

        private Expression VisitMultiply(BinaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntMultiply(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
            }
            else if (node.Type == typeof(long))
            {
                return new ObservableLongMultiply(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
            }
            else if (node.Type == typeof(uint))
            {
                return new ObservableUIntMultiply(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
            }
            else if (node.Type == typeof(ulong))
            {
                return new ObservableULongMultiply(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
            }
            else if (node.Type == typeof(float))
            {
                return new ObservableFloatMultiply(VisitObservable<float>(node.Left), VisitObservable<float>(node.Right));
            }
            else if (node.Type == typeof(double))
            {
                return new ObservableDoubleMultiply(VisitObservable<double>(node.Left), VisitObservable<double>(node.Right));
            }
            throw new NotSupportedException();
        }

        private Expression CreateExpression(Type expressionType, BinaryExpression node)
        {
            return System.Activator.CreateInstance(expressionType
                        .MakeGenericType(new Type[] { GetLeastGeneralCommonType(node.Left.Type, node.Right.Type) }),
                        node, this) as Expression;
        }

        private Expression VisitDivide(BinaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntDivide(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
            }
            else if (node.Type == typeof(long))
            {
                return new ObservableLongDivide(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
            }
            else if (node.Type == typeof(uint))
            {
                return new ObservableUIntDivide(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
            }
            else if (node.Type == typeof(ulong))
            {
                return new ObservableULongDivide(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
            }
            else if (node.Type == typeof(float))
            {
                return new ObservableFloatDivide(VisitObservable<float>(node.Left), VisitObservable<float>(node.Right));
            }
            else if (node.Type == typeof(double))
            {
                return new ObservableDoubleDivide(VisitObservable<double>(node.Left), VisitObservable<double>(node.Right));
            }
            throw new NotSupportedException();
        }

        private Expression VisitAdd(BinaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntPlus(VisitObservable<int>(node.Left), VisitObservable<int>(node.Right));
            }
            else if (node.Type == typeof(long))
            {
                return new ObservableLongPlus(VisitObservable<long>(node.Left), VisitObservable<long>(node.Right));
            }
            else if (node.Type == typeof(uint))
            {
                return new ObservableUIntPlus(VisitObservable<uint>(node.Left), VisitObservable<uint>(node.Right));
            }
            else if (node.Type == typeof(ulong))
            {
                return new ObservableULongPlus(VisitObservable<ulong>(node.Left), VisitObservable<ulong>(node.Right));
            }
            else if (node.Type == typeof(float))
            {
                return new ObservableFloatPlus(VisitObservable<float>(node.Left), VisitObservable<float>(node.Right));
            }
            else if (node.Type == typeof(double))
            {
                return new ObservableDoublePlus(VisitObservable<double>(node.Left), VisitObservable<double>(node.Right));
            }
            throw new NotSupportedException();
        }

        private Expression VisitAndAlso(BinaryExpression node)
        {
            var left = VisitObservable<bool>(node.Left);
            var oldCompress = compress;
            compress = false;
            var right = VisitObservable<bool>(node.Right);
            compress = oldCompress;
            return new ObservableLogicAndAlso(left, right);
        }

        private Expression VisitOrElse(BinaryExpression node)
        {
            var left = VisitObservable<bool>(node.Left);
            var oldCompress = compress;
            compress = false;
            var right = VisitObservable<bool>(node.Right);
            compress = oldCompress;
            return new ObservableLogicOrElse(left, right);
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return System.Activator.CreateInstance(typeof(ObservableConditionalExpression<>).MakeGenericType(node.Type), node, this) as Expression;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return System.Activator.CreateInstance(typeof(ObservableConstant<>).MakeGenericType(node.Type), node.Value) as Expression;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException("Please report the case under which you hit this error message to georg.hinkel@studentpartners.de");
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            object defaultValue;
            if (!ReflectionHelper.IsValueType(node.Type))
            {
                defaultValue = null;
            }
            else
            {
                defaultValue = Activator.CreateInstance(node.Type);
            }
            return Activator.CreateInstance(typeof(ObservableConstant<>).MakeGenericType(node.Type), defaultValue) as Expression;
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitExtension(Expression node)
        {
            return node;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (parameters == null || parameters.Count == 0) return new ObservableLambdaExpression<T>(node);
            var parameterMappings = new Dictionary<string, object>(this.parameters);
            foreach (var par in node.Parameters)
            {
                parameterMappings.Remove(par.Name);
            }
            var visitor = new ApplyParametersVisitor(parameterMappings);
            var applied = visitor.Visit(node);
            return new ObservableLambdaExpression<T>((Expression<T>)applied);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            if (node.Initializers.Count == 0) return Visit(node.NewExpression);
            return System.Activator.CreateInstance(typeof(ObservableListInit<>).MakeGenericType(node.Type), node, this) as Expression;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var property = node.Member as PropertyInfo;
            if (property != null)
            {
                if (ReflectionHelper.GetGetter(property).IsStatic)
                {
                    return VisitConstant(Expression.Constant(property.GetValue(null, null), property.PropertyType));
                }
                return VisitProperty(node, property);
            }
            var field = node.Member as FieldInfo;
            if (field != null)
            {
                if (field.IsStatic)
                {
                    return VisitConstant(Expression.Constant(field.GetValue(null), field.FieldType));
                }
                return VisitField(node, field);
            }
            throw new NotSupportedException();
        }

        private Expression VisitField(MemberExpression node, FieldInfo field)
        {
            return System.Activator.CreateInstance(typeof(ObservableReversableMemberExpression<,>).MakeGenericType(field.DeclaringType, field.FieldType),
                node, this, field.Name, field) as Expression;
        }

        private Expression VisitProperty(MemberExpression node, PropertyInfo property)
        {
            object getter;
            if (!ReflectionHelper.IsValueType(property.DeclaringType))
            {
                getter = ReflectionHelper.CreateDelegate(typeof(Func<,>).MakeGenericType(property.DeclaringType, property.PropertyType), ReflectionHelper.GetGetter(property));
            }
            else
            {
                //TODO: This is a bug in the BCL, code here to get out of this shit
                var param = Expression.Parameter(property.DeclaringType);
                var expression = Expression.Lambda(Expression.Property(param, property), param);
                getter = expression.Compile();
            }
            if (property.CanWrite)
            {
                var setter = ReflectionHelper.GetSetter(property);
                if (setter != null)
                {
                    if (!ReflectionHelper.IsValueType(property.DeclaringType))
                    {
                        return System.Activator.CreateInstance(typeof(ObservableReversableMemberExpression<,>).MakeGenericType(property.DeclaringType, property.PropertyType),
                            node, this, property.Name, getter, ReflectionHelper.CreateDelegate(typeof(Action<,>).MakeGenericType(property.DeclaringType, property.PropertyType), setter)) as Expression;
                    }
                    else
                    {
                        var setParam1 = Expression.Parameter(property.DeclaringType);
                        var setParam2 = Expression.Parameter(property.PropertyType);
                        var setExpression = Expression.Lambda(Expression.Assign(Expression.Property(setParam1, property), setParam2), setParam1, setParam2);

                        return System.Activator.CreateInstance(typeof(ObservableReversableMemberExpression<,>).MakeGenericType(property.DeclaringType, property.PropertyType),
                            node, this, property.Name, getter, setExpression.Compile()) as Expression;
                    }
                }
            }
            return System.Activator.CreateInstance(typeof(ObservableMemberExpression<,>).MakeGenericType(property.DeclaringType, property.PropertyType),
                node, this, property.Name, getter) as Expression;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            if (node.Bindings.Count == 0) return Visit(node.NewExpression);
            return System.Activator.CreateInstance(typeof(ObservableMemberInit<>).MakeGenericType(node.Type), node, this) as Expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (typeof(Delegate).IsAssignableFrom(node.Method.DeclaringType) && node.Method.Name == "Invoke")
            {
                // in that case, we need to insert a proxy node to infer proxies of the actual method that is being executed
            }

            var staticMethod = node.Method.IsStatic;
            var typeOffset = staticMethod ? 0 : 1;
            var typesLength = 1 + node.Arguments.Count + typeOffset;
            if (typesLength == 1 && staticMethod)
            {
                // we cannot get any information anyhow, so treat the result as constant
                return Activator.CreateInstance(typeof(ObservableConstant<>).MakeGenericType(node.Type), node.Method.Invoke(null, null)) as Expression;
            }

            var types = new Type[typesLength];
            var typesArg = new Type[node.Arguments.Count];
            var typesArgInc = new Type[node.Arguments.Count];
            var typesArgStatic = new Type[node.Arguments.Count + typeOffset];
            var typesArgStaticInc = new Type[node.Arguments.Count + typeOffset];
            var incType = typeof(INotifyValue<>);
            if (!staticMethod)
            {
                types[0] = node.Object.Type;
                typesArgStatic[0] = node.Object.Type;
                typesArgStaticInc[0] = incType.MakeGenericType(node.Object.Type);
            }
            var parameters = node.Method.GetParameters();
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                var t = parameters[i].ParameterType;
                types[i + typeOffset] = t;
                typesArg[i] = t;
                typesArgInc[i] = incType.MakeGenericType(t);
                typesArgStatic[i + typeOffset] = t;
                typesArgStaticInc[i + typeOffset] = typesArgInc[i];
            }
            types[node.Arguments.Count + typeOffset] = node.Method.ReturnType;

            var proxyTypes = node.Method.GetCustomAttributes(typeof(ObservableProxyAttribute), false);
            if (proxyTypes != null)
            {
                if(proxyTypes.FirstOrDefault() is ObservableProxyAttribute proxyAttribute)
                {
                    return VisitProxyMethodCall( node, types, typesArg, typesArgInc, typesArgStatic, typesArgStaticInc, proxyAttribute );
                }
            }
            if (node.Object != null && typeof(Delegate).IsAssignableFrom(node.Object.Type) && node.Method.Name == "Invoke")
            {
                return VisitDelegateInvokeExpression(node, types);
            }
            return VisitStandardMethodCall(node, types);
        }

        private Expression VisitDelegateInvokeExpression(MethodCallExpression node, Type[] types)
        {
            var proxyType = ObservableExpressionTypes.DelegateProxyTypes[node.Arguments.Count - 1];
            var typeArgs = new Type[node.Arguments.Count + 1];
            Array.Copy(types, 1, typeArgs, 0, typeArgs.Length);
            return Activator.CreateInstance(proxyType.MakeGenericType(typeArgs), node, this) as Expression;
        }

        private Expression VisitStandardMethodCall(MethodCallExpression node, Type[] types)
        {
            var lensAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute));
            Type[] methodArray;
            if (node.Method.IsStatic)
            {
                if (lensAttribute != null)
                {
                    methodArray = ObservableExpressionTypes.ObservableStaticLensMethodCall;
                }
                else
                {
                    methodArray = ObservableExpressionTypes.ObservableStaticMethodCall;
                }
            }
            else
            {
                if (lensAttribute != null)
                {
                    methodArray = ObservableExpressionTypes.ObservableLensMethodCall;
                }
                else
                {
                    methodArray = ObservableExpressionTypes.ObservableMethodCall;
                }
            }
            return System.Activator.CreateInstance(methodArray[types.Length - 2].MakeGenericType(types), node, this) as Expression;
        }

        private Expression VisitProxyMethodCall(MethodCallExpression node, Type[] types, Type[] typesArg, Type[] typesArgInc, Type[] typesArgStatic, Type[] typesArgStaticInc, ObservableProxyAttribute proxyAttribute)
        {
            var lensAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute));
            MethodInfo proxyMethod;
            if (!node.Method.IsStatic)
            {
                if (proxyAttribute.InitializeProxyMethod(node.Method, typesArgInc, out proxyMethod))
                {
                    return VisitIncrementalMemberProxyCall(node, lensAttribute, types, proxyMethod, proxyAttribute.IsRecursive);
                }
                else if (proxyAttribute.InitializeProxyMethod(node.Method, typesArg, out proxyMethod))
                {
                    return VisitStaticMemberProxyCall(node, types.Length, lensAttribute, types, proxyMethod);
                }
            }
            if (proxyAttribute.InitializeProxyMethod(node.Method, typesArgStaticInc, out proxyMethod))
            {
                return VisitIncrementalStaticProxyCall(node, lensAttribute, proxyMethod, proxyAttribute.IsRecursive);
            }
            else if (proxyAttribute.InitializeProxyMethod(node.Method, typesArgStatic, out proxyMethod))
            {
                return VisitStaticProxyCall(node, types.Length, lensAttribute, types, proxyMethod);
            }
            else
            {
                throw new NotSupportedException($"The parameters of the proxy method {proxyAttribute.MethodName} are invalid. Parameters must match the original method {node.Method.Name} or all parameters must be converted to monads.");
            }
        }

        private Expression VisitStaticProxyCall(MethodCallExpression node, int typesLength, Attribute lensAttribute, Type[] types, MethodInfo proxyMethod)
        {
            if (!proxyMethod.IsStatic)
            {
                throw new InvalidOperationException("The provided proxy method must be static or the target parameter must be omitted.");
            }
            CheckForOutParameter(proxyMethod.GetParameters());
            CheckReturnTypeIsCorrect(node, proxyMethod);
            var typeArray = lensAttribute == null ? ObservableExpressionTypes.ObservableStaticProxyCall : ObservableExpressionTypes.ObservableStaticLensProxyCall;
            return System.Activator.CreateInstance(typeArray[typesLength - 2].MakeGenericType(types), node, this, proxyMethod) as Expression;
        }

        private Expression VisitIncrementalStaticProxyCall(MethodCallExpression node, Attribute lensAttribute, MethodInfo proxyMethod, bool isRecursive)
        {
            var typeOffset = node.Method.IsStatic ? 0 : 1;
            if (!proxyMethod.IsStatic)
            {
                throw new InvalidOperationException("The provided proxy method must be static or the target parameter must be omitted.");
            }
            CheckForOutParameter(proxyMethod.GetParameters());
            CheckReturnTypeIsCorrect(node, proxyMethod);
            if (isRecursive)
            {
                var proxyArgs = new Object[1 + node.Arguments.Count + typeOffset];
                var proxyCallTypes = new Type[1 + node.Arguments.Count + typeOffset];
                proxyArgs[0] = proxyMethod;
                proxyCallTypes[0] = node.Type;
                if (!node.Method.IsStatic)
                {
                    proxyArgs[1] = Visit(node.Object);
                    proxyCallTypes[1] = node.Object.Type;
                }
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    proxyArgs[i + typeOffset + 1] = Visit(node.Arguments[i]);
                    proxyCallTypes[i + typeOffset + 1] = node.Arguments[i].Type;
                }
                if (lensAttribute == null)
                {
                    var deferredProxyType = ObservableExpressionTypes.DeferredProxyTypes[proxyCallTypes.Length - 2];
                    deferredProxyType = deferredProxyType.MakeGenericType(proxyCallTypes);
                    return Activator.CreateInstance(deferredProxyType, proxyArgs) as Expression;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var proxyArgs = new Object[node.Arguments.Count + typeOffset];
                if (!node.Method.IsStatic)
                {
                    proxyArgs[0] = Visit(node.Object);
                }
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    proxyArgs[i + typeOffset] = Visit(node.Arguments[i]);
                }
                try
                {
                    if (lensAttribute == null)
                    {
                        object proxy = proxyMethod.Invoke(null, proxyArgs);
                        if(proxy is Expression proxyExp) return proxyExp;
                        return System.Activator.CreateInstance(typeof(ObservableProxyExpression<>).MakeGenericType(node.Method.ReturnType), proxy) as Expression;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                catch (NullReferenceException)
                {
                    throw new InvalidOperationException(string.Format("The proxy method {0} threw a NullReferenceException. Is the underlying function recursive? If so, you have to specify this manually.", proxyMethod.Name));
                }
            }
        }

        private Expression VisitStaticMemberProxyCall(MethodCallExpression node, int typesLength, Attribute lensAttribute, Type[] types, MethodInfo proxyMethod)
        {
            if (proxyMethod.IsStatic)
            {
                throw new InvalidOperationException("The provided proxy method must not be static or the target parameter must be provided.");
            }
            CheckForOutParameter(proxyMethod.GetParameters());
            CheckReturnTypeIsCorrect(node, proxyMethod);
            var typeArray = lensAttribute == null ? ObservableExpressionTypes.ObservableMethodProxyCall : ObservableExpressionTypes.ObservableMethodLensProxyCall;
            return System.Activator.CreateInstance(typeArray[typesLength - 2].MakeGenericType(types), node, this, proxyMethod) as Expression;
        }

        private Expression VisitIncrementalMemberProxyCall(MethodCallExpression node, Attribute lensAttribute, Type[] types, MethodInfo proxyMethod, bool isRecursive)
        {
            if (proxyMethod.IsStatic)
            {
                throw new InvalidOperationException("The provided proxy method must not be static or the target parameter must be provided.");
            }
            CheckForOutParameter(proxyMethod.GetParameters());
            CheckReturnTypeIsCorrect(node, proxyMethod);
            var target = Visit(node.Object) as INotifyExpression;
            if (target.IsConstant && lensAttribute == null && !isRecursive)
            {
                var args = new Object[node.Arguments.Count];
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    args[i] = Visit(node.Arguments[i]);
                }
                return proxyMethod.Invoke(target.ValueObject, args) as Expression;
            }
            else
            {
                var implTypes = lensAttribute == null
                    ? ObservableExpressionTypes.ObservableSimpleMethodProxyCall
                    : ObservableExpressionTypes.ObservableSimpleLensMethodProxyCall;
                return System.Activator.CreateInstance(implTypes[node.Arguments.Count].MakeGenericType(types), node, this, proxyMethod) as Expression;
            }
        }

        private void CheckReturnTypeIsCorrect(MethodCallExpression node, MethodInfo proxyMethod)
        {
            if (!IsNotifyValue(proxyMethod.ReturnType, node.Method.ReturnType))
            {
                throw new InvalidOperationException(string.Format("The proxy method has the wrong return type. Expected return type INotifyValue[{0}] but detected {1}.", node.Method.ReturnType, proxyMethod.ReturnType));
            }
        }

        private static void CheckForOutParameter(System.Reflection.ParameterInfo[] proxyMethodParameters)
        {
            for (int i = 0; i < proxyMethodParameters.Length; i++)
            {
                if (proxyMethodParameters[i].IsOut)
                {
                    throw new NotSupportedException(string.Format("'{0}' is an Out-parameter. Out parameters are not supported.", proxyMethodParameters[i].Name));
                }
            }
        }

        private bool IsNotifyValue(Type actual, Type spec)
        {
            return ReflectionHelper.IsAssignableFrom(typeof(INotifyValue<>).MakeGenericType(spec), actual);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Arguments.Count == 0)
            {
                return Activator.CreateInstance(typeof(ObservableConstant<>).MakeGenericType(node.Type), node.Constructor.Invoke(null)) as Expression;
            }
            var types = new Type[node.Arguments.Count + 1];
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                types[i] = node.Arguments[i].Type;
            }
            types[node.Arguments.Count] = node.Type;
            return Activator.CreateInstance(ObservableExpressionTypes.ObservableNewExpression[node.Arguments.Count - 1].MakeGenericType(types), node, this) as Expression;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            if (node.NodeType == ExpressionType.NewArrayInit)
            {
                return Activator.CreateInstance(typeof(ObservableArrayInitializationExpression<>).MakeGenericType(node.Type.GetElementType()), node, this) as Expression;
            }
            else if (node.NodeType == ExpressionType.NewArrayBounds)
            {
                switch (node.Expressions.Count)
                {
                    case 1:
                        return Activator.CreateInstance(typeof(ObservableNewArray1Expression<>).MakeGenericType(node.Type.GetElementType()),
                            VisitObservable<int>(node.Expressions[0])) as Expression;
                    case 2:
                        return Activator.CreateInstance(typeof(ObservableNewArray2Expression<>).MakeGenericType(node.Type.GetElementType()),
                            VisitObservable<int>(node.Expressions[0]), VisitObservable<int>(node.Expressions[1])) as Expression;
                    case 3:
                        return Activator.CreateInstance(typeof(ObservableNewArray3Expression<>).MakeGenericType(node.Type.GetElementType()),
                            VisitObservable<int>(node.Expressions[0]), VisitObservable<int>(node.Expressions[1]), VisitObservable<int>(node.Expressions[2])) as Expression;
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object value;
            if (parameters.TryGetValue(node.Name, out value))
            {
                if (ReflectionHelper.IsInstanceOf(node.Type, value) || value == null)
                {
                    var createType = typeof(ObservableConstant<>).MakeGenericType(node.Type);
                    return (Expression)(ReflectionHelper.GetConstructor(createType).Invoke(new object[] { value }));
                }
                else
                {
                    if(value is Expression expression)
                    {
                        return expression;
                    }
                    else
                    {
                        throw new InvalidOperationException( string.Format( "The provided value {0} for parameter {1} is not valid.", value, node.Type ) );
                    }
                }
            }
            else
            {
                var createType = typeof(ObservableParameter<>).MakeGenericType(node.Type);
                return (Expression)(ReflectionHelper.GetConstructor(createType).Invoke(new object[] { node.Name }));
            }
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new NotSupportedException("Please report the case under which you hit this error message to t-georgh@microsoft.com");
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new NotSupportedException("Swicth statements are not supported");
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitTry(TryExpression node)
        {
            throw new NotSupportedException("Statements are not supported");
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            var inner = VisitObservable<object>(node.Expression);
            return new ObservableTypeExpression(inner, node.TypeOperand, node.NodeType == ExpressionType.TypeEqual);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Quote || node.NodeType == ExpressionType.UnaryPlus || node.NodeType == ExpressionType.Unbox) return Visit(node.Operand);
            if (node.Method != null)
            {
                return Activator.CreateInstance(typeof(ObservableUnaryExpression<,>).MakeGenericType(node.Operand.Type, node.Type), node, this) as Expression;
            }
            switch (node.NodeType)
            {
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return Activator.CreateInstance(typeof(ObservableConvert<,>).MakeGenericType(node.Operand.Type, node.Type), node, this) as Expression;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return VisitNegate(node);
                case ExpressionType.Not:
                    return new ObservableLogicNot(VisitObservable<bool>(node.Operand));
                case ExpressionType.OnesComplement:
                    return VisitOnesComplement(node);
                case ExpressionType.TypeAs:
                    return Activator.CreateInstance(typeof(ObservableTypeAs<,>).MakeGenericType(node.Operand.Type, node.Type), node, this) as Expression;
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                    return Visit(node.Operand);
                default:
                    break;
            }
            throw new NotSupportedException();
        }

        private Expression VisitOnesComplement(UnaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableIntOnesComplement(VisitObservable<int>(node.Operand));
            }
            if (node.Type == typeof(uint))
            {
                return new ObservableUIntOnesComplement(VisitObservable<uint>(node.Operand));
            }
            if (node.Type == typeof(long))
            {
                return new ObservableLongOnesComplement(VisitObservable<long>(node.Operand));
            }
            if (node.Type == typeof(ulong))
            {
                return new ObservableULongOnesComplement(VisitObservable<ulong>(node.Operand));
            }
            throw new NotSupportedException();
        }

        private Expression VisitNegate(UnaryExpression node)
        {
            if (node.Type == typeof(int))
            {
                return new ObservableUnaryIntMinus(VisitObservable<int>(node.Operand));
            }
            if (node.Type == typeof(long))
            {
                return new ObservableUnaryLongMinus(VisitObservable<long>(node.Operand));
            }
            if (node.Type == typeof(float))
            {
                return new ObservableUnaryFloatMinus(VisitObservable<float>(node.Operand));
            }
            if (node.Type == typeof(double))
            {
                return new ObservableUnaryDoubleMinus(VisitObservable<double>(node.Operand));
            }
            throw new NotSupportedException();
        }

        public INotifyExpression<T> VisitObservable<T>(Expression expression, bool allowNull = false)
        {
            var result = Visit(expression);
            var candidate = result as INotifyExpression<T>;
            if (candidate == null && !allowNull)
            {
                throw new InvalidOperationException(string.Format("The expression {0} cannot be interpreted as {1}.", expression.ToString(), typeof(T).Name));
            }
            if (compress)
            {
                candidate = candidate.Reduce();
            }
            return candidate;
        }

        public INotifyExpression VisitObservable(Expression expression, bool allowNull = false)
        {
            var result = Visit(expression);
            if (compress && result != null)
            {
                result = result.Reduce();
            }
            var candidate = result as INotifyExpression;
            if (candidate == null && !allowNull)
            {
                throw new InvalidOperationException(string.Format("The expression {0} is invalid.", expression.ToString()));
            }
            return candidate;
        }

        private static ObservableMemberBinding<T> CreateProperty<T, TMember>(MemberAssignment node, ObservableExpressionBinder binder, INotifyExpression<T> target)
        {
            INotifyExpression<TMember> value = binder.VisitObservable<TMember>(node.Expression);
            var property = node.Member as PropertyInfo;
            if(value is INotifyReversableExpression<TMember> reversable && ReflectionHelper.IsAssignableFrom( typeof( INotifyPropertyChanged ), typeof( T ) ))
            {
                return new ObservableReversablePropertyMemberBinding<T, TMember>( target, node.Member.Name,
                    ReflectionHelper.CreateDelegate( typeof( Func<T, TMember> ), ReflectionHelper.GetGetter( property ) ) as Func<T, TMember>,
                    ReflectionHelper.CreateDelegate( typeof( Action<T, TMember> ), ReflectionHelper.GetSetter( property ) ) as Action<T, TMember>,
                    reversable );
            }
            return new ObservablePropertyMemberBinding<T, TMember>(target,
                ReflectionHelper.CreateDelegate(typeof(Action<T, TMember>), ReflectionHelper.GetSetter(property)) as Action<T, TMember>, value);
        }

        internal ObservableMemberBinding<T> VisitMemberBinding<T>(MemberBinding memberBinding, INotifyExpression<T> target)
        {
            switch (memberBinding.BindingType)
            {
                case MemberBindingType.Assignment:
                    var assignment = memberBinding as MemberAssignment;
                    var property = assignment.Member as PropertyInfo;
                    if (property != null)
                    {
                        var setter = ReflectionHelper.GetSetter(property);
                        if (setter != null)
                        {
                            return memberBindingCreateProperty.MakeGenericMethod(typeof(T), property.PropertyType).Invoke(null, new object[] { memberBinding, this, target }) as ObservableMemberBinding<T>;
                        }
                    }
                    var field = assignment.Member as FieldInfo;
                    if (field != null)
                    {
                        return Activator.CreateInstance(typeof(ObservablePropertyMemberBinding<,>).MakeGenericType(typeof(T), field.FieldType),
                            memberBinding, this, target, field) as ObservableMemberBinding<T>;
                    }
                    break;
                case MemberBindingType.ListBinding:
                    break;
                case MemberBindingType.MemberBinding:
                    break;
                default:
                    break;
            }
            throw new NotSupportedException();
        }

        internal ObservableMemberBinding<T> VisitElementInit<T>(ElementInit e, INotifyExpression<T> inner)
        {
            if (e.Arguments.Count == 1)
            {
                return Activator.CreateInstance(typeof(ObservableListInitializer<,>).MakeGenericType(typeof(T), e.Arguments[0].Type), e, this, inner)
                    as ObservableMemberBinding<T>;
            }
            throw new NotSupportedException();
        }
    }
}
