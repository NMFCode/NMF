using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace NMF.Models.Dynamic
{
    internal class DecomposedReferenceProperty : IReferenceProperty, IDecomposedReferenceProperty
    {
        public void AddComponentReferenceProperty(IReferenceProperty referenceProperty)
        {
            var coalesceValue = referenceProperty.ReferencedElement;
            if (referenceProperty.Collection is IEnumerableExpression<IModelElement> enumerableExpression)
            {
                coalesceValue = (INotifyReversableExpression<IModelElement>)Observable.Reversable(() => enumerableExpression.FirstOrDefault());
            }
            Coalesce(coalesceValue);
        }

        private void Coalesce(INotifyReversableExpression<IModelElement> coalesceValue)
        {
            if (ReferencedElement == null)
            {
                ReferencedElement = coalesceValue;
            }
            else
            {
                ReferencedElement = Observable.Coalesce(ReferencedElement, coalesceValue);
            }
        }

        public void AddConstraint(IEnumerable<IModelElement> values)
        {
            Coalesce(new ReversibleConstant<IModelElement>(values.FirstOrDefault()));
        }

        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement { get; private set; }

        public bool IsContainment => false;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }
    }
}
