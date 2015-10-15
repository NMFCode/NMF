using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This interface is internally used to persist an output of transformation rules somehow
    /// </summary>
    public interface IPersistor
    {
        /// <summary>
        /// Persists the output of a computation
        /// </summary>
        /// <param name="output">The output of a computation</param>
        /// <remarks>This method is called by nmf transformations core</remarks>
        void Persist(object output);
    }
}
