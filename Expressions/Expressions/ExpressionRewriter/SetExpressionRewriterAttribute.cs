using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SetExpressionRewriterAttribute : ProxyMethodAttribute
    {
        public SetExpressionRewriterAttribute(Type proxyType, string methodName)
            : base(proxyType, methodName) { }
    }
}
