using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableStaticMethodCall<T1, TResult> : ObservableStaticMethodBase<Func<T1, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");

            Argument1 = arg1;
        }

        public ObservableStaticMethodCall(Func<T1, TResult> function, INotifyExpression<T1> arg1)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");

            Argument1 = arg1;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, TResult>(Function, Argument1.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, TResult> : ObservableStaticMethodBase<Func<T1, T2, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");

            Argument1 = arg1;
            Argument2 = arg2;
        }

        public ObservableStaticMethodCall(Func<T1, T2, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");

            Argument1 = arg1;
            Argument2 = arg2;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
        }

        public INotifyExpression<T1> Argument1 { get; private set; }
        public INotifyExpression<T2> Argument2 { get; private set; }
        public INotifyExpression<T3> Argument3 { get; private set; }
        public INotifyExpression<T4> Argument4 { get; private set; }

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");

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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");

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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");

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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");

            Argument1 = arg1;
            Argument2 = arg2;
            Argument3 = arg3;
            Argument4 = arg4;
            Argument5 = arg5;
            Argument6 = arg6;
            Argument7 = arg7;
            Argument8 = arg8;
        }

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");

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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9)
            : base(method)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");

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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9)
            : base(function)
        {
            if (arg1 == null) throw new ArgumentNullException("arg1");
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg3 == null) throw new ArgumentNullException("arg3");
            if (arg4 == null) throw new ArgumentNullException("arg4");
            if (arg5 == null) throw new ArgumentNullException("arg5");
            if (arg6 == null) throw new ArgumentNullException("arg6");
            if (arg7 == null) throw new ArgumentNullException("arg7");
            if (arg8 == null) throw new ArgumentNullException("arg8");
            if (arg9 == null) throw new ArgumentNullException("arg9");

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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9]), binder.VisitObservable<T11>(expression.Arguments[10])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant && Argument11.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
                yield return Argument11;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9]), binder.VisitObservable<T11>(expression.Arguments[10]), binder.VisitObservable<T12>(expression.Arguments[11])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant && Argument11.IsConstant && Argument12.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
                yield return Argument11;
                yield return Argument12;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9]), binder.VisitObservable<T11>(expression.Arguments[10]), binder.VisitObservable<T12>(expression.Arguments[11]), binder.VisitObservable<T13>(expression.Arguments[12])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant && Argument11.IsConstant && Argument12.IsConstant && Argument13.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
                yield return Argument11;
                yield return Argument12;
                yield return Argument13;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9]), binder.VisitObservable<T11>(expression.Arguments[10]), binder.VisitObservable<T12>(expression.Arguments[11]), binder.VisitObservable<T13>(expression.Arguments[12]), binder.VisitObservable<T14>(expression.Arguments[13])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant && Argument11.IsConstant && Argument12.IsConstant && Argument13.IsConstant && Argument14.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
                yield return Argument11;
                yield return Argument12;
                yield return Argument13;
                yield return Argument14;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), Argument14.ApplyParameters(parameters, trace));
        }
    }
    internal class ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableStaticMethodBase<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TResult>
    {
        public ObservableStaticMethodCall(MethodCallExpression expression, ObservableExpressionBinder binder)
            : this(expression.Method, binder.VisitObservable<T1>(expression.Arguments[0]), binder.VisitObservable<T2>(expression.Arguments[1]), binder.VisitObservable<T3>(expression.Arguments[2]), binder.VisitObservable<T4>(expression.Arguments[3]), binder.VisitObservable<T5>(expression.Arguments[4]), binder.VisitObservable<T6>(expression.Arguments[5]), binder.VisitObservable<T7>(expression.Arguments[6]), binder.VisitObservable<T8>(expression.Arguments[7]), binder.VisitObservable<T9>(expression.Arguments[8]), binder.VisitObservable<T10>(expression.Arguments[9]), binder.VisitObservable<T11>(expression.Arguments[10]), binder.VisitObservable<T12>(expression.Arguments[11]), binder.VisitObservable<T13>(expression.Arguments[12]), binder.VisitObservable<T14>(expression.Arguments[13]), binder.VisitObservable<T15>(expression.Arguments[14])) { }

        public ObservableStaticMethodCall(MethodInfo method, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14, INotifyExpression<T15> arg15)
            : base(method)
        {
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

        public ObservableStaticMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, INotifyExpression<T1> arg1, INotifyExpression<T2> arg2, INotifyExpression<T3> arg3, INotifyExpression<T4> arg4, INotifyExpression<T5> arg5, INotifyExpression<T6> arg6, INotifyExpression<T7> arg7, INotifyExpression<T8> arg8, INotifyExpression<T9> arg9, INotifyExpression<T10> arg10, INotifyExpression<T11> arg11, INotifyExpression<T12> arg12, INotifyExpression<T13> arg13, INotifyExpression<T14> arg14, INotifyExpression<T15> arg15)
            : base(function)
        {
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

        public override bool IsConstant
        {
            get
            {
                return Argument1.IsConstant && Argument2.IsConstant && Argument3.IsConstant && Argument4.IsConstant && Argument5.IsConstant && Argument6.IsConstant && Argument7.IsConstant && Argument8.IsConstant && Argument9.IsConstant && Argument10.IsConstant && Argument11.IsConstant && Argument12.IsConstant && Argument13.IsConstant && Argument14.IsConstant && Argument15.IsConstant;
            }
        }

        public override bool IsParameterFree
        {
            get { return Argument1.IsParameterFree && Argument2.IsParameterFree && Argument3.IsParameterFree && Argument4.IsParameterFree && Argument5.IsParameterFree && Argument6.IsParameterFree && Argument7.IsParameterFree && Argument8.IsParameterFree && Argument9.IsParameterFree && Argument10.IsParameterFree && Argument11.IsParameterFree && Argument12.IsParameterFree && Argument13.IsParameterFree && Argument14.IsParameterFree && Argument15.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Argument1;
                yield return Argument2;
                yield return Argument3;
                yield return Argument4;
                yield return Argument5;
                yield return Argument6;
                yield return Argument7;
                yield return Argument8;
                yield return Argument9;
                yield return Argument10;
                yield return Argument11;
                yield return Argument12;
                yield return Argument13;
                yield return Argument14;
                yield return Argument15;
            }
        }

        protected override TResult GetValue()
        {
            return Function(Argument1.Value, Argument2.Value, Argument3.Value, Argument4.Value, Argument5.Value, Argument6.Value, Argument7.Value, Argument8.Value, Argument9.Value, Argument10.Value, Argument11.Value, Argument12.Value, Argument13.Value, Argument14.Value, Argument15.Value);
        }

        protected override INotifyExpression<TResult> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Function, Argument1.ApplyParameters(parameters, trace), Argument2.ApplyParameters(parameters, trace), Argument3.ApplyParameters(parameters, trace), Argument4.ApplyParameters(parameters, trace), Argument5.ApplyParameters(parameters, trace), Argument6.ApplyParameters(parameters, trace), Argument7.ApplyParameters(parameters, trace), Argument8.ApplyParameters(parameters, trace), Argument9.ApplyParameters(parameters, trace), Argument10.ApplyParameters(parameters, trace), Argument11.ApplyParameters(parameters, trace), Argument12.ApplyParameters(parameters, trace), Argument13.ApplyParameters(parameters, trace), Argument14.ApplyParameters(parameters, trace), Argument15.ApplyParameters(parameters, trace));
        }
    }
}