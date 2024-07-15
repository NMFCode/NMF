using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an attribute to specify an explicit incrementalization of a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ObservableProxyAttribute : ProxyMethodAttribute
    {
        /// <summary>
        /// True, if the incrementalized method is recursive, otherwise False
        /// </summary>
        public bool IsRecursive { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="methodName">The name of the incrementalized method</param>
        /// <param name="isRecursive">True, if the incrementalized method is recursive, otherwise False</param>
        public ObservableProxyAttribute(string methodName, bool isRecursive = false)
            : base(null, methodName)
        {
            IsRecursive = isRecursive;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="proxyType">The type that hosts the incrementalized method</param>
        /// <param name="methodName">The name of the incrementalized method</param>
        /// <param name="isRecursive">True, if the incrementalized method is recursive, otherwise False</param>
        public ObservableProxyAttribute(Type proxyType, string methodName, bool isRecursive = false)
            : base(proxyType, methodName)
        {
            IsRecursive = isRecursive;
        }
    }
}
