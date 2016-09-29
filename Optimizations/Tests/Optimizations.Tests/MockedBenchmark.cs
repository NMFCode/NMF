using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Optimizations.Tests
{
    class MockedBenchmark : IBenchmark<string>
    {
        private int counter;
        private Dictionary<string, Func<int, double>> metrics;

        public IEnumerable<string> Metrics
        {
            get
            {
                yield return "Time";
                yield return "Memory";
                yield return "Throughput";
            }
        }

        public MockedBenchmark(Func<int, double> time = null, Func<int, double> memory = null, Func<int, double> throughput = null)
        {
            metrics = new Dictionary<string, Func<int, double>>();
            if (time != null) metrics.Add("Time", time);
            if (memory != null) metrics.Add("Memory", memory);
            if (throughput != null) metrics.Add("Throughput", throughput);
        }

        public MockedBenchmark(Dictionary<string, Func<int, double>> metrics)
        {
            this.metrics = metrics ?? new Dictionary<string, Func<int, double>>();
        }

        public IDictionary<string, double> MeasureConfiguration(string configuration)
        {
            var results = new Dictionary<string, double>();
            foreach (var m in metrics)
            {
                results.Add(m.Key, m.Value(counter));
            }
            counter++;
            return results;
        }

        public IEnumerable<MeasuredConfiguration<string>> CreateMeasurements(int n)
        {
            var measurements = new List<MeasuredConfiguration<string>>();
            counter = 0;
            for (int i = 0; i < n; i++)
            {
                var name = "Measurement" + counter.ToString();
                measurements.Add(new MeasuredConfiguration<string>(name, MeasureConfiguration(name)));
            }
            return measurements;
        }
    }
}
