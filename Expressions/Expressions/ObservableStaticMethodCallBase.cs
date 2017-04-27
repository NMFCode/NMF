using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableStaticMethodBase<TDelegate, TResult> : NotifyExpression<TResult>
        where TDelegate : class
    {
        public ObservableStaticMethodBase(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");

            Function = ReflectionHelper.CreateDelegate(typeof(TDelegate), method) as TDelegate;
        }

        public ObservableStaticMethodBase(TDelegate function)
        {
            Function = function;
        }

        protected object ExtractLensPut(MethodCallExpression node, Type[] types, Type delegateType, object targetReversable, LensPutAttribute lensAttribute)
        {
            MethodInfo lensMethod;
            if (lensAttribute.InitializeProxyMethod(node.Method, types, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return ReflectionHelper.CreateDelegate(delegateType, lensMethod);
                }
                else
                {
                    var reversableType = typeof(INotifyReversableValue<>).MakeGenericType(types[0]);
                    if (ReflectionHelper.IsInstanceOf(reversableType, targetReversable))
                    {
                        var parameters = types.Select((t, i) => Parameter(t, "arg" + i.ToString())).ToArray();
                        var target = Constant(targetReversable, reversableType);
                        var targetValue = MakeMemberAccess(target, ReflectionHelper.GetProperty(reversableType, "Value"));
                        var lensCall = Call(lensMethod, parameters);
                        var expression = Lambda(delegateType, Assign(targetValue, lensCall), parameters);
                        return expression.Compile();
                    }
                    return null;
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {node.Method.Name} has the wrong signature.");
            }
        }

        public TDelegate Function { get; private set; }

        public override ExpressionType NodeType { get { return ExpressionType.Invoke; } }
        
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var oldValue = Value;
            var result = base.Notify(sources);

            if (result.Changed)
            {
                var disposable = oldValue as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            return result;
        }
    }
}
