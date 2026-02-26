using System.Collections;
using System.Collections.Generic;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the update of a model element
    /// </summary>
    /// <param name="UpdatedElement">a semantic model element that was updated outside the textual representation</param>
    /// <param name="UpdatedFeatures">a collection of changed features or null to update the entire rule application</param>
    /// <param name="UpdateReferences">true, if references need to be updated, otherwise False</param>
    public record struct ModelUpdate(object UpdatedElement, IEnumerable<string> UpdatedFeatures, bool UpdateReferences)
    {
    }
}
