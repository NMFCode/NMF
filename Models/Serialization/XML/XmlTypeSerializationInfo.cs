using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;

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
        private Action<object, object> addMethod;

        public XmlTypeSerializationInfo(Type type)
        {
            this.type = type;
            this.converter = TypeDescriptor.GetConverter(type);
        }

        public List<IPropertySerializationInfo> DeclaredAttributeProperties
        {
            get
            {
                return attributeProperties;
            }
        }

        public List<IPropertySerializationInfo> DeclaredElementProperties
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
                if (addMethod == null)
                {
                    CreateCollectionAddMethod();
                }
                addMethod(collection, item);
            }
        }

        public IEnumerable<IPropertySerializationInfo> AttributeProperties
        {
            get
            {
                var stack = new Stack<ITypeSerializationInfo>();
                ITypeSerializationInfo current = this;
                XmlTypeSerializationInfo currentType = this;
                while (current != null)
                {
                    if (currentType != null)
                    {
                        foreach (var att in currentType.DeclaredAttributeProperties)
                        {
                            yield return att;
                        }
                        foreach (var bt in currentType.BaseTypes)
                        {
                            stack.Push(bt);
                        }
                    }
                    else
                    {
                        foreach (var att in current.AttributeProperties)
                        {
                            yield return att;
                        }
                    }
                    current = stack.Count > 0 ? stack.Pop() : null;
                    currentType = current as XmlTypeSerializationInfo;
                }
            }
        }

        public IEnumerable<IPropertySerializationInfo> ElementProperties
        {
            get
            {
                var stack = new Stack<ITypeSerializationInfo>();
                ITypeSerializationInfo current = this;
                XmlTypeSerializationInfo currentType = this;
                while (current != null)
                {
                    if (currentType != null)
                    {
                        foreach (var att in currentType.DeclaredElementProperties)
                        {
                            yield return att;
                        }
                        foreach (var bt in currentType.BaseTypes)
                        {
                            stack.Push(bt);
                        }
                    }
                    else
                    {
                        foreach (var att in current.ElementProperties)
                        {
                            yield return att;
                        }
                    }
                    current = stack.Count > 0 ? stack.Pop() : null;
                    currentType = current as XmlTypeSerializationInfo;
                }
            }
        }


        public ITypeSerializationInfo CollectionItemType
        {
            get; 
            set;
        }

        public Type CollectionItemRawType { get; set; }


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

        public void CreateCollectionAddMethod()
        {
            var itemType = CollectionItemType.Type;
            var collectionAddMethod = CollectionType.GetMethod("Add", new Type[] { itemType });
            if (collectionAddMethod == null) throw new Exception($"Could not find a suitable add method in the type {CollectionType}");
            var parameters = collectionAddMethod.GetParameters();
            if (parameters == null || parameters.Length != 1) throw new Exception($"The add method of type {CollectionType} has the wrong amount of arguments");
            itemType = parameters[0].ParameterType;
            var p = Expression.Parameter(typeof(object));
            var coll = Expression.Parameter(typeof(object));
            var body = Expression.Call(Expression.Convert(coll, CollectionType), collectionAddMethod, Expression.Convert(p, itemType));
            addMethod = Expression.Lambda<Action<object, object>>(body, coll, p).Compile();
        }
    }
}
