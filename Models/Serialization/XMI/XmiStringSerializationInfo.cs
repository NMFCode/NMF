using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Serialization.Xmi
{
    internal class XmiStringSerializationInfo : ITypeSerializationInfo
    {
        private XmiStringSerializationInfo() { }
        private static readonly XmiStringSerializationInfo instance = new XmiStringSerializationInfo();

        public static XmiStringSerializationInfo Instance { get { return instance; } }

        public IEnumerable<IPropertySerializationInfo> AttributeProperties
        {
            get { return Enumerable.Empty<XmlPropertySerializationInfo>(); }
        }

        public IEnumerable<IPropertySerializationInfo> ElementProperties
        {
            get { return Enumerable.Empty<XmlPropertySerializationInfo>(); }
        }

        public Type MappedType
        {
            get { return typeof(string); }
        }

        public IPropertySerializationInfo[] ConstructorProperties
        {
            get { return null; }
        }

        public bool IsIdentified
        {
            get { return false; }
        }

        public IPropertySerializationInfo IdentifierProperty
        {
            get { return null; }
        }

        public void Reset()
        {
        }

        public bool IsCollection
        {
            get { return false; }
        }

        public ITypeSerializationInfo CollectionItemType
        {
            get { return null; }
        }

        public void AddToCollection(object collection, object item)
        {
            throw new InvalidOperationException();
        }

        public string ElementName
        {
            get { return null; }
        }

        public string Namespace
        {
            get { return null; }
        }

        public string NamespacePrefix
        {
            get { return null; }
        }

        public bool IsStringConvertible
        {
            get { return true; }
        }

        public object ConvertFromString(string text)
        {
            return text;
        }

        public string ConvertToString(object input)
        {
            return input.ToString();
        }

        public object CreateObject(object[] args)
        {
            throw new NotSupportedException();
        }

        public bool IsAssignableFrom(ITypeSerializationInfo specializedType)
        {
            return specializedType is XmiStringSerializationInfo;
        }

        public bool IsInstanceOf(object instance)
        {
            return instance is string;
        }

        public IEnumerable<ITypeSerializationInfo> BaseTypes
        {
            get { return Enumerable.Empty<ITypeSerializationInfo>(); }
        }

        public IPropertySerializationInfo DefaultProperty
        {
            get { return null; }
        }
    }
}
