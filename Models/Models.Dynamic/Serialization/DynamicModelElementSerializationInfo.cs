using System;
using System.Collections.Generic;
using System.Linq;
using NMF.Models.Meta;
using NMF.Serialization;
using Type = System.Type;

namespace NMF.Models.Dynamic
{
    internal class DynamicModelElementSerializationInfo : ITypeSerializationInfo
    {
        public DynamicModelElementSerializationInfo(IClass @class)
        {
            Class = @class;
        }

        public string ElementName => Class.Name;

        public string Namespace => Class.Namespace.Uri.AbsoluteUri;

        public string NamespacePrefix => Class.Namespace.Prefix;

        public Type MappedType => typeof(DynamicModelElement);

        public IPropertySerializationInfo DefaultProperty => null;

        public IEnumerable<IPropertySerializationInfo> DeclaredAttributeProperties { get; internal set; }

        public IEnumerable<IPropertySerializationInfo> DeclaredElementProperties { get; internal set; }

        public IEnumerable<DynamicModelElementSerializationInfo> BaseTypes { get; internal set; }

        public IPropertySerializationInfo[] ConstructorProperties => null;

        public bool IsIdentified => IdentifierProperty != null;

        public IPropertySerializationInfo IdentifierProperty { get; }

        public bool IsCollection => false;

        public ITypeSerializationInfo CollectionItemType => null;

        public bool IsStringConvertible => false;

        public IClass Class { get; }

        public IEnumerable<IPropertySerializationInfo> AttributeProperties => DeclaredAttributeProperties.Concat(BaseTypes.SelectMany(type => type.DeclaredAttributeProperties));

        public IEnumerable<IPropertySerializationInfo> ElementProperties => DeclaredElementProperties.Concat(BaseTypes.SelectMany(type => type.DeclaredElementProperties));

        public void AddToCollection(object collection, object item)
        {
            throw new NotSupportedException();
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
            return new DynamicModelElement(Class);
        }

        public bool IsAssignableFrom(ITypeSerializationInfo specializedType)
        {
            if (specializedType is DynamicModelElementSerializationInfo dynamicInfo)
            {
                return Class.IsAssignableFrom(dynamicInfo.Class);
            }
            return false;
        }

        public bool IsExplicitTypeInformationRequired(ITypeSerializationInfo itemType)
        {
            if (itemType is DynamicModelElementSerializationInfo dynamicInfo)
            {
                return Class != dynamicInfo.Class;
            }
            return false;
        }

        public bool IsInstanceOf(object instance)
        {
            return instance is DynamicModelElement;
        }
    }
}
