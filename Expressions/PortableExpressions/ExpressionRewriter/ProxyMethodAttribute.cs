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

        public bool IsInitialized
        {
            get
            {
                return ProxyMethod != null;
            }
        }

        public MethodInfo InitializeProxyMethod(MethodInfo sourceMethod)
        {
            if (ProxyMethod != null) return ProxyMethod;
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
            if (ProxyType.IsGenericTypeDefinition)
            {
                var typeArgs = new Type[ProxyType.GenericTypeParameters.Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = proxyTypeArgs[typeArrayPointer];
                    typeArrayPointer++;
                }
                ProxyType = ProxyType.MakeGenericType(typeArgs).GetTypeInfo();
            }
            var proxyMethod = ProxyType.GetDeclaredMethod(MethodName);
            if (proxyMethod.IsGenericMethodDefinition)
            {
                var typeArgs = new Type[proxyMethod.GetGenericArguments().Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    typeArgs[i] = proxyTypeArgs[typeArrayPointer];
                    typeArrayPointer++;
                }
                proxyMethod = proxyMethod.MakeGenericMethod(typeArgs);
            }
            ProxyMethod = proxyMethod;
            return proxyMethod;
        }

        public TypeInfo ProxyType { get; private set; }

        public string MethodName { get; private set; }

        public MethodInfo ProxyMethod { get; private set; }
    }
}
