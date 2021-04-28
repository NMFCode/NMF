using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes the serialization information for a property
    /// </summary>
    public interface IPropertySerializationInfo
    {
        /// <summary>
        /// The element name that should be serialized
        /// </summary>
        string ElementName { get; }

        /// <summary>
        /// The namespace to which the element should be serialized or null
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// The namespace prefix that should be used for serialization or null
        /// </summary>
        string NamespacePrefix { get; }

        /// <summary>
        /// Adds the given object to the collection
        /// </summary>
        /// <param name="input">The context object</param>
        /// <param name="item">The item that should be added</param>
        /// <param name="context">The deserialization context</param>
        void AddToCollection(object input, object item, XmlSerializationContext context);

        /// <summary>
        /// Determines whether the instance should be created explicitly
        /// </summary>
        bool ShallCreateInstance { get; }

        /// <summary>
        /// Determines whether the provided value should be serialized
        /// </summary>
        /// <param name="obj">The context object</param>
        /// <param name="value">The value in question</param>
        /// <returns>True, if the value should be serialized, otherwise False</returns>
        bool ShouldSerializeValue(object obj, object value);

        /// <summary>
        /// True, if the property is read-only, otherwise False
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the value of this property for the given input object
        /// </summary>
        /// <param name="input">The context object</param>
        /// <param name="context">The serialization context</param>
        /// <returns></returns>
        object GetValue(object input, XmlSerializationContext context);

        /// <summary>
        /// Sets the value for the property
        /// </summary>
        /// <param name="input">The context object</param>
        /// <param name="value">The value of the property</param>
        /// <param name="context">The deserialization context</param>
        void SetValue(object input, object value, XmlSerializationContext context);

        /// <summary>
        /// True, if the property is an identifier, otherwise False
        /// </summary>
        bool IsIdentifier { get; }

        /// <summary>
        /// Gets the identification mode of the property
        /// </summary>
        XmlIdentificationMode IdentificationMode { get; }

        /// <summary>
        /// Gets the property type
        /// </summary>
        ITypeSerializationInfo PropertyType { get; }

        /// <summary>
        /// Gets the minimum type system type of the property
        /// </summary>
        Type PropertyMinType { get; }

        /// <summary>
        /// Gets the opposite property or null
        /// </summary>
        IPropertySerializationInfo Opposite { get; }

        /// <summary>
        /// True, if the value of this property allows roundtrip-serialization to string
        /// </summary>
        bool IsStringConvertible { get; }

        /// <summary>
        /// Deserializes the provided text
        /// </summary>
        /// <param name="text">the text</param>
        /// <returns>The deserialized value</returns>
        object ConvertFromString(string text);

        /// <summary>
        /// Converts the provided object to a string
        /// </summary>
        /// <param name="input">The object to convert</param>
        /// <returns>A string representation</returns>
        string ConvertToString(object input);
    }
}
