using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableNewExpression<T1, T> : ObservableStaticMethodCall<T1, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0])) { }

        private static Func<T1, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var expression = Expression.Lambda<Func<T1, T>>(
                Expression.New(node.Constructor, p1), p1);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T> : ObservableStaticMethodCall<T1, T2, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1])) { }

        private static Func<T1, T2, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var expression = Expression.Lambda<Func<T1, T2, T>>(
                Expression.New(node.Constructor, p1, p2), p1, p2);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T> : ObservableStaticMethodCall<T1, T2, T3, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2])) { }

        private static Func<T1, T2, T3, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var expression = Expression.Lambda<Func<T1, T2, T3, T>>(
                Expression.New(node.Constructor, p1, p2, p3), p1, p2, p3);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3])) { }

        private static Func<T1, T2, T3, T4, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4), p1, p2, p3, p4);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4])) { }

        private static Func<T1, T2, T3, T4, T5, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5), p1, p2, p3, p4, p5);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6), p1, p2, p3, p4, p5, p6);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7), p1, p2, p3, p4, p5, p6, p7);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8), p1, p2, p3, p4, p5, p6, p7, p8);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9), p1, p2, p3, p4, p5, p6, p7, p8, p9);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9]), binder.VisitObservable<T11>(node.Arguments[10])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var p11 = Expression.Parameter(node.Arguments[10].Type, "p11");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9]), binder.VisitObservable<T11>(node.Arguments[10]), binder.VisitObservable<T12>(node.Arguments[11])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var p11 = Expression.Parameter(node.Arguments[10].Type, "p11");
            var p12 = Expression.Parameter(node.Arguments[11].Type, "p12");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9]), binder.VisitObservable<T11>(node.Arguments[10]), binder.VisitObservable<T12>(node.Arguments[11]), binder.VisitObservable<T13>(node.Arguments[12])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var p11 = Expression.Parameter(node.Arguments[10].Type, "p11");
            var p12 = Expression.Parameter(node.Arguments[11].Type, "p12");
            var p13 = Expression.Parameter(node.Arguments[12].Type, "p13");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9]), binder.VisitObservable<T11>(node.Arguments[10]), binder.VisitObservable<T12>(node.Arguments[11]), binder.VisitObservable<T13>(node.Arguments[12]), binder.VisitObservable<T14>(node.Arguments[13])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var p11 = Expression.Parameter(node.Arguments[10].Type, "p11");
            var p12 = Expression.Parameter(node.Arguments[11].Type, "p12");
            var p13 = Expression.Parameter(node.Arguments[12].Type, "p13");
            var p14 = Expression.Parameter(node.Arguments[13].Type, "p14");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
            return expression.Compile();
        }

    }
    internal class ObservableNewExpression<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T>
    {
        public ObservableNewExpression(NewExpression node, ObservableExpressionBinder binder)
            : base(CreateDelegate(node), binder.VisitObservable<T1>(node.Arguments[0]), binder.VisitObservable<T2>(node.Arguments[1]), binder.VisitObservable<T3>(node.Arguments[2]), binder.VisitObservable<T4>(node.Arguments[3]), binder.VisitObservable<T5>(node.Arguments[4]), binder.VisitObservable<T6>(node.Arguments[5]), binder.VisitObservable<T7>(node.Arguments[6]), binder.VisitObservable<T8>(node.Arguments[7]), binder.VisitObservable<T9>(node.Arguments[8]), binder.VisitObservable<T10>(node.Arguments[9]), binder.VisitObservable<T11>(node.Arguments[10]), binder.VisitObservable<T12>(node.Arguments[11]), binder.VisitObservable<T13>(node.Arguments[12]), binder.VisitObservable<T14>(node.Arguments[13]), binder.VisitObservable<T15>(node.Arguments[14])) { }

        private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> CreateDelegate(NewExpression node)
        {
            var p1 = Expression.Parameter(node.Arguments[0].Type, "p1");
            var p2 = Expression.Parameter(node.Arguments[1].Type, "p2");
            var p3 = Expression.Parameter(node.Arguments[2].Type, "p3");
            var p4 = Expression.Parameter(node.Arguments[3].Type, "p4");
            var p5 = Expression.Parameter(node.Arguments[4].Type, "p5");
            var p6 = Expression.Parameter(node.Arguments[5].Type, "p6");
            var p7 = Expression.Parameter(node.Arguments[6].Type, "p7");
            var p8 = Expression.Parameter(node.Arguments[7].Type, "p8");
            var p9 = Expression.Parameter(node.Arguments[8].Type, "p9");
            var p10 = Expression.Parameter(node.Arguments[9].Type, "p10");
            var p11 = Expression.Parameter(node.Arguments[10].Type, "p11");
            var p12 = Expression.Parameter(node.Arguments[11].Type, "p12");
            var p13 = Expression.Parameter(node.Arguments[12].Type, "p13");
            var p14 = Expression.Parameter(node.Arguments[13].Type, "p14");
            var p15 = Expression.Parameter(node.Arguments[14].Type, "p15");
            var expression = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T>>(
                Expression.New(node.Constructor, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15), p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
            return expression.Compile();
        }

    }
}