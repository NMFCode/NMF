using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a running model synchronization
    /// </summary>
    public interface IRunningSynchronization : IDisposable
    {
        /// <summary>
        /// The underlying synchronization
        /// </summary>
        IModelSynchronization Synchronization { get; }

        /// <summary>
        /// Gets a collection of synchronized URIs
        /// </summary>
        IEnumerable<Uri> SynchronizedUris { get; }

        /// <summary>
        /// Gets a collection of synchronized models
        /// </summary>
        IEnumerable<IModelElement> SynchronizedModels { get; }
    }
}
