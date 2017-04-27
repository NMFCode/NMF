using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    class ObservableLensStaticMethodCallPrototype<T1, TResult> : ObservableStaticMethodCall<T1, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableLensStaticMethodCallPrototype(MethodCallExpression node, ObservableExpressionBinder binder, LensPutAttribute lensPut) : base(node, binder)
        {
            LensPut = (Action<T1, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(TResult) }, typeof(Action<T1, TResult>), Argument1, lensPut);
        }

        public ObservableLensStaticMethodCallPrototype(Func<T1, TResult> function, Action<T1, TResult> lensPut, INotifyExpression<T1> argument1) : base(function, argument1)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableLensStaticMethodCallPrototype<T1, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters));
        }
    }
}
