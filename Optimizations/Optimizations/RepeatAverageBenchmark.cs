using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    public class RepeatAverageBenchmark<T> : IBenchmark<T>
    {
        public IBenchmark<T> Inner { get; set; }

        public int N { get; set; }

        public RepeatAverageBenchmark(IBenchmark<T> inner, int n = 5)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (n <= 0) throw new ArgumentOutOfRangeException("n", "Amount of repeated measurements must be positive");

            Inner = inner;
            N = n;
        }


        public IDictionary<string, double> MeasureConfiguration(T configuration)
        {
            var initial = Inner.MeasureConfiguration(configuration);
            if (initial == null) return null;
            Dictionary<string, double> dict = new Dictionary<string, double>(initial);
            for (int i = 0; i < N - 1; i++)
            {
                var newMeasurements = Inner.MeasureConfiguration(configuration);
                if (newMeasurements == null) return null;
                foreach (var measurement in newMeasurements)
                {
                    dict[measurement.Key] += measurement.Value;
                }
            }
            foreach (var key in dict.Keys.ToArray())
            {
                dict[key] /= N;
            }
            return dict;
        }
    }
}
