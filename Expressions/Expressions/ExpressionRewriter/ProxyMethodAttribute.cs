using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    public abstract class ProxyMethodAttribute : Attribute
    {
        public ProxyMethodAttribute(Type proxyType, string methodName)
        {
            ProxyType = proxyType;
            MethodName = methodName;
        }

        public bool InitializeProxyMethod(MethodInfo sourceMethod, Type[] types, out MethodInfo proxyMethod)
        {
            if (types == null) throw new ArgumentNullException("types");
            var proxyTypeArgs = new List<Type>();
            if (ReflectionHelper.IsGenericType(sourceMethod.DeclaringType))
            {
                proxyTypeArgs.AddRange(sourceMethod.DeclaringType.GetGenericArguments());
            }
            if (sourceMethod.IsGenericMethod)
            {
                proxyTypeArgs.AddRange(sourceMethod.GetGenericArguments());
            }
            var typeArrayPointer = 0;
            var proxyType = this.ProxyType;
            if (proxyType.IsGenericTypeDefinition)
            {
                var typeArgs = new Type[proxyType.GetGenericArguments().Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = proxyTypeArgs[typeArrayPointer];
                    typeArrayPointer++;
                }
                proxyType = proxyType.MakeGenericType(typeArgs);
            }
            if (proxyTypeArgs.Count > typeArrayPointer)
            {
                var methodParamArgs = new Type[proxyTypeArgs.Count - typeArrayPointer];
                for (int i = 0; i < methodParamArgs.Length; i++)
                {
                    methodParamArgs[i] = proxyTypeArgs[i + typeArrayPointer];
                }
                proxyMethod = proxyType.GetMethods().Select(m =>
                {
                    if (m.IsGenericMethodDefinition && m.Name == MethodName)
                    {
                        var methodTypeParameters = m.GetGenericArguments();
                        if (methodTypeParameters.Length != methodParamArgs.Length) return null;
                        var genericMethod = m.MakeGenericMethod(methodParamArgs);
                        var parameters = genericMethod.GetParameters();
                        if (parameters.Length != types.Length) return null;
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != types[i]) return null;
                        }
                        return genericMethod;
                    }
                    return null;
                }).FirstOrDefault(m => m != null);
            }
            else
            {
                proxyMethod = proxyType.GetMethod(MethodName, types);
            }
            return proxyMethod != null;
        }

        public Type ProxyType { get; private set; }

        public string MethodName { get; private set; }
    }
}
