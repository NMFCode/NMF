using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    public interface IMeasurementFilter<T>
    {
        IEnumerable<MeasuredConfiguration<T>> Filter(IEnumerable<MeasuredConfiguration<T>> measurements);
    }
}
