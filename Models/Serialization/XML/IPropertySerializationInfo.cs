using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NMF.Serialization
{
    public interface IPropertySerializationInfo
    {
        string ElementName { get; }
        string Namespace { get; }
        string NamespacePrefix { get; }

        bool ShallCreateInstance { get; }
        bool ShouldSerializeValue(object obj, object value);
        bool IsReadOnly { get; }
        object GetValue(object input, XmlSerializationContext context);
        void SetValue(object input, object value, XmlSerializationContext context);

        bool IsIdentifier { get; }

        XmlIdentificationMode IdentificationMode { get; }
        ITypeSerializationInfo PropertyType { get; }

        bool IsStringConvertible { get; }
        object ConvertFromString(string text);
        string ConvertToString(object input);
    }
}
