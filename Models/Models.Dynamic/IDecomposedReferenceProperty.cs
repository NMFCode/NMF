using System.Collections.Generic;

namespace NMF.Models.Dynamic
{
    internal interface IDecomposedReferenceProperty
    {
        void AddComponentReferenceProperty(IReferenceProperty referenceProperty);
        void AddConstraint(IEnumerable<IModelElement> value);
    }
}