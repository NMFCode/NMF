using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal class ObservableStaticLensMethodCall<T1, TResult> : ObservableStaticMethodCall<T1, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, TResult> function, LensPut<T1, TResult> lensPut, INotifyExpression<T1> argument1) : base(function, argument1)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1);
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, TResult> : ObservableStaticMethodCall<T1, T2, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, TResult> function, LensPut<T1, T2, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2) : base(function, argument1, argument2)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, TResult> : ObservableStaticMethodCall<T1, T2, T3, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, TResult> function, LensPut<T1, T2, T3, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3) : base(function, argument1, argument2, argument3)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, TResult> function, LensPut<T1, T2, T3, T4, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4) : base(function, argument1, argument2, argument3, argument4)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, TResult> function, LensPut<T1, T2, T3, T4, T5, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5) : base(function, argument1, argument2, argument3, argument4, argument5)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6) : base(function, argument1, argument2, argument3, argument4, argument5, argument6)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13, INotifyExpression<T14> argument14) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), Argument14.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, INotifyReversableExpression<TResult>
    {
        public LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value
		{
			get
			{
				return Value;
			}
			set
			{
				Successors.SetDummy();
				LensPut.SetValue(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, Argument15.Value, value);
			}
		}

        public bool IsReversable
		{
			get
			{
				return LensPut != null && LensPut.CanApply;
			}
		}

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPutAttribute = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>.FromLensPutAttribute(lensPutAttribute, node.Method, Argument1 as INotifyReversableValue<T1>);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, LensPut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13, INotifyExpression<T14> argument14, INotifyExpression<T15> argument15) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15)
        {
            LensPut = lensPut;
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
			var newArgument1 = Argument1.ApplyParameters(parameters, trace);
			if (newArgument1 == Argument1) return this;
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Function, LensPut.ApplyNewTarget(newArgument1 as INotifyReversableValue<T1>), newArgument1, Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), Argument14.ApplyParameters(parameters, trace), Argument15.ApplyParameters(parameters, trace));
        }
    }
}
