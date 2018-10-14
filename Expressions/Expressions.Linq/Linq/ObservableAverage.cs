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
            return ObservableIntAverage.Create(source.Select(selector));
        }

        public static INotifyValue<double> CreateLong<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long>> predicate)
        {
            return ObservableLongAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<float> CreateFloat<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float>> predicate)
        {
            return ObservableFloatAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<double> CreateDouble<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double>> predicate)
        {
            return ObservableDoubleAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<decimal> CreateDecimal<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal>> predicate)
        {
            return ObservableDecimalAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableInt<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int?>> predicate)
        {
            return ObservableNullableIntAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableLong<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long?>> predicate)
        {
            return ObservableNullableLongAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<float?> CreateNullableFloat<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float?>> predicate)
        {
            return ObservableNullableFloatAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableDouble<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double?>> predicate)
        {
            return ObservableNullableDoubleAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<decimal?> CreateNullableDecimal<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal?>> predicate)
        {
            return ObservableNullableDecimalAverage.Create(source.Select(predicate));
        }

        public static INotifyValue<double> CreateIntExpression<TSource>(IEnumerableExpression<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return ObservableIntAverage.Create(source.AsNotifiable().Select(selector));
        }

        public static INotifyValue<double> CreateLongExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long>> predicate)
        {
            return ObservableLongAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<float> CreateFloatExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float>> predicate)
        {
            return ObservableFloatAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double> CreateDoubleExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double>> predicate)
        {
            return ObservableDoubleAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<decimal> CreateDecimalExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal>> predicate)
        {
            return ObservableDecimalAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableIntExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int?>> predicate)
        {
            return ObservableNullableIntAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableLongExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long?>> predicate)
        {
            return ObservableNullableLongAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<float?> CreateNullableFloatExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float?>> predicate)
        {
            return ObservableNullableFloatAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<double?> CreateNullableDoubleExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double?>> predicate)
        {
            return ObservableNullableDoubleAverage.Create(source.AsNotifiable().Select(predicate));
        }

        public static INotifyValue<decimal?> CreateNullableDecimalExpression<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal?>> predicate)
        {
            return ObservableNullableDecimalAverage.Create(source.AsNotifiable().Select(predicate));
        }
    }

    internal class ObservableIntAverage : ObservableAggregate<int, AverageData<int>, double>
    {
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableIntAverage Create(INotifyEnumerable<int> source)
        {
            var observable = new ObservableIntAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableIntAverage CreateExpression(IEnumerableExpression<int> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableLongAverage Create(INotifyEnumerable<long> source)
        {
            var observable = new ObservableLongAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableLongAverage CreateExpression(IEnumerableExpression<long> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableFloatAverage Create(INotifyEnumerable<float> source)
        {
            var observable = new ObservableFloatAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableFloatAverage CreateExpression(IEnumerableExpression<float> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableDoubleAverage Create(INotifyEnumerable<double> source)
        {
            var observable = new ObservableDoubleAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableDoubleAverage CreateExpression(IEnumerableExpression<double> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableDecimalAverage Create(INotifyEnumerable<decimal> source)
        {
            var observable = new ObservableDecimalAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableDecimalAverage CreateExpression(IEnumerableExpression<decimal> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableNullableIntAverage Create(INotifyEnumerable<int?> source)
        {
            var observable = new ObservableNullableIntAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableNullableIntAverage CreateExpression(IEnumerableExpression<int?> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableNullableLongAverage Create(INotifyEnumerable<long?> source)
        {
            var observable = new ObservableNullableLongAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableNullableLongAverage CreateExpression(IEnumerableExpression<long?> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableNullableFloatAverage Create(INotifyEnumerable<float?> source)
        {
            var observable = new ObservableNullableFloatAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableNullableFloatAverage CreateExpression(IEnumerableExpression<float?> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableNullableDoubleAverage Create(INotifyEnumerable<double?> source)
        {
            var observable = new ObservableNullableDoubleAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableNullableDoubleAverage CreateExpression(IEnumerableExpression<double?> source)
        {
            return Create(source.AsNotifiable());
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
        public override string ToString()
        {
            return "[Average]";
        }

        public static ObservableNullableDecimalAverage Create(INotifyEnumerable<decimal?> source)
        {
            var observable = new ObservableNullableDecimalAverage(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableNullableDecimalAverage CreateExpression(IEnumerableExpression<decimal?> source)
        {
            return Create(source.AsNotifiable());
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
