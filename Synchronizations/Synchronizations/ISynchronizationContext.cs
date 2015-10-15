using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public interface ISynchronizationContext : ITransformationContext
    {
        SynchronizationDirection Direction { get; }

        ChangePropagationMode ChangePropagation { get; }
    }
}
