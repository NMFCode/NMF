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
            proxyMethod = ProxyType.GetDeclaredMethods(MethodName).FirstOrDefault(m => CheckTypes(m.GetParameters(), types));
            if (proxyMethod == null) return false;
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
