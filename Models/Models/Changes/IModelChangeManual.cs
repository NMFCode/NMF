using System.Collections.Generic;

namespace NMF.Models.Changes
{
    public partial interface IModelChange
    {
        /// <summary>
        /// Applies the change
        /// </summary>
        void Apply();

        /// <summary>
        /// Inverts the change
        /// </summary>
        /// <returns>A collection of changes that represent the inversion of the change</returns>
        IEnumerable<IModelChange> Invert();
    }
}
