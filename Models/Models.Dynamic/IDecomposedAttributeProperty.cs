using System.Collections.Generic;

namespace NMF.Models.Dynamic
{
    internal interface IDecomposedAttributeProperty
    {
        void AddComponentAttributeProperty(IAttributeProperty attributeProperty);
        void AddConstraint(IEnumerable<object> value);
    }
}