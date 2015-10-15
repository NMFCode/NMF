using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    public interface ITransformationEngineContext : ITransformationContext
    {
        void ExecutePending();
    }
}
