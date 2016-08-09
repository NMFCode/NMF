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
            Dictionary<string, double> dict = new Dictionary<string, double>(Inner.MeasureConfiguration(configuration));
            for (int i = 0; i < N - 1; i++)
            {
                var newMeasurements = Inner.MeasureConfiguration(configuration);
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
