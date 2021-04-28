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
    /// <summary>
    /// Denotes the default type serialization info read through reflection
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("SerializationInfo ({Type})")]
    public class XmlTypeSerializationInfo : ITypeSerializationInfo
    {
        private readonly Type type;
        private readonly List<IPropertySerializationInfo> attributeProperties = new List<IPropertySerializationInfo>();
        private readonly List<IPropertySerializationInfo> elementProperties = new List<IPropertySerializationInfo>();
        private readonly List<ITypeSerializationInfo> baseTypes = new List<ITypeSerializationInfo>();
        private IPropertySerializationInfo identifierProperty;
        private readonly TypeConverter converter;
        private Action<object, object> addMethod;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type">The type that is read</param>
        public XmlTypeSerializationInfo(Type type)
        {
            this.type = type;
            this.converter = TypeDescriptor.GetConverter(type);
        }

        /// <inheritdoc />
        public List<IPropertySerializationInfo> DeclaredAttributeProperties
        {
            get
            {
                return attributeProperties;
            }
        }

        /// <inheritdoc />
        public List<IPropertySerializationInfo> DeclaredElementProperties
        {
            get
            {
                return elementProperties;
            }
        }

        /// <inheritdoc />
        public List<ITypeSerializationInfo> BaseTypes
        {
            get
            {
                return baseTypes;
            }
        }

        /// <inheritdoc />
        public Type Type
        {
            get
            {
                return type;
            }
        }

        /// <inheritdoc />
        public IPropertySerializationInfo[] ConstructorProperties
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int ConstructorParameterCount
        {
            get
            {
                return ConstructorProperties == null ? 0 : ConstructorProperties.GetLength(0);
            }
        }

        /// <inheritdoc />
        public bool HasConstructorParameters
        {
            get
            {
                return ConstructorParameterCount > 0;
            }
        }

        /// <inheritdoc />
        public ConstructorInfo Constructor
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string ElementName
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Type CollectionType
        {
            get;
            set;
        }

        /// <inheritdoc />
        public bool IsCollection
        {
            get
            {
                return CollectionType != null;
            }
        }

        /// <inheritdoc />
        public bool IsStaticCollection
        {
            get
            {
                return CollectionType == typeof(IList);
            }
        }

        /// <inheritdoc />
        public string Namespace
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string NamespacePrefix
        {
            get;
            set;
        }

        /// <inheritdoc />
        public bool IsIdentified
        {
            get { return identifierProperty != null; }
        }

        /// <inheritdoc />
        public IPropertySerializationInfo IdentifierProperty
        {
            get { return identifierProperty; }
            set
            {
                this.identifierProperty = value;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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


        /// <inheritdoc />
        public ITypeSerializationInfo CollectionItemType
        {
            get; 
            set;
        }

        /// <inheritdoc />
        public Type CollectionItemRawType { get; set; }


        /// <inheritdoc />
        public bool IsStringConvertible
        {
            get { return converter != null && converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(typeof(string)); }
        }

        /// <inheritdoc />
        public object ConvertFromString(string text)
        {
            return converter.ConvertFromInvariantString(text);
        }

        /// <inheritdoc />
        public string ConvertToString(object input)
        {
            return converter.ConvertToInvariantString(input);
        }

        /// <inheritdoc />
        public IPropertySerializationInfo DefaultProperty { get; set; }

        /// <inheritdoc />
        public Type MappedType => Type;

        /// <inheritdoc />
        public void CreateCollectionAddMethod()
        {
            var itemType = CollectionItemType.MappedType;
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

        /// <inheritdoc />
        public object CreateObject(object[] args)
        {
            return Constructor.Invoke(args);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Type.FullName;
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSerializationInfo specializedType)
        {
            return specializedType is XmlTypeSerializationInfo typeInfo && Type.IsAssignableFrom(typeInfo.Type);
        }

        /// <inheritdoc />
        public bool IsInstanceOf(object instance)
        {
            return Type.IsInstanceOfType(instance);
        }
    }
}
