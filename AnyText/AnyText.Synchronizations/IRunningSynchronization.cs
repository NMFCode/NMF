using NMF.Models;
using NMF.Synchronizations.Inconsistencies;
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

        /// <summary>
        /// Determines whether the given document is the left document
        /// </summary>
        /// <param name="document">the document</param>
        /// <returns>true, if it is the left document, otherwise false</returns>
        bool IsLeft(Uri document);

        /// <summary>
        /// Gets the inconsistencies from this synchronization
        /// </summary>
        ICollection<IInconsistency> Inconsistencies { get; }
    }
}
