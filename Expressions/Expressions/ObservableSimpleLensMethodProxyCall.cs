using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal class ObservableSimpleLensMethodProxyCall<T, TResult> : ObservableSimpleMethodProxyCall<T, TResult>
    {
        public LensPut<T, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, LensPut<T, TResult> lensPut) : base(target, proxyFunction)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, TResult>(newTarget, ProxyMethod, LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, TResult> : ObservableSimpleMethodProxyCall<T, T1, TResult>
    {
        public LensPut<T, T1, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, LensPut<T, T1, TResult> lensPut) : base(target, proxyFunction, arg1)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, TResult>
    {
        public LensPut<T, T1, T2, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, LensPut<T, T1, T2, TResult> lensPut) : base(target, proxyFunction, arg1, arg2)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, TResult>
    {
        public LensPut<T, T1, T2, T3, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, LensPut<T, T1, T2, T3, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, LensPut<T, T1, T2, T3, T4, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, LensPut<T, T1, T2, T3, T4, T5, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, LensPut<T, T1, T2, T3, T4, T5, T6, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, LensPut<T, T1, T2, T3, T4, T5, T6, T7, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
    internal class ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        public LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> LensPut { get; private set; }

        public ObservableSimpleLensMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod) : base(node, binder, proxyMethod)
        {
            var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Target as INotifyReversableValue<T>);
        }

        public ObservableSimpleLensMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14, LensPut<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> lensPut) : base(target, proxyFunction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14)
        {
            LensPut = lensPut;
        }

        public override bool IsReversable
        {
            get
            {
                return LensPut != null && LensPut.CanApply;
            }
        }

        public override void SetValue(TResult value)
        {
            LensPut.SetValue(Target.Value, Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var newTarget = Target.ApplyParameters(parameters, trace);
            return new ObservableSimpleLensMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(newTarget, ProxyMethod, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), Argument14.ApplyParameters(parameters, trace), LensPut.ApplyNewTarget((INotifyReversableValue<T>)newTarget));
        }
    }
}
