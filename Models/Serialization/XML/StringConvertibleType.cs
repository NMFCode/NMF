using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Serialization
{
    internal class StringConvertibleType : ITypeSerializationInfo
    {
        private TypeConverter converter;
        private Type sourceType;

        public StringConvertibleType(TypeConverter converter, Type sourceType)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            this.converter = converter;
            this.sourceType = sourceType;
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

        public IEnumerable<IPropertySerializationInfo> AttributeProperties
        {
            get { return Enumerable.Empty<IPropertySerializationInfo>(); }
        }

        public IEnumerable<IPropertySerializationInfo> ElementProperties
        {
            get { return Enumerable.Empty<IPropertySerializationInfo>(); }
        }

        public IEnumerable<ITypeSerializationInfo> BaseTypes
        {
            get { return Enumerable.Empty<ITypeSerializationInfo>(); }
        }

        public Type Type
        {
            get { return sourceType; }
        }

        public IPropertySerializationInfo[] ConstructorProperties
        {
            get { return null; }
        }

        public System.Reflection.ConstructorInfo Constructor
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

        public bool IsStringConvertible
        {
            get { return true; }
        }

        public object ConvertFromString(string text)
        {
            return converter.ConvertFromInvariantString(text);
        }

        public string ConvertToString(object input)
        {
            return converter.ConvertToInvariantString(input);
        }
    }
}
