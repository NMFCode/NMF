using System;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ObservableProxyAttribute : ProxyMethodAttribute
    {
        public bool IsRecursive { get; private set; }

        public ObservableProxyAttribute(Type proxyType, string methodName, bool isRecursive = false)
            : base(proxyType, methodName)
        {
            IsRecursive = isRecursive;
        }
    }
}
