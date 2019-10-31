using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models.Meta;
using Type = System.Type;

namespace NMF.Models.Dynamic
{
    internal class AttributeCollectionProperty : IAttributeProperty
    {
        public AttributeCollectionProperty(IAttribute attribute)
        {
            Type collectionType;
            if (attribute.IsUnique)
            {
                if (attribute.IsOrdered)
                {
                    collectionType = typeof(ObservableOrderedSet<>);
                }
                else
                {
                    collectionType = typeof(ObservableSet<>);
                }
            }
            else
            {
                collectionType = typeof(ObservableList<>);
            }
            var mappedType = attribute.Type.GetExtension<MappedType>();
            Collection = Activator.CreateInstance(collectionType.MakeGenericType(mappedType.SystemType)) as IList;
        }

        public IList Collection { get; }

        public INotifyReversableExpression<object> Value => null;

        public object GetValue(int index)
        {
            return Collection[index];
        }
    }
}
