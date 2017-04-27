using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LensPutAttribute : ProxyMethodAttribute
    {
        public LensPutAttribute(Type proxyType, string methodName) : base(proxyType, methodName)
        {
        }
    }
}
