using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal sealed class ObservableStaticProxyCall<T1, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0))) { }

        public ObservableStaticProxyCall(Func<T1, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }

        public Func<T1, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1))) { }

        public ObservableStaticProxyCall(Func<T1, T2, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }

        public Func<T1, T2, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }

        public Func<T1, T2, T3, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }

        public Func<T1, T2, T3, T4, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }

        public Func<T1, T2, T3, T4, T5, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 10))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");
            if (arg11 == null) throw new ArgumentNullException("arg11");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
            Argument11 = arg11;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }
        public INotifyExpression<T11> Argument11 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;                yield return Argument11;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 11))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");
            if (arg11 == null) throw new ArgumentNullException("arg11");
            if (arg12 == null) throw new ArgumentNullException("arg12");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
            Argument11 = arg11;
            Argument12 = arg12;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }
        public INotifyExpression<T11> Argument11 { get; private set; }
        public INotifyExpression<T12> Argument12 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;                yield return Argument11;                yield return Argument12;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 12))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");
            if (arg11 == null) throw new ArgumentNullException("arg11");
            if (arg12 == null) throw new ArgumentNullException("arg12");
            if (arg13 == null) throw new ArgumentNullException("arg13");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
            Argument11 = arg11;
            Argument12 = arg12;
            Argument13 = arg13;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }
        public INotifyExpression<T11> Argument11 { get; private set; }
        public INotifyExpression<T12> Argument12 { get; private set; }
        public INotifyExpression<T13> Argument13 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;                yield return Argument11;                yield return Argument12;                yield return Argument13;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 12)), binder.VisitObservable<T14>(ExpressionHelper.GetArg(node, 13))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");
            if (arg11 == null) throw new ArgumentNullException("arg11");
            if (arg12 == null) throw new ArgumentNullException("arg12");
            if (arg13 == null) throw new ArgumentNullException("arg13");
            if (arg14 == null) throw new ArgumentNullException("arg14");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
            Argument11 = arg11;
            Argument12 = arg12;
            Argument13 = arg13;
            Argument14 = arg14;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }
        public INotifyExpression<T11> Argument11 { get; private set; }
        public INotifyExpression<T12> Argument12 { get; private set; }
        public INotifyExpression<T13> Argument13 { get; private set; }
        public INotifyExpression<T14> Argument14 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;                yield return Argument11;                yield return Argument12;                yield return Argument13;                yield return Argument14;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableStaticProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(ReflectionHelper.CreateDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, INotifyValue<TResult>>>(proxyMethod), binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 0)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 12)), binder.VisitObservable<T14>(ExpressionHelper.GetArg(node, 13)), binder.VisitObservable<T15>(ExpressionHelper.GetArg(node, 14))) { }

        public ObservableStaticProxyCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, INotifyValue<TResult>> proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14, INotifyExpression<T15> arg15)
        {
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");
            if (arg10 == null) throw new ArgumentNullException("arg10");
            if (arg11 == null) throw new ArgumentNullException("arg11");
            if (arg12 == null) throw new ArgumentNullException("arg12");
            if (arg13 == null) throw new ArgumentNullException("arg13");
            if (arg14 == null) throw new ArgumentNullException("arg14");
            if (arg15 == null) throw new ArgumentNullException("arg15");

            ProxyFunction = proxyFunction;
            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
            Argument9 = arg9;
            Argument10 = arg10;
            Argument11 = arg11;
            Argument12 = arg12;
            Argument13 = arg13;
            Argument14 = arg14;
            Argument15 = arg15;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }
        public INotifyExpression<T5> Argument5 { get; private set; }
        public INotifyExpression<T6> Argument6 { get; private set; }
        public INotifyExpression<T7> Argument7 { get; private set; }
        public INotifyExpression<T8> Argument8 { get; private set; }
        public INotifyExpression<T9> Argument9 { get; private set; }
        public INotifyExpression<T10> Argument10 { get; private set; }
        public INotifyExpression<T11> Argument11 { get; private set; }
        public INotifyExpression<T12> Argument12 { get; private set; }
        public INotifyExpression<T13> Argument13 { get; private set; }
        public INotifyExpression<T14> Argument14 { get; private set; }
        public INotifyExpression<T15> Argument15 { get; private set; }

        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, INotifyValue<TResult>> ProxyFunction { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;

                yield return Argument1;                yield return Argument2;                yield return Argument3;                yield return Argument4;                yield return Argument5;                yield return Argument6;                yield return Argument7;                yield return Argument8;                yield return Argument9;                yield return Argument10;                yield return Argument11;                yield return Argument12;                yield return Argument13;                yield return Argument14;                yield return Argument15;            }
        }

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, Argument15.Value);
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree && Argument15.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticProxyCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(ProxyFunction, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters), Argument15.ApplyParameters(parameters));
        }
    }
}