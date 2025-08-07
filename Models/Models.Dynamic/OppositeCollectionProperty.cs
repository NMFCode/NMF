using System.Collections;
using NMF.Expressions;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class OppositeCollectionProperty : IReferenceProperty
    {
        public OppositeCollectionProperty(IModelElement parent, IReference reference)
        {
            if (reference.IsUnique)
            {
                if (reference.IsOrdered)
                {
                    Collection = new DynamicOppositeOrderedSet(parent, reference);
                }
                else
                {
                    Collection = new DynamicOppositeSet(parent, reference);
                }
            }
            else
            {
                Collection = new DynamicOppositeList(parent, reference);
            }
            IsContainment = reference.IsContainment;
        }

        public IModelElementCollection Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment { get; }

        public int Count => Collection.Count;

        IList IReferenceProperty.Collection => Collection;

        public bool Contains(IModelElement element)
        {
            return element == ReferencedElement.Value;
        }

        public object GetValue(int index)
        {
            return Collection[index];
        }

        public void Reset()
        {
            Collection.Clear();
        }

        public bool TryAdd(IModelElement element)
        {
            return Collection.TryAdd(element);
        }

        public bool TryRemove(IModelElement element)
        {
            return Collection.TryRemove(element);
        }
    }
}
