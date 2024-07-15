using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an attribute to specify a compile rewriter method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpressionCompileRewriterAttribute : ProxyMethodAttribute
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="methodName">The name of the proxy method</param>
        public ExpressionCompileRewriterAttribute(string methodName)
            : base(null, methodName) { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="hostType">The hosting type of the proxy method</param>
        /// <param name="methodName">The name of the proxy method</param>
        public ExpressionCompileRewriterAttribute(Type hostType, string methodName)
            : base(hostType, methodName) { }
    }
}
