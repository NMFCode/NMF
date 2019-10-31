using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NMF.Serialization
{
    public interface ITypeSerializationInfo
    {
        string ElementName { get; }
        string Namespace { get; }
        string NamespacePrefix { get; }

        Type MappedType { get; }

        IPropertySerializationInfo DefaultProperty { get; }

        IEnumerable<IPropertySerializationInfo> AttributeProperties { get; }
        IEnumerable<IPropertySerializationInfo> ElementProperties { get; }

        IEnumerable<ITypeSerializationInfo> BaseTypes { get; }

        bool IsAssignableFrom(ITypeSerializationInfo specializedType);

        bool IsInstanceOf(object instance);

        IPropertySerializationInfo[] ConstructorProperties { get; }

        object CreateObject(object[] args);

        bool IsIdentified { get; }

        IPropertySerializationInfo IdentifierProperty { get; }

        bool IsCollection { get; }

        ITypeSerializationInfo CollectionItemType { get; }

        void AddToCollection(object collection, object item);

        bool IsStringConvertible { get; }
        object ConvertFromString(string text);
        string ConvertToString(object input);
    }
}
