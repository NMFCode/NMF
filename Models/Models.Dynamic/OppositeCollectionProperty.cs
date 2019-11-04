using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Expressions;
using NMF.Models.Meta;
using Type = System.Type;

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

        public IList Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment { get; }

        public object GetValue(int index)
        {
            return Collection[index];
        }
    }
}
