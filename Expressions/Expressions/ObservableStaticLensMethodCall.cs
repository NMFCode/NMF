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
        public Action<T1, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(TResult) }, typeof(Action<T1, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, TResult> function, Action<T1, TResult> lensPut, INotifyExpression<T1> argument1) : base(function, argument1)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, TResult> : ObservableStaticMethodCall<T1, T2, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(TResult) }, typeof(Action<T1, T2, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, TResult> function, Action<T1, T2, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2) : base(function, argument1, argument2)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, TResult> : ObservableStaticMethodCall<T1, T2, T3, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(TResult) }, typeof(Action<T1, T2, T3, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, TResult> function, Action<T1, T2, T3, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3) : base(function, argument1, argument2, argument3)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, TResult> function, Action<T1, T2, T3, T4, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4) : base(function, argument1, argument2, argument3, argument4)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, TResult> function, Action<T1, T2, T3, T4, T5, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5) : base(function, argument1, argument2, argument3, argument4, argument5)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, TResult> function, Action<T1, T2, T3, T4, T5, T6, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6) : base(function, argument1, argument2, argument3, argument4, argument5, argument6)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13, INotifyExpression<T14> argument14) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters));
        }
    }
    internal class ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, INotifyReversableExpression<TResult>
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> LensPut { get; set; }

        TResult INotifyReversableValue<TResult>.Value { get => Value; set => LensPut(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, Argument15.Value, value); }

        public bool IsReversable => LensPut != null;

        public ObservableStaticLensMethodCall(MethodCallExpression node, ObservableExpressionBinder binder) : base(node, binder)
        {
		    var lensPut = node.Method.GetCustomAttribute(typeof(LensPutAttribute)) as LensPutAttribute;
            LensPut = (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>)ExtractLensPut(node, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(TResult) }, typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>), Argument1, lensPut);
        }

        public ObservableStaticLensMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> lensPut, INotifyExpression<T1> argument1, INotifyExpression<T2> argument2, INotifyExpression<T3> argument3, INotifyExpression<T4> argument4, INotifyExpression<T5> argument5, INotifyExpression<T6> argument6, INotifyExpression<T7> argument7, INotifyExpression<T8> argument8, INotifyExpression<T9> argument9, INotifyExpression<T10> argument10, INotifyExpression<T11> argument11, INotifyExpression<T12> argument12, INotifyExpression<T13> argument13, INotifyExpression<T14> argument14, INotifyExpression<T15> argument15) : base(function, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15)
        {
            LensPut = lensPut;
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableStaticLensMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Function, LensPut, Argument1.ApplyParameters(parameters), Argument2.ApplyParameters(parameters), Argument3.ApplyParameters(parameters), Argument4.ApplyParameters(parameters), Argument5.ApplyParameters(parameters), Argument6.ApplyParameters(parameters), Argument7.ApplyParameters(parameters), Argument8.ApplyParameters(parameters), Argument9.ApplyParameters(parameters), Argument10.ApplyParameters(parameters), Argument11.ApplyParameters(parameters), Argument12.ApplyParameters(parameters), Argument13.ApplyParameters(parameters), Argument14.ApplyParameters(parameters), Argument15.ApplyParameters(parameters));
        }
    }
}
