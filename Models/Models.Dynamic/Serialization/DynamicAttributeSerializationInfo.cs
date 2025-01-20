using System;
using NMF.Models.Meta;
using NMF.Serialization;
using Type = System.Type;

namespace NMF.Models.Dynamic.Serialization
{
    internal class DynamicAttributeSerializationInfo : IPropertySerializationInfo
    {
        public DynamicAttributeSerializationInfo(IAttribute attribute, ITypeSerializationInfo type)
        {
            Attribute = attribute;
            PropertyType = type;
        }

        public string ElementName => Attribute.Name;

        public string Namespace => null;

        public string NamespacePrefix => null;

        public bool ShallCreateInstance => false;

        public bool IsReadOnly => false;

        public bool IsIdentifier => Attribute.DeclaringType is IClass cl && cl.Identifier == Attribute;

        public XmlIdentificationMode IdentificationMode => XmlIdentificationMode.AsNeeded;

        public ITypeSerializationInfo PropertyType { get; }

        public Type PropertyMinType => null;

        public IPropertySerializationInfo Opposite => null;

        public bool IsStringConvertible => true;

        public IAttribute Attribute { get; }

        public void AddToCollection(object input, object item, XmlSerializationContext context)
        {
            var modelElement = (IModelElement)input;
            modelElement.GetAttributeValues(Attribute).Add(item);
        }

        public object ConvertFromString(string text)
        {
            if (Attribute.Type is IEnumeration enumeration)
            {
                return text;
            }
            return Attribute.Type.Parse(text);
        }

        public string ConvertToString(object input)
        {
            if (Attribute.Type is IEnumeration enumeration)
            {
                return input.ToString();
            }
            return Attribute.Type.Serialize(input);
        }

        public object GetValue(object input, XmlSerializationContext context)
        {
            var modelElement = (IModelElement)input;
            if (Attribute.UpperBound == 1)
            {
                return modelElement.GetAttributeValue(Attribute);
            }
            return modelElement.GetAttributeValues(Attribute);
        }

        public void Initialize(object input, XmlSerializationContext context)
        {
            if (Attribute.UpperBound != 1)
            {
                var modelElement = (IModelElement)input;
                modelElement.GetAttributeValues(Attribute).Clear();
            }
        }

        public void SetValue(object input, object value, XmlSerializationContext context)
        {
            var modelElement = (IModelElement)input;
            modelElement.SetAttributeValue(Attribute, value);
        }

        public bool ShouldSerializeValue(object obj, object value)
        {
            var mappedType = Attribute.Type.GetExtension<MappedType>();
            if (mappedType != null && mappedType.SystemType.IsValueType)
            {
                return !object.Equals(value, Activator.CreateInstance(mappedType.SystemType));
            }
            return value != null;
        }

        public bool RequiresInitialization => Attribute.UpperBound != 1 && Attribute.DefaultValue != null;

        public object DefaultValue => Attribute.DefaultValue != null ? Attribute.Type.Parse(Attribute.DefaultValue) : null;
    }
}
