using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;
using NMF.Serialization;

namespace NMF.Models.Dynamic.Serialization
{
    internal sealed class CollectionTypeSerializationInfo : ITypeSerializationInfo
    {
        public CollectionTypeSerializationInfo(ITypeSerializationInfo type)
        {
            CollectionItemType = type;
        }

        public string ElementName => null;

        public string Namespace => null;

        public string NamespacePrefix => null;

        public Type MappedType => typeof(IEnumerableExpression<IModelElement>);

        public IPropertySerializationInfo DefaultProperty => null;

        public IEnumerable<IPropertySerializationInfo> AttributeProperties => Enumerable.Empty<IPropertySerializationInfo>();

        public IEnumerable<IPropertySerializationInfo> ElementProperties => Enumerable.Empty<IPropertySerializationInfo>();

        public IEnumerable<ITypeSerializationInfo> BaseTypes => Enumerable.Empty<ITypeSerializationInfo>();

        public IPropertySerializationInfo[] ConstructorProperties => null;

        public bool IsIdentified => false;

        public IPropertySerializationInfo IdentifierProperty => null;

        public bool IsCollection => true;

        public ITypeSerializationInfo CollectionItemType { get; }

        public bool IsStringConvertible => false;

        public void AddToCollection(object collection, object item)
        {
            var list = collection as IList;
            list.Add(item);
        }

        public object ConvertFromString(string text)
        {
            throw new NotSupportedException();
        }

        public string ConvertToString(object input)
        {
            throw new NotSupportedException();
        }

        public object CreateObject(object[] args)
        {
            throw new NotSupportedException();
        }

        public bool IsAssignableFrom(ITypeSerializationInfo specializedType)
        {
            throw new NotSupportedException();
        }

        public bool IsExplicitTypeInformationRequired(ITypeSerializationInfo itemType)
        {
            return false;
        }

        public bool IsInstanceOf(object instance)
        {
            throw new NotSupportedException();
        }
    }
}
