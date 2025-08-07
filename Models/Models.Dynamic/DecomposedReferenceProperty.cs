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
            _inner.Add(referenceProperty);
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

        public bool Contains(IModelElement element)
        {
            return element == ReferencedElement.Value;
        }

        private readonly List<IReferenceProperty> _inner = new List<IReferenceProperty>();

        public void AddConstraint(IEnumerable<IModelElement> values)
        {
            Coalesce(new ReversibleConstant<IModelElement>(values.FirstOrDefault()));
        }

        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement { get; private set; }

        public bool IsContainment => false;

        public int Count => ReferencedElement.Value == null ? 0 : 1;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        public bool TryAdd(IModelElement element)
        {
            foreach (var inner in _inner)
            {
                if (inner.TryAdd(element))
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            foreach(var inner in _inner)
            {
                if (inner.TryRemove(element))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            foreach (var inner in _inner)
            {
                inner.Reset();
            }
        }
    }
}
