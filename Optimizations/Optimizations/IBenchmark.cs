using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    public interface IBenchmark<T>
    {
        IDictionary<string, double> MeasureConfiguration(T configuration);
    }
}
