using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an attribute to specify a proxy method for a lens put operation
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LensPutAttribute : ProxyMethodAttribute
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="methodName">The name of the method representing the lens put operation</param>
        public LensPutAttribute(string methodName) : base(null, methodName)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="proxyType">The type that hosts the lens put operation</param>
        /// <param name="methodName">The name of the method representing the lens put operation</param>
        public LensPutAttribute(Type proxyType, string methodName) : base(proxyType, methodName)
        {
        }
    }
}
