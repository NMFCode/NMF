using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpressionCompileRewriterAttribute : ProxyMethodAttribute
    {
        public ExpressionCompileRewriterAttribute(Type hostType, string methodName)
            : base(hostType, methodName) { }
    }
}
