using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    internal abstract class ObservableStaticMethodBase<TDelegate, TResult> : NotifyExpression<TResult>
        where TDelegate : Delegate
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

        public TDelegate Function { get; private set; }

        public override ExpressionType NodeType { get { return ExpressionType.Invoke; } }

        public override string ToString()
        {
            return $"[Invoke {Function.Method.Name}]";
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var oldValue = Value;
            var result = base.Notify(sources);

            if (result.Changed)
            {
                if(oldValue is IDisposable disposable)
                    disposable.Dispose();
            }

            return result;
        }
    }
}
