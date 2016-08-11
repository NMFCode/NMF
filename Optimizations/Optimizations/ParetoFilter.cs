using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    public class ParetoFilter<T> : IMeasurementFilter<T>
    {
        public IDictionary<string, DimensionRating> ParetoDimensions { get; private set; }

        public ParetoFilter(IDictionary<string, DimensionRating> dimensions)
        {
            ParetoDimensions = dimensions;
        }

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
