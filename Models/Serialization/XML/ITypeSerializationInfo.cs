using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes abstract information necessary for serialization
    /// </summary>
    public interface ITypeSerializationInfo
    {
        /// <summary>
        /// Gets the element name of the type
        /// </summary>
        string ElementName { get; }

        /// <summary>
        /// Gets the namespace of the type or null
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Gets the namespace prefix of the type or null
        /// </summary>
        string NamespacePrefix { get; }

        /// <summary>
        /// Gets the system type this type serialization is mapped to
        /// </summary>
        Type MappedType { get; }

        /// <summary>
        /// Gets the default property of this type
        /// </summary>
        IPropertySerializationInfo DefaultProperty { get; }

        /// <summary>
        /// Gets a collection of properties serialized as attributes
        /// </summary>
        IEnumerable<IPropertySerializationInfo> AttributeProperties { get; }

        /// <summary>
        /// Gets a collection of properties serialized as elements
        /// </summary>
        IEnumerable<IPropertySerializationInfo> ElementProperties { get; }

        /// <summary>
        /// Determines whether an instance of the given more concrete type can be assigned to this type
        /// </summary>
        /// <param name="specializedType">the more concrete type</param>
        /// <returns>True, if the type is assignable, otherwise False</returns>
        bool IsAssignableFrom(ITypeSerializationInfo specializedType);

        /// <summary>
        /// Determines whether the given object is an instance of this type
        /// </summary>
        /// <param name="instance">the instance</param>
        /// <returns>True, if the object is an instance of the serialization type, otherwise False</returns>
        bool IsInstanceOf(object instance);

        /// <summary>
        /// Gets the properties required for constructor calls
        /// </summary>
        IPropertySerializationInfo[] ConstructorProperties { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="args">The constructor parameters</param>
        /// <returns></returns>
        object CreateObject(object[] args);

        /// <summary>
        /// True, if instances of this type are identified
        /// </summary>
        bool IsIdentified { get; }

        /// <summary>
        /// Gets the property used as identifier
        /// </summary>
        IPropertySerializationInfo IdentifierProperty { get; }

        /// <summary>
        /// True, if this type is a collection
        /// </summary>
        bool IsCollection { get; }

        /// <summary>
        /// Gets the element type of a collection
        /// </summary>
        ITypeSerializationInfo CollectionItemType { get; }

        /// <summary>
        /// Adds the given item to the collection
        /// </summary>
        /// <param name="collection">the collection</param>
        /// <param name="item">the item to add</param>
        void AddToCollection(object collection, object item);

        /// <summary>
        /// True, if the items can be converted to string
        /// </summary>
        bool IsStringConvertible { get; }

        /// <summary>
        /// Deserializes the given text into an object
        /// </summary>
        /// <param name="text">the textual representation</param>
        /// <returns>The deserialized object</returns>
        object ConvertFromString(string text);

        /// <summary>
        /// Serializes the given object into a string
        /// </summary>
        /// <param name="input">the object</param>
        /// <returns>a textual representation</returns>
        string ConvertToString(object input);
    }
}
