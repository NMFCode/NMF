using System;
using System.Collections;
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
                Type collectionType = GetCollectionTypeForReference(reference);
                Collection = Activator.CreateInstance(collectionType.MakeGenericType(mappedType.SystemType), parent) as IModelElementCollection;
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

        private static Type GetCollectionTypeForReference(IReference reference)
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

            return collectionType;
        }

        public IModelElementCollection Collection { get; }

        public INotifyReversableExpression<IModelElement> ReferencedElement => null;

        public bool IsContainment => true;

        IList IReferenceProperty.Collection => Collection;

        public int Count => Collection.Count;

        public object GetValue(int index)
        {
            return Collection[index];
        }

        public bool Contains(IModelElement element)
        {
            return Collection.Contains(element);
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

        private sealed class CompositionOrderedSet<T> : ObservableCompositionOrderedSet<T>, IModelElementCollection where T : class, IModelElement
        {
            public CompositionOrderedSet(ModelElement parent) : base(parent)
            {
            }

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
                return element is T casted && Remove(casted);
            }
        }

        private sealed class CompositionSet<T> : ObservableCompositionSet<T>, IModelElementCollection where T : class, IModelElement
        {
            public CompositionSet(IModelElement parent) : base(parent)
            {
            }

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
                return element is T casted && Remove(casted);
            }
        }

        private sealed class CompositionList<T> : ObservableCompositionList<T>, IModelElementCollection where T : class, IModelElement
        {
            public CompositionList(ModelElement parent) : base(parent)
            {
            }

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
                return element is T casted && Remove(casted);
            }
        }
    }
}
