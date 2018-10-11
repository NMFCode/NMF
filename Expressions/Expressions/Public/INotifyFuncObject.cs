using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NMF.Expressions.Public
{
    public interface INotifyFuncObject
    {
        MethodInfo GetProxyFunction(MethodInfo method);
    }
}
