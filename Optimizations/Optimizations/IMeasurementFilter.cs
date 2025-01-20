using System.Collections.Generic;

namespace NMF.Optimizations
{
    /// <summary>
    /// Denotes a component that filters measurement results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMeasurementFilter<T>
    {
        /// <summary>
        /// Filters the given measurement results
        /// </summary>
        /// <param name="measurements">The measurement results</param>
        /// <returns>A collection with filtered measurement results</returns>
        IEnumerable<MeasuredConfiguration<T>> Filter(IEnumerable<MeasuredConfiguration<T>> measurements);
    }
}
