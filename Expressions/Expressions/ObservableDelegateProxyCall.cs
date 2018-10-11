using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableDelegateProxy<T1, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, TReturn>> target, INotifyExpression<T1> argument1)
        {
            Target = target;
            Arg1 = argument1;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute != null)
            {
                if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>) }, out var incProxy))
                {
                    Func<INotifyValue<T1>, INotifyValue<TReturn>> incProxyFunc;
                    if (incProxy.IsStatic)
                    {
                        incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<TReturn>>)
                            incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<TReturn>>));
                    }
                    else
                    {
                        incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<TReturn>>)
                            incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<TReturn>>), Target.Value.Target);
                    }
                    var inner = incProxyFunc(Arg1);
                    inner.Successors.Set(this);
                    argChangesTriggerReeval = false;
                    return inner;
                }
                else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1) }, out var shallowProxy))
                {
                    Func<T1, INotifyValue<TReturn>> shallowProxyFunc;
                    if (shallowProxy.IsStatic)
                    {
                        shallowProxyFunc = (Func<T1, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, INotifyValue<TReturn>>));
                    }
                    else
                    {
                        shallowProxyFunc = (Func<T1, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, INotifyValue<TReturn>>), Target.Value.Target);
                    }
                    var inner = shallowProxyFunc(Arg1.Value);
                    inner.Successors.Set(this);
                    argChangesTriggerReeval = true;
                    return inner;
                }
            }
            var cons = Observable.Constant(Target.Value(Arg1.Value));
            argChangesTriggerReeval = true;
            cons.Successors.Set(this);
            return cons;
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, TReturn>(targetCopy, arg1Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2)}, out var shallowProxy))
            {
                Func<T1, T2, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, TReturn>(targetCopy, arg1Copy, arg2Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3)}, out var shallowProxy))
            {
                Func<T1, T2, T3, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }
        public INotifyExpression<T6> Arg6 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, T6, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, T6, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
            Arg6 = argument6;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree && Arg6.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                yield return Arg6;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>), typeof(INotifyValue<T6>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, T6, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);
            var arg6Copy = Arg6.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy && Arg6 == arg6Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy, arg6Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }
        public INotifyExpression<T6> Arg6 { get; private set; }
        public INotifyExpression<T7> Arg7 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, T6, T7, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
            Arg6 = argument6;
            Arg7 = argument7;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree && Arg6.IsParameterFree && Arg7.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                yield return Arg6;
                yield return Arg7;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>), typeof(INotifyValue<T6>), typeof(INotifyValue<T7>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);
            var arg6Copy = Arg6.ApplyParameters(parameters, trace);
            var arg7Copy = Arg7.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy && Arg6 == arg6Copy && Arg7 == arg7Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy, arg6Copy, arg7Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }
        public INotifyExpression<T6> Arg6 { get; private set; }
        public INotifyExpression<T7> Arg7 { get; private set; }
        public INotifyExpression<T8> Arg8 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
            Arg6 = argument6;
            Arg7 = argument7;
            Arg8 = argument8;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree && Arg6.IsParameterFree && Arg7.IsParameterFree && Arg8.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                yield return Arg6;
                yield return Arg7;
                yield return Arg8;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>), typeof(INotifyValue<T6>), typeof(INotifyValue<T7>), typeof(INotifyValue<T8>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);
            var arg6Copy = Arg6.ApplyParameters(parameters, trace);
            var arg7Copy = Arg7.ApplyParameters(parameters, trace);
            var arg8Copy = Arg8.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy && Arg6 == arg6Copy && Arg7 == arg7Copy && Arg8 == arg8Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy, arg6Copy, arg7Copy, arg8Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }
        public INotifyExpression<T6> Arg6 { get; private set; }
        public INotifyExpression<T7> Arg7 { get; private set; }
        public INotifyExpression<T8> Arg8 { get; private set; }
        public INotifyExpression<T9> Arg9 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
            Arg6 = argument6;
            Arg7 = argument7;
            Arg8 = argument8;
            Arg9 = argument9;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree && Arg6.IsParameterFree && Arg7.IsParameterFree && Arg8.IsParameterFree && Arg9.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                yield return Arg6;
                yield return Arg7;
                yield return Arg8;
                yield return Arg9;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>), typeof(INotifyValue<T6>), typeof(INotifyValue<T7>), typeof(INotifyValue<T8>), typeof(INotifyValue<T9>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value, Arg9.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value, Arg9.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);
            var arg6Copy = Arg6.ApplyParameters(parameters, trace);
            var arg7Copy = Arg7.ApplyParameters(parameters, trace);
            var arg8Copy = Arg8.ApplyParameters(parameters, trace);
            var arg9Copy = Arg9.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy && Arg6 == arg6Copy && Arg7 == arg7Copy && Arg8 == arg8Copy && Arg9 == arg9Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy, arg6Copy, arg7Copy, arg8Copy, arg9Copy);
            return copy;
        }
    }
    internal class ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn> : ObservableDelegateProxyBase<TReturn>
    {
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>> Target { get; private set; }
        public INotifyExpression<T1> Arg1 { get; private set; }
        public INotifyExpression<T2> Arg2 { get; private set; }
        public INotifyExpression<T3> Arg3 { get; private set; }
        public INotifyExpression<T4> Arg4 { get; private set; }
        public INotifyExpression<T5> Arg5 { get; private set; }
        public INotifyExpression<T6> Arg6 { get; private set; }
        public INotifyExpression<T7> Arg7 { get; private set; }
        public INotifyExpression<T8> Arg8 { get; private set; }
        public INotifyExpression<T9> Arg9 { get; private set; }
        public INotifyExpression<T10> Arg10 { get; private set; }

		public ObservableDelegateProxy(MethodCallExpression node, ObservableExpressionBinder binder)
		    : this(binder.VisitObservable<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>>(node.Object), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9])) {}

        public ObservableDelegateProxy(INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>> target, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10)
        {
            Target = target;
            Arg1 = argument1;
            Arg2 = argument2;
            Arg3 = argument3;
            Arg4 = argument4;
            Arg5 = argument5;
            Arg6 = argument6;
            Arg7 = argument7;
            Arg8 = argument8;
            Arg9 = argument9;
            Arg10 = argument10;
        }

        public override bool IsParameterFree => Target.IsParameterFree && Arg1.IsParameterFree && Arg2.IsParameterFree && Arg3.IsParameterFree && Arg4.IsParameterFree && Arg5.IsParameterFree && Arg6.IsParameterFree && Arg7.IsParameterFree && Arg8.IsParameterFree && Arg9.IsParameterFree && Arg10.IsParameterFree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Arg1;
                yield return Arg2;
                yield return Arg3;
                yield return Arg4;
                yield return Arg5;
                yield return Arg6;
                yield return Arg7;
                yield return Arg8;
                yield return Arg9;
                yield return Arg10;
                if (Inner != null) yield return Inner;
            }
        }

		protected override INotifyExpression<Delegate> GetTarget() => Target;

        protected override INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval)
        {
            var proxyAttribute = method.GetCustomAttribute<ObservableProxyAttribute>(inherit: false);
            if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(INotifyValue<T1>), typeof(INotifyValue<T2>), typeof(INotifyValue<T3>), typeof(INotifyValue<T4>), typeof(INotifyValue<T5>), typeof(INotifyValue<T6>), typeof(INotifyValue<T7>), typeof(INotifyValue<T8>), typeof(INotifyValue<T9>), typeof(INotifyValue<T10>) }, out var incProxy))
            {
                Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TReturn>> incProxyFunc;
                if (incProxy.IsStatic)
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TReturn>>));
                }
                else
                {
                    incProxyFunc = (Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TReturn>>)
                        incProxy.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = incProxyFunc(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
                inner.Successors.Set(this);
                argChangesTriggerReeval = false;
				return inner;
            }
            else if (proxyAttribute.InitializeProxyMethod(method, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)}, out var shallowProxy))
            {
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TReturn>> shallowProxyFunc;
                if (shallowProxy.IsStatic)
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TReturn>>));
                }
                else
                {
                    shallowProxyFunc = (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TReturn>>)shallowProxy.CreateDelegate(typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, INotifyValue<TReturn>>), Target.Value.Target);
                }
                var inner = shallowProxyFunc(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value, Arg9.Value, Arg10.Value);
                inner.Successors.Set(this);
                argChangesTriggerReeval = true;
				return inner;
            }
            else
            {
                var inner = Observable.Constant(Target.Value(Arg1.Value, Arg2.Value, Arg3.Value, Arg4.Value, Arg5.Value, Arg6.Value, Arg7.Value, Arg8.Value, Arg9.Value, Arg10.Value));
                argChangesTriggerReeval = true;
				inner.Successors.Set(this);
				return inner;
            }
        }

        protected override INotifyExpression<TReturn> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var targetCopy = Target.ApplyParameters(parameters, trace);
            var arg1Copy = Arg1.ApplyParameters(parameters, trace);
            var arg2Copy = Arg2.ApplyParameters(parameters, trace);
            var arg3Copy = Arg3.ApplyParameters(parameters, trace);
            var arg4Copy = Arg4.ApplyParameters(parameters, trace);
            var arg5Copy = Arg5.ApplyParameters(parameters, trace);
            var arg6Copy = Arg6.ApplyParameters(parameters, trace);
            var arg7Copy = Arg7.ApplyParameters(parameters, trace);
            var arg8Copy = Arg8.ApplyParameters(parameters, trace);
            var arg9Copy = Arg9.ApplyParameters(parameters, trace);
            var arg10Copy = Arg10.ApplyParameters(parameters, trace);

            if (Target == targetCopy && Arg1 == arg1Copy && Arg2 == arg2Copy && Arg3 == arg3Copy && Arg4 == arg4Copy && Arg5 == arg5Copy && Arg6 == arg6Copy && Arg7 == arg7Copy && Arg8 == arg8Copy && Arg9 == arg9Copy && Arg10 == arg10Copy) { return this; }

            var copy = new ObservableDelegateProxy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturn>(targetCopy, arg1Copy, arg2Copy, arg3Copy, arg4Copy, arg5Copy, arg6Copy, arg7Copy, arg8Copy, arg9Copy, arg10Copy);
            return copy;
        }
    }
    internal partial class ObservableExpressionTypes
	{
	    public static readonly Type[] DelegateProxyTypes = new Type[] { typeof(ObservableDelegateProxy<,>), typeof(ObservableDelegateProxy<,,>), typeof(ObservableDelegateProxy<,,,>), typeof(ObservableDelegateProxy<,,,,>), typeof(ObservableDelegateProxy<,,,,,>), typeof(ObservableDelegateProxy<,,,,,,>), typeof(ObservableDelegateProxy<,,,,,,,>), typeof(ObservableDelegateProxy<,,,,,,,,>), typeof(ObservableDelegateProxy<,,,,,,,,,>), typeof(ObservableDelegateProxy<,,,,,,,,,,>) };
	}
}
