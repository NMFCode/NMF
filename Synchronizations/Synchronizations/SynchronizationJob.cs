using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public interface ISynchronizationJob<TLeft, TRight>
    {
        bool IsEarly { get; }

        void Perform(TLeft left, TRight right, SynchronizationDirection direction, ISynchronizationContext context);
    }
}
