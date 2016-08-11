using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public interface ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        bool IsEarly { get; }

        IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context);
    }
}
