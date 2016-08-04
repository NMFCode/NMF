using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;

namespace NMF.Expressions
{
    internal static class ReflectionHelper
    {
        public static bool IsValueType(Type t)
        {
            return t.GetTypeInfo().IsValueType;
        }

        public static ConstructorInfo GetConstructor(Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.Single();
        }

        public static bool IsValueType<T>()
        {
            return typeof(T).GetTypeInfo().IsValueType;
        }

        internal static Action<T, TField> CreateDynamicFieldSetter<T, TField>(FieldInfo field)
        {
            var target = Expression.Parameter(field.DeclaringType, "target");
            var value = Expression.Parameter(field.FieldType, "value");
            var lambda = Expression.Lambda<Action<T, TField>>(Expression.Assign(Expression.Field(target, field), value), target, value);
            return lambda.Compile();
        }

        internal static Func<T, TField> CreateDynamicFieldGetter<T, TField>(FieldInfo field)
        {
            var target = Expression.Parameter(field.DeclaringType, "target");
            var lambda = Expression.Lambda<Func<T, TField>>(Expression.Field(target, field), target);
            return lambda.Compile();
        }

        internal static object CreateDelegate(Type delegateType, MethodInfo method)
        {
            return method.CreateDelegate(delegateType);
        }

        internal static object CreateDelegate(Type delegateType, object target, MethodInfo method)
        {
            try
            {
                return method.CreateDelegate(delegateType, target);
            }
            catch (ArgumentException)
            {
                throw new NotSupportedException("You have just run into a known bug of NMF Expressions that is caused by a subtle bug of the .NET platform.");
            }
        }

        internal static TDelegate CreateDelegate<TDelegate>(MethodInfo method) where TDelegate : class
        {
            return method.CreateDelegate(typeof(TDelegate)) as TDelegate;
        }

        internal static bool IsGenericType(Type t)
        {
            return t.GetTypeInfo().IsGenericType;
        }

        internal static TAttribute[] GetCustomAttributes<TAttribute>(MethodInfo method, bool inherit) where TAttribute : Attribute
        {
            return (TAttribute[])method.GetCustomAttributes(typeof(TAttribute), inherit);
        }

        internal static bool IsAssignableFrom(Type baseType, Type assignable)
        {
            return baseType.GetTypeInfo().IsAssignableFrom(assignable.GetTypeInfo());
        }

        internal static bool IsInstanceOf(Type type, object instance)
        {
            return instance != null && IsAssignableFrom(type, instance.GetType());
        }

        internal static MethodInfo GetAction(Expression<Action> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetAction<T1>(Expression<Action<T1>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetAction<T1, T2>(Expression<Action<T1, T2>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetAction<T1, T2, T3>(Expression<Action<T1, T2, T3>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetAction<T1, T2, T3, T4>(Expression<Action<T1, T2, T3, T4>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetFunc<TReturn>(Expression<Func<TReturn>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetFunc<T1, TReturn>(Expression<Func<T1, TReturn>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetFunc<T1, T2, TReturn>(Expression<Func<T1, T2, TReturn>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetFunc<T1, T2, T3, TReturn>(Expression<Func<T1, T2, T3, TReturn>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        internal static MethodInfo GetFunc<T1, T2, T3, T4, TReturn>(Expression<Func<T1, T2, T3, T4, TReturn>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        private static MethodInfo GetMethodInfo(LambdaExpression lambda)
        {
            var methodCall = lambda.Body as MethodCallExpression;
            return methodCall.Method;
        }

        internal static MethodInfo GetGetter(PropertyInfo property)
        {
            return property.GetMethod;
        }

        internal static MethodInfo GetSetter(PropertyInfo property)
        {
            return property.SetMethod;
        }

        internal static MethodInfo GetMethod(Type type, string name, Type[] types)
        {
            return type.GetRuntimeMethod(name, types);
        }

        internal static PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetRuntimeProperty(name);
        }

        internal static MethodInfo GetRemoveMethod(Type collectionType, Type elementType)
        {
            var collectionTypeInfo = collectionType.GetTypeInfo();
            var elementTypeInfo = elementType.GetTypeInfo();
            if (ReflectionHelper.IsAssignableFrom(typeof(IList), collectionType))
            {
                var map = collectionTypeInfo.GetRuntimeInterfaceMap(typeof(IList));
                return map.TargetMethods[9];
            }
            var methods = collectionType.GetRuntimeMethods();
            foreach (var method in methods)
            {
                if (method.Name == "Remove")
                {
                    var parameters = method.GetParameters();
                    if (parameters != null && parameters.Length == 1)
                    {
                        if (!parameters[0].IsOut && parameters[0].ParameterType.GetTypeInfo().IsAssignableFrom(elementTypeInfo))
                        {
                            return method;
                        }
                    }
                }
            }
            throw new InvalidOperationException("No suitable Remove method found");
        }
    }
}
