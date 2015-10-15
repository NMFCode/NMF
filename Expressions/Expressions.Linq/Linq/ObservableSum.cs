using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal static class ObservableSum
    {
        public static INotifyValue<int> SumInt(INotifyEnumerable<int> source)
        {
            return new ObservableIntSum(source);
        }

        public static INotifyValue<long> SumLong(INotifyEnumerable<long> source)
        {
            return new ObservableLongSum(source);
        }

        public static INotifyValue<float> SumFloat(INotifyEnumerable<float> source)
        {
            return new ObservableFloatSum(source);
        }

        public static INotifyValue<double> SumDouble(INotifyEnumerable<double> source)
        {
            return new ObservableDoubleSum(source);
        }

        public static INotifyValue<decimal> SumDecimal(INotifyEnumerable<decimal> source)
        {
            return new ObservableDecimalSum(source);
        }

        public static INotifyValue<int?> SumNullableInt(INotifyEnumerable<int?> source)
        {
            return new ObservableNullableIntSum(source);
        }

        public static INotifyValue<long?> SumNullableLong(INotifyEnumerable<long?> source)
        {
            return new ObservableNullableLongSum(source);
        }

        public static INotifyValue<float?> SumNullableFloat(INotifyEnumerable<float?> source)
        {
            return new ObservableNullableFloatSum(source);
        }

        public static INotifyValue<double?> SumNullableDouble(INotifyEnumerable<double?> source)
        {
            return new ObservableNullableDoubleSum(source);
        }

        public static INotifyValue<decimal?> SumNullableDecimal(INotifyEnumerable<decimal?> source)
        {
            return new ObservableNullableDecimalSum(source);
        }

        public static INotifyValue<int> SumLambdaInt<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, int>> selector) 
        {
            return new ObservableIntSum(source.Select(selector));
        }

        public static INotifyValue<long> SumLambdaLong<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return new ObservableLongSum(source.Select(selector));
        }

        public static INotifyValue<float> SumLambdaFloat<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return new ObservableFloatSum(source.Select(selector));
        }

        public static INotifyValue<double> SumLambdaDouble<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return new ObservableDoubleSum(source.Select(selector));
        }

        public static INotifyValue<decimal> SumLambdaDecimal<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return new ObservableDecimalSum(source.Select(selector));
        }

        public static INotifyValue<int?> SumLambdaNullableInt<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return new ObservableNullableIntSum(source.Select(selector));
        }

        public static INotifyValue<long?> SumLambdaNullableLong<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return new ObservableNullableLongSum(source.Select(selector));
        }

        public static INotifyValue<float?> SumLambdaNullableFloat<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            return new ObservableNullableFloatSum(source.Select(selector));
        }

        public static INotifyValue<double?> SumLambdaNullableDouble<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return new ObservableNullableDoubleSum(source.Select(selector));
        }

        public static INotifyValue<decimal?> SumLambdaNullableDecimal<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return new ObservableNullableDecimalSum(source.Select(selector));
        }

        public static INotifyValue<int> SumIntExpression(IEnumerableExpression<int> source)
        {
            return new ObservableIntSum(source.AsNotifiable());
        }

        public static INotifyValue<long> SumLongExpression(IEnumerableExpression<long> source)
        {
            return new ObservableLongSum(source.AsNotifiable());
        }

        public static INotifyValue<float> SumFloatExpression(IEnumerableExpression<float> source)
        {
            return new ObservableFloatSum(source.AsNotifiable());
        }

        public static INotifyValue<double> SumDoubleExpression(IEnumerableExpression<double> source)
        {
            return new ObservableDoubleSum(source.AsNotifiable());
        }

        public static INotifyValue<decimal> SumDecimalExpression(IEnumerableExpression<decimal> source)
        {
            return new ObservableDecimalSum(source.AsNotifiable());
        }

        public static INotifyValue<int?> SumNullableIntExpression(IEnumerableExpression<int?> source)
        {
            return new ObservableNullableIntSum(source.AsNotifiable());
        }

        public static INotifyValue<long?> SumNullableLongExpression(IEnumerableExpression<long?> source)
        {
            return new ObservableNullableLongSum(source.AsNotifiable());
        }

        public static INotifyValue<float?> SumNullableFloatExpression(IEnumerableExpression<float?> source)
        {
            return new ObservableNullableFloatSum(source.AsNotifiable());
        }

        public static INotifyValue<double?> SumNullableDoubleExpression(IEnumerableExpression<double?> source)
        {
            return new ObservableNullableDoubleSum(source.AsNotifiable());
        }

        public static INotifyValue<decimal?> SumNullableDecimalExpression(IEnumerableExpression<decimal?> source)
        {
            return new ObservableNullableDecimalSum(source.AsNotifiable());
        }

        public static INotifyValue<int> SumLambdaIntExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return new ObservableIntSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<long> SumLambdaLongExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return new ObservableLongSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<float> SumLambdaFloatExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return new ObservableFloatSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<double> SumLambdaDoubleExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return new ObservableDoubleSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<decimal> SumLambdaDecimalExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return new ObservableDecimalSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<int?> SumLambdaNullableIntExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return new ObservableNullableIntSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<long?> SumLambdaNullableLongExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return new ObservableNullableLongSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<float?> SumLambdaNullableFloatExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            return new ObservableNullableFloatSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<double?> SumLambdaNullableDoubleExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return new ObservableNullableDoubleSum(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<decimal?> SumLambdaNullableDecimalExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return new ObservableNullableDecimalSum(source.AsNotifiable().Select(selector));
        }
    }

    internal class ObservableIntSum : ObservableAggregate<int, int, int>
    {
        public ObservableIntSum(INotifyEnumerable<int> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(int item)
        {
            Accumulator -= item;
        }

        protected override void AddItem(int item)
        {
            Accumulator += item;
        }

        public override int Value
        {
            get { return Accumulator; }
        }
    }

    internal class ObservableLongSum : ObservableAggregate<long, long, long>
    {
        public ObservableLongSum(INotifyEnumerable<long> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(long item)
        {
            Accumulator -= item;
        }

        protected override void AddItem(long item)
        {
            Accumulator += item;
        }

        public override long Value
        {
            get { return Accumulator; }
        }
    }

    internal class ObservableFloatSum : ObservableAggregate<float, float, float>
    {
        public ObservableFloatSum(INotifyEnumerable<float> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(float item)
        {
            Accumulator -= item;
        }

        protected override void AddItem(float item)
        {
            Accumulator += item;
        }

        public override float Value
        {
            get { return Accumulator; }
        }
    }

    internal class ObservableDoubleSum : ObservableAggregate<double, double, double>
    {
        public ObservableDoubleSum(INotifyEnumerable<double> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(double item)
        {
            Accumulator -= item;
        }

        protected override void AddItem(double item)
        {
            Accumulator += item;
        }

        public override double Value
        {
            get { return Accumulator; }
        }
    }

    internal class ObservableDecimalSum : ObservableAggregate<decimal, decimal, decimal>
    {
        public ObservableDecimalSum(INotifyEnumerable<decimal> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(decimal item)
        {
            Accumulator -= item;
        }

        protected override void AddItem(decimal item)
        {
            Accumulator += item;
        }

        public override decimal Value
        {
            get { return Accumulator; }
        }
    }

    internal class ObservableNullableIntSum : ObservableAggregate<int?, SumData<int>, int?>
    {
        public ObservableNullableIntSum(INotifyEnumerable<int?> source)
            : base(source, new SumData<int>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new SumData<int>();
        }

        protected override void RemoveItem(int? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<int>()
            {
                ElementCount = Accumulator.ElementCount - 1,
                Sum = Accumulator.Sum - item.Value
            };
        }

        protected override void AddItem(int? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<int>()
            {
                ElementCount = Accumulator.ElementCount + 1,
                Sum = Accumulator.Sum + item.Value
            };
        }

        public override int? Value
        {
            get 
            {
                if (Accumulator.ElementCount == 0)
                {
                    return null;
                }
                else
                {
                    return Accumulator.Sum;
                }
            }
        }
    }

    internal class ObservableNullableLongSum : ObservableAggregate<long?, SumData<long>, long?>
    {
        public ObservableNullableLongSum(INotifyEnumerable<long?> source)
            : base(source, new SumData<long>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new SumData<long>();
        }

        protected override void RemoveItem(long? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<long>()
            {
                ElementCount = Accumulator.ElementCount - 1,
                Sum = Accumulator.Sum - item.Value
            };
        }

        protected override void AddItem(long? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<long>()
            {
                ElementCount = Accumulator.ElementCount + 1,
                Sum = Accumulator.Sum + item.Value
            };
        }

        public override long? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0)
                {
                    return null;
                }
                else
                {
                    return Accumulator.Sum;
                }
            }
        }
    }

    internal class ObservableNullableFloatSum : ObservableAggregate<float?, SumData<float>, float?>
    {
        public ObservableNullableFloatSum(INotifyEnumerable<float?> source)
            : base(source, new SumData<float>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new SumData<float>();
        }

        protected override void RemoveItem(float? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<float>()
            {
                ElementCount = Accumulator.ElementCount - 1,
                Sum = Accumulator.Sum - item.Value
            };
        }

        protected override void AddItem(float? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<float>()
            {
                ElementCount = Accumulator.ElementCount + 1,
                Sum = Accumulator.Sum + item.Value
            };
        }

        public override float? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0)
                {
                    return null;
                }
                else
                {
                    return Accumulator.Sum;
                }
            }
        }
    }

    internal class ObservableNullableDoubleSum : ObservableAggregate<double?, SumData<double>, double?>
    {
        public ObservableNullableDoubleSum(INotifyEnumerable<double?> source)
            : base(source, new SumData<double>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new SumData<double>();
        }

        protected override void RemoveItem(double? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<double>()
            {
                ElementCount = Accumulator.ElementCount - 1,
                Sum = Accumulator.Sum - item.Value
            };
        }

        protected override void AddItem(double? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<double>()
            {
                ElementCount = Accumulator.ElementCount + 1,
                Sum = Accumulator.Sum + item.Value
            };
        }

        public override double? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0)
                {
                    return null;
                }
                else
                {
                    return Accumulator.Sum;
                }
            }
        }
    }

    internal class ObservableNullableDecimalSum : ObservableAggregate<decimal?, SumData<decimal>, decimal?>
    {
        public ObservableNullableDecimalSum(INotifyEnumerable<decimal?> source)
            : base(source, new SumData<decimal>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new SumData<decimal>();
        }

        protected override void RemoveItem(decimal? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<decimal>()
            {
                ElementCount = Accumulator.ElementCount - 1,
                Sum = Accumulator.Sum - item.Value
            };
        }

        protected override void AddItem(decimal? item)
        {
            if (!item.HasValue) return;
            Accumulator = new SumData<decimal>()
            {
                ElementCount = Accumulator.ElementCount + 1,
                Sum = Accumulator.Sum + item.Value
            };
        }

        public override decimal? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0)
                {
                    return null;
                }
                else
                {
                    return Accumulator.Sum;
                }
            }
        }
    }

    internal struct SumData<T>
    {
        public T Sum { get; set; }
        public int ElementCount { get; set; }
    }
}
