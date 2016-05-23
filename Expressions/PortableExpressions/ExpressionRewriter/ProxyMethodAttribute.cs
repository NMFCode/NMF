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
            ProxyType = proxyType.GetTypeInfo();
            MethodName = methodName;
        }

        public bool InitializeProxyMethod(MethodInfo sourceMethod, Type[] types, out MethodInfo proxyMethod)
        {
            var proxyType = this.ProxyType;
            if (types == null) throw new ArgumentOutOfRangeException("types");
            var proxyTypeArgs = new List<Type>();
            if (ReflectionHelper.IsGenericType(sourceMethod.DeclaringType))
            {
                proxyTypeArgs.AddRange(sourceMethod.DeclaringType.GetTypeInfo().GenericTypeArguments);
            }
            if (sourceMethod.IsGenericMethod)
            {
                proxyTypeArgs.AddRange(sourceMethod.GetGenericArguments());
            }
            var typeArrayPointer = 0;
            if (proxyType.IsGenericTypeDefinition)
            {
                var typeArgs = new Type[proxyType.GenericTypeParameters.Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = proxyTypeArgs[typeArrayPointer];
                    typeArrayPointer++;
                }
                proxyType = proxyType.MakeGenericType(typeArgs).GetTypeInfo();
            }
            if (proxyTypeArgs.Count == typeArrayPointer)
            {
                proxyMethod = proxyType.GetDeclaredMethods(MethodName).FirstOrDefault(m => CheckTypes(m.GetParameters(), types));
            }
            else
            {
                var typeArgs = new Type[proxyTypeArgs.Count - typeArrayPointer];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = proxyTypeArgs[typeArrayPointer];
                    typeArrayPointer++;
                }
                proxyMethod = proxyType.GetDeclaredMethods(MethodName).FirstOrDefault(m =>
                {
                    if (!m.IsGenericMethodDefinition || m.GetGenericArguments().Length != typeArgs.Length) return false;

                    var genericMethod = m.MakeGenericMethod(typeArgs);

                    return CheckTypes(genericMethod.GetParameters(), types);
                });
                if (proxyMethod != null)
                {
                    proxyMethod = proxyMethod.MakeGenericMethod(typeArgs);
                    return true;
                }
            }
            if (proxyMethod == null) return false;
            return true;
        }

        private bool CheckTypes(ParameterInfo[] parameterInfo, Type[] types)
        {
            if (parameterInfo.Length != types.Length) return false;
            for (int i = 0; i < parameterInfo.Length; i++)
            {
                if (parameterInfo[i].ParameterType != types[i]) return false;
            }
            return true;
        }

        public TypeInfo ProxyType { get; private set; }

        public string MethodName { get; private set; }
    }
}
