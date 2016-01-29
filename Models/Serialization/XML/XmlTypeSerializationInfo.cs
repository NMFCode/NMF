using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace NMF.Serialization
{
    [System.Diagnostics.DebuggerDisplay("SerializationInfo ({Type})")]
    public class XmlTypeSerializationInfo : ITypeSerializationInfo
    {
        private Type type;
        private List<IPropertySerializationInfo> attributeProperties = new List<IPropertySerializationInfo>();
        private List<IPropertySerializationInfo> elementProperties = new List<IPropertySerializationInfo>();
        private List<ITypeSerializationInfo> baseTypes = new List<ITypeSerializationInfo>();
        private IPropertySerializationInfo identifierProperty;
        private TypeConverter converter;

        public XmlTypeSerializationInfo(Type type)
        {
            this.type = type;
            this.converter = TypeDescriptor.GetConverter(type);
        }

        public List<IPropertySerializationInfo> AttributeProperties
        {
            get
            {
                return attributeProperties;
            }
        }

        public List<IPropertySerializationInfo> ElementProperties
        {
            get
            {
                return elementProperties;
            }
        }

        public List<ITypeSerializationInfo> BaseTypes
        {
            get
            {
                return baseTypes;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }
        }

        public IPropertySerializationInfo[] ConstructorProperties
        {
            get;
            set;
        }

        public int ConstructorParameterCount
        {
            get
            {
                return ConstructorProperties == null ? 0 : ConstructorProperties.GetLength(0);
            }
        }

        public bool HasConstructorParameters
        {
            get
            {
                return ConstructorParameterCount > 0;
            }
        }

        public ConstructorInfo Constructor
        {
            get;
            set;
        }

        public string ElementName
        {
            get;
            set;
        }

        public Type CollectionType
        {
            get;
            set;
        }

        public bool IsCollection
        {
            get
            {
                return CollectionType != null;
            }
        }

        public bool IsStaticCollection
        {
            get
            {
                return CollectionType == typeof(IList);
            }
        }

        public string Namespace
        {
            get;
            set;
        }

        public string NamespacePrefix
        {
            get;
            set;
        }

        public bool IsIdentified
        {
            get { return identifierProperty != null; }
        }

        public IPropertySerializationInfo IdentifierProperty
        {
            get { return identifierProperty; }
            set
            {
                this.identifierProperty = value;
            }
        }

        public void AddToCollection(object collection, object item)
        {
            if (item != null)
            {
                CollectionType.InvokeMember("Add", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance, null, collection, new object[] { item });
            }
        }

        IEnumerable<IPropertySerializationInfo> ITypeSerializationInfo.AttributeProperties
        {
            get { return AttributeProperties; }
        }

        IEnumerable<IPropertySerializationInfo> ITypeSerializationInfo.ElementProperties
        {
            get { return ElementProperties; }
        }


        public ITypeSerializationInfo CollectionItemType
        {
            get; 
            set;
        }


        public bool IsStringConvertible
        {
            get { return converter != null && converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(typeof(string)); }
        }

        public object ConvertFromString(string text)
        {
            return converter.ConvertFromInvariantString(text);
        }

        public string ConvertToString(object input)
        {
            return converter.ConvertToInvariantString(input);
        }


        IEnumerable<ITypeSerializationInfo> ITypeSerializationInfo.BaseTypes
        {
            get { return baseTypes; }
        }
    }
}
