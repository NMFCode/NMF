using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an attribute to specify a proxy method to rewrite a method to a lens
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SetExpressionRewriterAttribute : ProxyMethodAttribute
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="proxyType">The type that hosts the rewriter method</param>
        /// <param name="methodName">The name of the rewriter method</param>
        public SetExpressionRewriterAttribute(Type proxyType, string methodName)
            : base(proxyType, methodName) { }
    }
}
