using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    /// <summary>
    /// Denotes a pareto filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParetoFilter<T> : IMeasurementFilter<T>
    {
        /// <summary>
        /// Gets the dimensions of the pareto filter
        /// </summary>
        public IDictionary<string, DimensionRating> ParetoDimensions { get; private set; }

        /// <summary>
        /// Creates a pareto filter with the given dimension ratings
        /// </summary>
        /// <param name="dimensions"></param>
        public ParetoFilter(IDictionary<string, DimensionRating> dimensions)
        {
            ParetoDimensions = dimensions;
        }

        /// <summary>
        /// Filters the given measurements
        /// </summary>
        /// <param name="measurements">The measurements to filter</param>
        /// <returns>A filtered collection of measurements</returns>
        public IEnumerable<MeasuredConfiguration<T>> Filter(IEnumerable<MeasuredConfiguration<T>> measurements)
        {
            return measurements.Where(m => measurements.All(m2 =>
            {
                foreach (var dimension in ParetoDimensions)
                {
                    if (!dimension.Value.IsBetter(m2.Measurements[dimension.Key], m.Measurements[dimension.Key]))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
    }
}
