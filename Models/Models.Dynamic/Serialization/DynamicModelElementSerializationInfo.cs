using System;
using System.Collections.Generic;
using System.Text;
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

        public IEnumerable<IPropertySerializationInfo> AttributeProperties { get; internal set; }

        public IEnumerable<IPropertySerializationInfo> ElementProperties { get; internal set; }

        public IEnumerable<ITypeSerializationInfo> BaseTypes { get; internal set; }

        public IPropertySerializationInfo[] ConstructorProperties => null;

        public bool IsIdentified => IdentifierProperty != null;

        public IPropertySerializationInfo IdentifierProperty { get; }

        public bool IsCollection => false;

        public ITypeSerializationInfo CollectionItemType => null;

        public bool IsStringConvertible => false;

        public IClass Class { get; }

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

        public bool IsInstanceOf(object instance)
        {
            return instance is DynamicModelElement;
        }
    }
}
