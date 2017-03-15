using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace NMF.Expressions
{
    internal static class ReflectionHelper
    {
        public static bool IsValueType(Type t)
        {
            return t.IsValueType;
        }

        public static bool IsValueType<T>()
        {
            return typeof(T).IsValueType;
        }

        public static ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructors()[0];
        }

        internal static Action<T, TField> CreateDynamicFieldSetter<T, TField>(FieldInfo field)
        {
            var setMethod = new DynamicMethod("_set_" + field.Name, typeof(void), new Type[] { field.DeclaringType, field.FieldType }, field.DeclaringType);
            var setGenerator = setMethod.GetILGenerator();
            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, field);
            setGenerator.Emit(OpCodes.Ret);
            return setMethod.CreateDelegate(typeof(Action<T, TField>)) as Action<T, TField>;
        }

        internal static Func<T, TField> CreateDynamicFieldGetter<T, TField>(FieldInfo field)
        {
            var getMethod = new DynamicMethod("_get_" + field.Name, field.FieldType, new Type[] { field.DeclaringType }, field.DeclaringType);
            var getGenerator = getMethod.GetILGenerator();
            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, field);
            getGenerator.Emit(OpCodes.Ret);
            return getMethod.CreateDelegate(typeof(Func<T, TField>)) as Func<T, TField>;
        }

        internal static object CreateDelegate(Type delegateType, MethodInfo method)
        {
            return Delegate.CreateDelegate(delegateType, method);
        }

        internal static object CreateDelegate(Type delegateType, object target, MethodInfo method)
        {
            try
            {
                return Delegate.CreateDelegate(delegateType, target, method);
            }
            catch (ArgumentException)
            {
                return Delegate.CreateDelegate(delegateType, target, method.Name);
            }
        }

        internal static TDelegate CreateDelegate<TDelegate>(MethodInfo method) where TDelegate : class
        {
            return Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
        }

        internal static bool IsGenericType(Type t)
        {
            return t.IsGenericType;
        }

        internal static TAttribute[] GetCustomAttributes<TAttribute>(MethodInfo method, bool inherit) where TAttribute : Attribute
        {
            return (TAttribute[])method.GetCustomAttributes(typeof(TAttribute), inherit);
        }

        internal static bool IsAssignableFrom(Type baseType, Type assignable)
        {
            return baseType.IsAssignableFrom(assignable);
        }

        internal static bool IsInstanceOf(Type type, object instance)
        {
            return type.IsInstanceOfType(instance);
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
            return property.GetGetMethod();
        }

        internal static MethodInfo GetSetter(PropertyInfo property)
        {
            return property.GetSetMethod();
        }

        internal static MethodInfo GetMethod(Type type, string name, Type[] types)
        {
            return type.GetMethod(name, types);
        }

        internal static PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        internal static MethodInfo GetRemoveMethod(Type collectionType, Type elementType)
        {
            if (ReflectionHelper.IsAssignableFrom(typeof(IList), collectionType))
            {
                var map = collectionType.GetInterfaceMap(typeof(IList));
                return map.TargetMethods[9];
            }
            var methods = collectionType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in methods)
            {
                if (method.Name == "Remove")
                {
                    var parameters = method.GetParameters();
                    if (parameters != null && parameters.Length == 1)
                    {
                        if (!parameters[0].IsOut && parameters[0].ParameterType.IsAssignableFrom(elementType))
                        {
                            return method;
                        }
                    }
                }
            }
            throw new InvalidOperationException("No suitable Remove method found");
        }

        internal static bool Implements<T>(this Type type)
        {
            return type.GetInterface(typeof(T).FullName) != null;
        }
    }
}
