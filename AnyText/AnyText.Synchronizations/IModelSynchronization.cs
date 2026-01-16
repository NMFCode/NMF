using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a model synchronization
    /// </summary>
    public interface IModelSynchronization
    {
        /// <summary>
        /// Determines whether a document of the given Uri can be synchronized
        /// </summary>
        /// <param name="uri">the Uri to synchronize</param>
        /// <param name="synchronizedUri">the Uri that the given document can be synchronized with</param>
        /// <returns>true, if the document can be synchronized, otherwise false</returns>
        bool CanSynchronize(Uri uri, out Uri synchronizedUri);

        /// <summary>
        /// Starts synchronizing the given Uri
        /// </summary>
        /// <param name="uri1">the first Uri to synchronize</param>
        /// <param name="uri2">the second Uri to synchronize</param>
        /// <param name="root1">the root element of the first model</param>
        /// <param name="root2">the root element of the second model</param>
        IRunningSynchronization Synchronize(Uri uri1, ref IModelElement root1, Uri uri2, ref IModelElement root2);

        /// <summary>
        /// True, if the synchronization should be performed automatically, otherwise false
        /// </summary>
        bool IsAutomatic { get; }

        /// <summary>
        /// The unique identifier of the synchronization
        /// </summary>
        string Name { get; }
    }
}
