using System;
using NMF.Models.Meta;
using NMF.Serialization;
using Type = System.Type;

namespace NMF.Models.Dynamic.Serialization
{
    internal class DynamicReferenceSerializationInfo : IPropertySerializationInfo
    {
        public DynamicReferenceSerializationInfo(IReference reference, ITypeSerializationInfo type)
        {
            Reference = reference;
            PropertyType = type;
        }

        public string ElementName => Reference.Name;

        public string Namespace => null;

        public string NamespacePrefix => null;

        public bool ShallCreateInstance => Reference.IsContainment;

        public bool IsReadOnly => false;

        public bool IsIdentifier => false;

        public XmlIdentificationMode IdentificationMode => XmlIdentificationMode.AsNeeded;

        public ITypeSerializationInfo PropertyType { get; }

        public Type PropertyMinType => typeof(ModelElement);

        public IPropertySerializationInfo Opposite { get; internal set; }

        public bool IsStringConvertible => false;

        public IReference Reference { get; }

        public void AddToCollection(object input, object item, XmlSerializationContext context)
        {
            if (!context.IsBlocked(input, this))
            {
                var modelElement = (ModelElement)input;
                modelElement.GetReferencedElements(Reference).Add(item);
                if (Opposite != null)
                {
                    context.BlockProperty(item, Opposite);
                }
            }
        }

        public object ConvertFromString(string text)
        {
            throw new NotSupportedException();
        }

        public string ConvertToString(object input)
        {
            throw new NotSupportedException();
        }

        public object GetValue(object input, XmlSerializationContext context)
        {
            var modelElement = (ModelElement)input;
            if (Reference.UpperBound == 1)
            {
                return modelElement.GetReferencedElement(Reference);
            }
            return modelElement.GetReferencedElements(Reference);
        }

        public void Initialize(object input, XmlSerializationContext context)
        {
        }

        public void SetValue(object input, object value, XmlSerializationContext context)
        {
            if (!context.IsBlocked(input, this))
            {
                var modelElement = (ModelElement)input;
                modelElement.SetReferencedElement(Reference, value as IModelElement);
                if (Opposite != null)
                {
                    context.BlockProperty(value, Opposite);
                }
            }
        }

        public bool ShouldSerializeValue(object obj, object value)
        {
            return value != null && (Reference.Opposite == null || !Reference.Opposite.IsContainment);
        }

        public bool RequiresInitialization => false;

        public object DefaultValue => null;
    }
}
