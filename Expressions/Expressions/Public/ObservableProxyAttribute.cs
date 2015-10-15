using System;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ObservableProxyAttribute : ProxyMethodAttribute
    {
        public ObservableProxyAttribute(Type proxyType, string methodName)
            : base(proxyType, methodName) { }
    }
}
