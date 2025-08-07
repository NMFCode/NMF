using System;
using System.Collections;
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
                Collection = Activator.CreateInstance(collectionType.MakeGenericType(mappedType.SystemType)) as IModelElementCollection;
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
                    collectionType = typeof(AOrderedSet<>);
                }
                else
                {
                    collectionType = typeof(ASet<>);
                }
            }
            else
            {
                collectionType = typeof(AList<>);
            }

            return collectionType;
        }

        public IModelElementCollection Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment => false;

        public int Count => Collection.Count;

        IList IReferenceProperty.Collection => Collection;

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

        public bool Contains(IModelElement element)
        {
            return Collection.Contains(element);
        }

        private sealed class AOrderedSet<T> : AssociationOrderedSet<T>, IModelElementCollection where T : class, IModelElement
        {
            public bool TryAdd(IModelElement element)
            {
                if (element is T casted)
                {
                    return Add(casted);
                }
                return false;
            }

            public bool TryRemove(IModelElement element)
            {
                if (element is T casted)
                {
                    return Remove(casted);
                }
                return false;
            }
        }

        private sealed class ASet<T> : AssociationSet<T>, IModelElementCollection where T : class, IModelElement
        {
            public bool TryAdd(IModelElement element)
            {
                if (element is T casted)
                {
                    return Add(casted);
                }
                return false;
            }

            public bool TryRemove(IModelElement element)
            {
                if (element is T casted)
                {
                    return Remove(casted);
                }
                return false;
            }
        }

        private sealed class AList<T> : AssociationList<T>, IModelElementCollection where T : class, IModelElement
        {
            public bool TryAdd(IModelElement element)
            {
                if (element is T casted)
                {
                    Add(casted);
                    return true;
                }
                return false;
            }

            public bool TryRemove(IModelElement element)
            {
                if (element is T casted)
                {
                    return Remove(casted);
                }
                return false;
            }
        }
    }
}
