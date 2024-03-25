using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Expressions;
using NMF.Models.Collections;
using NMF.Models.Meta;
using Type = System.Type;

namespace NMF.Models.Dynamic
{
    internal class AssociationCollectionProperty : IReferenceProperty
    {
        public AssociationCollectionProperty(IReference reference)
        {
            var mappedType = reference.Type.GetExtension<MappedType>();
            if (mappedType != null)
            {
                Type collectionType = GetCollectionTypeForReference(reference);
                Collection = Activator.CreateInstance(collectionType.MakeGenericType(mappedType.SystemType)) as IList;
            }
            else
            {
                if (reference.IsUnique)
                {
                    if (reference.IsOrdered)
                    {
                        Collection = new DynamicAssociationOrderedSet((IClass)reference.ReferenceType);
                    }
                    else
                    {
                        Collection = new DynamicAssociationSet((IClass)reference.ReferenceType);
                    }
                }
                else
                {
                    Collection = new DynamicAssociationList((IClass)reference.ReferenceType);
                }
            }
        }

        private static Type GetCollectionTypeForReference(IReference reference)
        {
            Type collectionType;
            if (reference.IsUnique)
            {
                if (reference.IsOrdered)
                {
                    collectionType = typeof(AssociationOrderedSet<>);
                }
                else
                {
                    collectionType = typeof(AssociationSet<>);
                }
            }
            else
            {
                collectionType = typeof(AssociationList<>);
            }

            return collectionType;
        }

        public IList Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment => false;

        public object GetValue(int index)
        {
            return Collection[index];
        }
    }
}
