using System;

namespace NMF.Expressions
{
    internal class RecurseInfo<T1, TResult>
    {
        public ObservingFunc<Func<T1, TResult>, T1, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, TResult>, T1, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1)
        {
            return InnerFunc.Evaluate(Evaluate, in1);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1)
        {
            return InnerFunc.Observe(Func, in1);
        }
    }
    internal class RecurseInfo<T1, T2, TResult>
    {
        public ObservingFunc<Func<T1, T2, TResult>, T1, T2, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, TResult>, T1, T2, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2)
        {
            return InnerFunc.Observe(Func, in1, in2);
        }
    }
    internal class RecurseInfo<T1, T2, T3, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, TResult>, T1, T2, T3, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, TResult>, T1, T2, T3, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3)
        {
            return InnerFunc.Observe(Func, in1, in2, in3);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, TResult>, T1, T2, T3, T4, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, TResult>, T1, T2, T3, T4, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, T5, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, T5, TResult>, T1, T2, T3, T4, T5, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, T5, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, T5, TResult>, T1, T2, T3, T4, T5, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, T5, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4, in5);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4, in5);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, T5, T6, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, T5, T6, TResult>, T1, T2, T3, T4, T5, T6, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, T5, T6, TResult>, T1, T2, T3, T4, T5, T6, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, T5, T6, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4, in5, in6);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4, in5, in6);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, T1, T2, T3, T4, T5, T6, T7, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, TResult>, T1, T2, T3, T4, T5, T6, T7, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, T5, T6, T7, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4, in5, in6, in7);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4, in5, in6, in7);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4, in5, in6, in7, in8);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4, in5, in6, in7, in8);
        }
    }
    internal class RecurseInfo<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        public ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> InnerFunc { get; }
        public INotifyExpression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> Func { get; }

        public RecurseInfo(ObservingFunc<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> inner)
        {
            InnerFunc = inner;
            Func = Observable.Constant(new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Evaluate));
        }

        [ObservableProxy(typeof(RecurseInfo<,,,,,,,,,>), "Observe", isRecursive: true)]
        public TResult Evaluate(T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, T7 in7, T8 in8, T9 in9)
        {
            return InnerFunc.Evaluate(Evaluate, in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        public INotifyValue<TResult> Observe(INotifyValue<T1> in1, INotifyValue<T2> in2, INotifyValue<T3> in3, INotifyValue<T4> in4, INotifyValue<T5> in5, INotifyValue<T6> in6, INotifyValue<T7> in7, INotifyValue<T8> in8, INotifyValue<T9> in9)
        {
            return InnerFunc.Observe(Func, in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }
    }
}