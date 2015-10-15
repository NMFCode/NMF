using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal static class ObservableAverage
    {
        public static INotifyValue<double> CreateInt<TSource>(INotifyEnumerable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return new ObservableIntAverage(source.Select(selector));
        }

        public static INotifyValue<double> CreateLong<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long>> predicate)
        {
            return new ObservableLongAverage(source.Select(predicate));
        }

        public static INotifyValue<float> CreateFloat<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float>> predicate)
        {
            return new ObservableFloatAverage(source.Select(predicate));
        }

        public static INotifyValue<double> CreateDouble<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double>> predicate)
        {
            return new ObservableDoubleAverage(source.Select(predicate));
        }

        public static INotifyValue<decimal> CreateDecimal<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal>> predicate)
        {
            return new ObservableDecimalAverage(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableInt<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int?>> predicate)
        {
            return new ObservableNullableIntAverage(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableLong<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long?>> predicate)
        {
            return new ObservableNullableLongAverage(source.Select(predicate));
        }

        public static INotifyValue<float?> CreateNullableFloat<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float?>> predicate)
        {
            return new ObservableNullableFloatAverage(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableDouble<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double?>> predicate)
        {
            return new ObservableNullableDoubleAverage(source.Select(predicate));
        }

        public static INotifyValue<decimal?> CreateNullableDecimal<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal?>> predicate)
        {
            return new ObservableNullableDecimalAverage(source.Select(predicate));
        }

        public static INotifyValue<double> CreateIntExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return new ObservableIntAverage(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<double> CreateLongExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long>> predicate)
        {
            return new ObservableLongAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<float> CreateFloatExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float>> predicate)
        {
            return new ObservableFloatAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double> CreateDoubleExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double>> predicate)
        {
            return new ObservableDoubleAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<decimal> CreateDecimalExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal>> predicate)
        {
            return new ObservableDecimalAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableIntExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int?>> predicate)
        {
            return new ObservableNullableIntAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableLongExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long?>> predicate)
        {
            return new ObservableNullableLongAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<float?> CreateNullableFloatExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float?>> predicate)
        {
            return new ObservableNullableFloatAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableDoubleExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double?>> predicate)
        {
            return new ObservableNullableDoubleAverage(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<decimal?> CreateNullableDecimalExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal?>> predicate)
        {
            return new ObservableNullableDecimalAverage(source.AsNotifiable().Select(predicate));
        }
    }

    internal class ObservableIntAverage : ObservableAggregate<int, AverageData<int>, double>
    {
        public static ObservableIntAverage Create(INotifyEnumerable<int> source)
        {
            return new ObservableIntAverage(source);
        }

        public static ObservableIntAverage CreateExpression(IEnumerableExpression<int> source)
        {
            return new ObservableIntAverage(source.AsNotifiable());
        }

        public ObservableIntAverage(INotifyEnumerable<int> source)
            : base(source, new AverageData<int>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<int>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(int item)
        {
            Accumulator = new AverageData<int>()
            {
                Sum = Accumulator.Sum - item,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(int item)
        {
            Accumulator = new AverageData<int>()
            {
                Sum = Accumulator.Sum + item,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return double.NaN;
                return ((double)Accumulator.Sum) / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableLongAverage : ObservableAggregate<long, AverageData<long>, double>
    {
        public static ObservableLongAverage Create(INotifyEnumerable<long> source)
        {
            return new ObservableLongAverage(source);
        }

        public static ObservableLongAverage CreateExpression(IEnumerableExpression<long> source)
        {
            return new ObservableLongAverage(source.AsNotifiable());
        }

        public ObservableLongAverage(INotifyEnumerable<long> source)
            : base(source, new AverageData<long>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<long>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(long item)
        {
            Accumulator = new AverageData<long>()
            {
                Sum = Accumulator.Sum - item,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(long item)
        {
            Accumulator = new AverageData<long>()
            {
                Sum = Accumulator.Sum + item,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return double.NaN;
                return ((double)Accumulator.Sum) / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableFloatAverage : ObservableAggregate<float, AverageData<float>, float>
    {
        public static ObservableFloatAverage Create(INotifyEnumerable<float> source)
        {
            return new ObservableFloatAverage(source);
        }

        public static ObservableFloatAverage CreateExpression(IEnumerableExpression<float> source)
        {
            return new ObservableFloatAverage(source.AsNotifiable());
        }

        public ObservableFloatAverage(INotifyEnumerable<float> source)
            : base(source, new AverageData<float>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<float>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(float item)
        {
            Accumulator = new AverageData<float>()
            {
                Sum = Accumulator.Sum - item,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(float item)
        {
            Accumulator = new AverageData<float>()
            {
                Sum = Accumulator.Sum + item,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override float Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return float.NaN;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableDoubleAverage : ObservableAggregate<double, AverageData<double>, double>
    {
        public static ObservableDoubleAverage Create(INotifyEnumerable<double> source)
        {
            return new ObservableDoubleAverage(source);
        }

        public static ObservableDoubleAverage CreateExpression(IEnumerableExpression<double> source)
        {
            return new ObservableDoubleAverage(source.AsNotifiable());
        }

        public ObservableDoubleAverage(INotifyEnumerable<double> source)
            : base(source, new AverageData<double>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<double>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(double item)
        {
            Accumulator = new AverageData<double>()
            {
                Sum = Accumulator.Sum - item,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(double item)
        {
            Accumulator = new AverageData<double>()
            {
                Sum = Accumulator.Sum + item,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return double.NaN;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableDecimalAverage : ObservableAggregate<decimal, AverageData<decimal>, decimal>
    {
        public static ObservableDecimalAverage Create(INotifyEnumerable<decimal> source)
        {
            return new ObservableDecimalAverage(source);
        }

        public static ObservableDecimalAverage CreateExpression(IEnumerableExpression<decimal> source)
        {
            return new ObservableDecimalAverage(source.AsNotifiable());
        }

        public ObservableDecimalAverage(INotifyEnumerable<decimal> source)
            : base(source, new AverageData<decimal>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<decimal>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(decimal item)
        {
            Accumulator = new AverageData<decimal>()
            {
                Sum = Accumulator.Sum - item,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(decimal item)
        {
            Accumulator = new AverageData<decimal>()
            {
                Sum = Accumulator.Sum + item,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override decimal Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return decimal.Zero;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }
    
    internal class ObservableNullableIntAverage : ObservableAggregate<int?, AverageData<int>, double?>
    {
        public static ObservableNullableIntAverage Create(INotifyEnumerable<int?> source)
        {
            return new ObservableNullableIntAverage(source);
        }

        public static ObservableNullableIntAverage CreateExpression(IEnumerableExpression<int?> source)
        {
            return new ObservableNullableIntAverage(source.AsNotifiable());
        }

        public ObservableNullableIntAverage(INotifyEnumerable<int?> source)
            : base(source, new AverageData<int>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<int>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(int? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<int>()
            {
                Sum = Accumulator.Sum - item.Value,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(int? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<int>()
            {
                Sum = Accumulator.Sum + item.Value,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return null;
                return ((double)Accumulator.Sum) / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableNullableLongAverage : ObservableAggregate<long?, AverageData<long>, double?>
    {
        public static ObservableNullableLongAverage Create(INotifyEnumerable<long?> source)
        {
            return new ObservableNullableLongAverage(source);
        }

        public static ObservableNullableLongAverage CreateExpression(IEnumerableExpression<long?> source)
        {
            return new ObservableNullableLongAverage(source.AsNotifiable());
        }

        public ObservableNullableLongAverage(INotifyEnumerable<long?> source)
            : base(source, new AverageData<long>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<long>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(long? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<long>()
            {
                Sum = Accumulator.Sum - item.Value,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(long? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<long>()
            {
                Sum = Accumulator.Sum + item.Value,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return null;
                return ((double)Accumulator.Sum) / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableNullableFloatAverage : ObservableAggregate<float?, AverageData<float>, float?>
    {
        public static ObservableNullableFloatAverage Create(INotifyEnumerable<float?> source)
        {
            return new ObservableNullableFloatAverage(source);
        }

        public static ObservableNullableFloatAverage CreateExpression(IEnumerableExpression<float?> source)
        {
            return new ObservableNullableFloatAverage(source.AsNotifiable());
        }

        public ObservableNullableFloatAverage(INotifyEnumerable<float?> source)
            : base(source, new AverageData<float>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<float>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(float? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<float>()
            {
                Sum = Accumulator.Sum - item.Value,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(float? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<float>()
            {
                Sum = Accumulator.Sum + item.Value,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override float? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return null;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableNullableDoubleAverage : ObservableAggregate<double?, AverageData<double>, double?>
    {
        public static ObservableNullableDoubleAverage Create(INotifyEnumerable<double?> source)
        {
            return new ObservableNullableDoubleAverage(source);
        }

        public static ObservableNullableDoubleAverage CreateExpression(IEnumerableExpression<double?> source)
        {
            return new ObservableNullableDoubleAverage(source.AsNotifiable());
        }

        public ObservableNullableDoubleAverage(INotifyEnumerable<double?> source)
            : base(source, new AverageData<double>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<double>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(double? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<double>()
            {
                Sum = Accumulator.Sum - item.Value,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(double? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<double>()
            {
                Sum = Accumulator.Sum + item.Value,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override double? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return null;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }

    internal class ObservableNullableDecimalAverage : ObservableAggregate<decimal?, AverageData<decimal>, decimal?>
    {
        public static ObservableNullableDecimalAverage Create(INotifyEnumerable<decimal?> source)
        {
            return new ObservableNullableDecimalAverage(source);
        }

        public static ObservableNullableDecimalAverage CreateExpression(IEnumerableExpression<decimal?> source)
        {
            return new ObservableNullableDecimalAverage(source.AsNotifiable());
        }

        public ObservableNullableDecimalAverage(INotifyEnumerable<decimal?> source)
            : base(source, new AverageData<decimal>())
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = new AverageData<decimal>()
            {
                Sum = 0,
                ElementCount = 0
            };
        }

        protected override void RemoveItem(decimal? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<decimal>()
            {
                Sum = Accumulator.Sum - item.Value,
                ElementCount = Accumulator.ElementCount - 1
            };
        }

        protected override void AddItem(decimal? item)
        {
            if (!item.HasValue) return;
            Accumulator = new AverageData<decimal>()
            {
                Sum = Accumulator.Sum + item.Value,
                ElementCount = Accumulator.ElementCount + 1
            };
        }

        public override decimal? Value
        {
            get
            {
                if (Accumulator.ElementCount == 0) return null;
                return Accumulator.Sum / Accumulator.ElementCount;
            }
        }
    }
    
    internal struct AverageData<T>
    {
        public int ElementCount { get; set; }

        public T Sum { get; set; }
    }
}
