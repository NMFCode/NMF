using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an attribute that denotes the definition of a proxy method
    /// </summary>
    public abstract class ProxyMethodAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="proxyType">The type of the proxy</param>
        /// <param name="methodName">The name of the method</param>
        protected ProxyMethodAttribute(Type proxyType, string methodName)
        {
            ProxyType = proxyType;
            MethodName = methodName;
        }

        /// <summary>
        /// Finds the proxy method for the given source method
        /// </summary>
        /// <param name="sourceMethod">The method for which a proxy is needed</param>
        /// <param name="parameterTypes">The desired parameter types</param>
        /// <param name="proxyMethod">The proxy method</param>
        /// <returns>True, if a suitable method was found, otherwise False</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameterTypes is null</exception>
        public bool InitializeProxyMethod(MethodInfo sourceMethod, Type[] parameterTypes, out MethodInfo proxyMethod)
        {
            if (parameterTypes == null) throw new ArgumentNullException(nameof(parameterTypes));
            var proxyTypeArgs = ProxyMethodAttribute.CreateProxyTypeArgs(sourceMethod);
            var typeArrayPointer = 0;
            var proxyType = ProxyType ?? sourceMethod.DeclaringType;
            CloseProxyType(proxyTypeArgs, ref typeArrayPointer, ref proxyType);
            if (proxyTypeArgs.Count > typeArrayPointer)
            {
                var methodParamArgs = new Type[proxyTypeArgs.Count - typeArrayPointer];
                for (int i = 0; i < methodParamArgs.Length; i++)
                {
                    methodParamArgs[i] = proxyTypeArgs[i + typeArrayPointer];
                }
                proxyMethod = MakeGenericProxyMethod(parameterTypes, proxyType, methodParamArgs);
            }
            else
            {
                proxyMethod = proxyType.GetMethod(MethodName, parameterTypes);
            }
            return proxyMethod != null;
        }

        private MethodInfo MakeGenericProxyMethod(Type[] types, Type proxyType, Type[] methodParamArgs)
        {
            return proxyType.GetMethods().Select(m =>
            {
                if (m.IsGenericMethodDefinition && m.Name == MethodName)
                {
                    var methodTypeParameters = m.GetGenericArguments();
                    if (methodTypeParameters.Length != methodParamArgs.Length) return null;
                    var genericMethod = m.MakeGenericMethod(methodParamArgs);
                    if (MatchesRequiredSignature(genericMethod, types))
                    {
                        return genericMethod;
                    }
                }
                return null;
            }).FirstOrDefault(m => m != null);
        }

        private static bool MatchesRequiredSignature(MethodInfo method, Type[] types)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != types.Length) return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != types[i]) return false;
            }
            return true;
        }

        private static void CloseProxyType(List<Type> proxyTypeArgs, ref int typeArrayPointer, ref Type proxyType)
        {
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
        }

        private static List<Type> CreateProxyTypeArgs(MethodInfo sourceMethod)
        {
            var proxyTypeArgs = new List<Type>();
            if (ReflectionHelper.IsGenericType(sourceMethod.DeclaringType) && sourceMethod.DeclaringType != null)
            {
                proxyTypeArgs.AddRange(sourceMethod.DeclaringType.GetGenericArguments());
            }
            if (sourceMethod.IsGenericMethod)
            {
                proxyTypeArgs.AddRange(sourceMethod.GetGenericArguments());
            }

            return proxyTypeArgs;
        }

        /// <summary>
        /// The type in which the proxy method is defined
        /// </summary>
        public Type ProxyType { get; private set; }

        /// <summary>
        /// The name of the method used as a proxy
        /// </summary>
        public string MethodName { get; private set; }
    }
}
