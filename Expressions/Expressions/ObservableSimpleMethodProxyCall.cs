using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal sealed class ObservableSimpleMethodProxyCall<T, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");

			Target = target;
			ProxyMethod = proxyFunction;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }

        public Func<INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction();
        }

        protected override void AttachCore()
        {
			Target.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, TResult>(Target.ApplyParameters(parameters), ProxyMethod);
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");
			if (arg4 == null) throw new ArgumentNullException("arg4");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");
			if (arg4 == null) throw new ArgumentNullException("arg4");
			if (arg5 == null) throw new ArgumentNullException("arg5");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			Argument5 = arg5;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }
		public INotifyExpression<T5> Argument5 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");
			if (arg4 == null) throw new ArgumentNullException("arg4");
			if (arg5 == null) throw new ArgumentNullException("arg5");
			if (arg6 == null) throw new ArgumentNullException("arg6");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			Argument5 = arg5;
			Argument6 = arg6;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }
		public INotifyExpression<T5> Argument5 { get; private set; }
		public INotifyExpression<T6> Argument6 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");
			if (arg4 == null) throw new ArgumentNullException("arg4");
			if (arg5 == null) throw new ArgumentNullException("arg5");
			if (arg6 == null) throw new ArgumentNullException("arg6");
			if (arg7 == null) throw new ArgumentNullException("arg7");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			Argument5 = arg5;
			Argument6 = arg6;
			Argument7 = arg7;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }
		public INotifyExpression<T5> Argument5 { get; private set; }
		public INotifyExpression<T6> Argument6 { get; private set; }
		public INotifyExpression<T7> Argument7 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8)
        {
			if (target == null) throw new ArgumentNullException("target");
            if (proxyFunction == null) throw new ArgumentNullException("proxyFunction");
			if (arg1 == null) throw new ArgumentNullException("arg1");
			if (arg2 == null) throw new ArgumentNullException("arg2");
			if (arg3 == null) throw new ArgumentNullException("arg3");
			if (arg4 == null) throw new ArgumentNullException("arg4");
			if (arg5 == null) throw new ArgumentNullException("arg5");
			if (arg6 == null) throw new ArgumentNullException("arg6");
			if (arg7 == null) throw new ArgumentNullException("arg7");
			if (arg8 == null) throw new ArgumentNullException("arg8");

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			Argument5 = arg5;
			Argument6 = arg6;
			Argument7 = arg7;
			Argument8 = arg8;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }
		public INotifyExpression<T5> Argument5 { get; private set; }
		public INotifyExpression<T6> Argument6 { get; private set; }
		public INotifyExpression<T7> Argument7 { get; private set; }
		public INotifyExpression<T8> Argument8 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
			Argument1 = arg1;
			Argument2 = arg2;
			Argument3 = arg3;
			Argument4 = arg4;
			Argument5 = arg5;
			Argument6 = arg6;
			Argument7 = arg7;
			Argument8 = arg8;
			Argument9 = arg9;
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
		public INotifyExpression<T1> Argument1 { get; private set; }
		public INotifyExpression<T2> Argument2 { get; private set; }
		public INotifyExpression<T3> Argument3 { get; private set; }
		public INotifyExpression<T4> Argument4 { get; private set; }
		public INotifyExpression<T5> Argument5 { get; private set; }
		public INotifyExpression<T6> Argument6 { get; private set; }
		public INotifyExpression<T7> Argument7 { get; private set; }
		public INotifyExpression<T8> Argument8 { get; private set; }
		public INotifyExpression<T9> Argument9 { get; private set; }

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 11))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10, Argument11);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			Argument11.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
			Argument11.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 12))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10, Argument11, Argument12);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			Argument11.Attach();
			Argument12.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
			Argument11.Detach();
			Argument12.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 12)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 13))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10, Argument11, Argument12, Argument13);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			Argument11.Attach();
			Argument12.Attach();
			Argument13.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
			Argument11.Detach();
			Argument12.Detach();
			Argument13.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 12)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 13)), binder.VisitObservable<T14>(ExpressionHelper.GetArg(node, 14))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10, Argument11, Argument12, Argument13, Argument14);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			Argument11.Attach();
			Argument12.Attach();
			Argument13.Attach();
			Argument14.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
			Argument11.Detach();
			Argument12.Detach();
			Argument13.Detach();
			Argument14.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters));
        }
    }
    internal sealed class ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableProxyCallBase<TResult>
    {
        public ObservableSimpleMethodProxyCall(MethodCallExpression node, ObservableExpressionBinder binder, MethodInfo proxyMethod)
            : this(binder.VisitObservable<T>(ExpressionHelper.GetArg(node, 0)), proxyMethod, binder.VisitObservable<T1>(ExpressionHelper.GetArg(node, 1)), binder.VisitObservable<T2>(ExpressionHelper.GetArg(node, 2)), binder.VisitObservable<T3>(ExpressionHelper.GetArg(node, 3)), binder.VisitObservable<T4>(ExpressionHelper.GetArg(node, 4)), binder.VisitObservable<T5>(ExpressionHelper.GetArg(node, 5)), binder.VisitObservable<T6>(ExpressionHelper.GetArg(node, 6)), binder.VisitObservable<T7>(ExpressionHelper.GetArg(node, 7)), binder.VisitObservable<T8>(ExpressionHelper.GetArg(node, 8)), binder.VisitObservable<T9>(ExpressionHelper.GetArg(node, 9)), binder.VisitObservable<T10>(ExpressionHelper.GetArg(node, 10)), binder.VisitObservable<T11>(ExpressionHelper.GetArg(node, 11)), binder.VisitObservable<T12>(ExpressionHelper.GetArg(node, 12)), binder.VisitObservable<T13>(ExpressionHelper.GetArg(node, 13)), binder.VisitObservable<T14>(ExpressionHelper.GetArg(node, 14)), binder.VisitObservable<T15>(ExpressionHelper.GetArg(node, 15))) { }

        public ObservableSimpleMethodProxyCall(INotifyExpression<T> target, MethodInfo proxyFunction, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14, INotifyExpression<T15> arg15)
        {
			if (target == null) throw new ArgumentNullException("target");
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

			Target = target;
			ProxyMethod = proxyFunction;
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
			
			Target.ValueChanged += TargetChanged;
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			RenewProxyFunction();
			RenewProxy();
			Refresh();
		}

		public INotifyExpression<T> Target { get; private set; }
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

        public Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<T15>, INotifyValue<TResult>> ProxyFunction { get; private set; }

		public MethodInfo ProxyMethod { get; private set; }

		private void RenewProxyFunction()
		{
			ProxyFunction = ReflectionHelper.CreateDelegate(typeof(Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<T15>, INotifyValue<TResult>>), Target.Value, ProxyMethod) as Func<INotifyValue<T1>, INotifyValue<T2>, INotifyValue<T3>, INotifyValue<T4>, INotifyValue<T5>, INotifyValue<T6>, INotifyValue<T7>, INotifyValue<T8>, INotifyValue<T9>, INotifyValue<T10>, INotifyValue<T11>, INotifyValue<T12>, INotifyValue<T13>, INotifyValue<T14>, INotifyValue<T15>, INotifyValue<TResult>>;
		}

        protected override INotifyValue<TResult> CreateProxy()
        {
            return ProxyFunction(Argument1, Argument2, Argument3, Argument4, Argument5, Argument6, Argument7, Argument8, Argument9, Argument10, Argument11, Argument12, Argument13, Argument14, Argument15);
        }

        protected override void AttachCore()
        {
			Target.Attach();
			Argument1.Attach();
			Argument2.Attach();
			Argument3.Attach();
			Argument4.Attach();
			Argument5.Attach();
			Argument6.Attach();
			Argument7.Attach();
			Argument8.Attach();
			Argument9.Attach();
			Argument10.Attach();
			Argument11.Attach();
			Argument12.Attach();
			Argument13.Attach();
			Argument14.Attach();
			Argument15.Attach();
			RenewProxyFunction();
            base.AttachCore();
        }

        protected override void DetachCore()
        {
            base.DetachCore();
			Target.Detach();
			Argument1.Detach();
			Argument2.Detach();
			Argument3.Detach();
			Argument4.Detach();
			Argument5.Detach();
			Argument6.Detach();
			Argument7.Detach();
			Argument8.Detach();
			Argument9.Detach();
			Argument10.Detach();
			Argument11.Detach();
			Argument12.Detach();
			Argument13.Detach();
			Argument14.Detach();
			Argument15.Detach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree && Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree && Argument15.IsParameterFree; }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableSimpleMethodProxyCall<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Target.ApplyParameters(parameters), ProxyMethod, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters), Argument15.ApplyParameters(parameters));
        }
    }
}