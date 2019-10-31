using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models.Collections;
using NMF.Models.Meta;
using Type = System.Type;

namespace NMF.Models.Dynamic
{
    internal class ContainmentCollectionProperty : IReferenceProperty
    {
        public ContainmentCollectionProperty(ModelElement parent, IReference reference)
        {
            var mappedType = (reference.Type ?? ModelElement.ClassInstance).GetExtension<MappedType>();
            if (mappedType != null)
            {
                Type collectionType;
                if (reference.IsUnique)
                {
                    if (reference.IsOrdered)
                    {
                        collectionType = typeof(CompositionOrderedSet<>);
                    }
                    else
                    {
                        collectionType = typeof(CompositionSet<>);
                    }
                }
                else
                {
                    collectionType = typeof(CompositionList<>);
                }
                Collection = Activator.CreateInstance(collectionType.MakeGenericType(mappedType.SystemType), parent) as IList;
            }
            else
            {
                if (reference.IsUnique)
                {
                    if (reference.IsOrdered)
                    {
                        Collection = new DynamicCompositionOrderedSet(parent, (IClass)reference.ReferenceType);
                    }
                    else
                    {
                        Collection = new DynamicCompositionSet(parent, (IClass)reference.ReferenceType);
                    }
                }
                else
                {
                    Collection = new DynamicCompositionList(parent, (IClass)reference.ReferenceType);
                }
            }
        }

        public IList Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment => true;

        public object GetValue(int index)
        {
            return Collection[index];
        }
    }
}
