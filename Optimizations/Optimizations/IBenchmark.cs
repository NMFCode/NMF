using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    /// <summary>
    /// Denotes the interface for a benchmark
    /// </summary>
    /// <typeparam name="T">The type that is measured</typeparam>
    public interface IBenchmark<T>
    {
        /// <summary>
        /// Measures the given configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>A dictionary of measurement results</returns>
        IDictionary<string, double> MeasureConfiguration(T configuration);

        /// <summary>
        /// Gets a collection of metrics measured by this benchmark
        /// </summary>
        IEnumerable<string> Metrics { get; }
    }
}
