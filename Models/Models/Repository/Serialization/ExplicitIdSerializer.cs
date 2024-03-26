using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NMF.Serialization;
using NMF.Serialization.Xmi;

namespace NMF.Models.Repository.Serialization
{
    /// <summary>
    /// Denotes a serializer that is able to read and understand all model URIs but serializes using XMI IDs
    /// </summary>
    public class ExplicitIdSerializer : ModelSerializer
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ExplicitIdSerializer()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="settings">the serializer settings</param>
        public ExplicitIdSerializer(XmlSerializationSettings settings) : base(settings)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the parent serializer</param>
        public ExplicitIdSerializer(XmlSerializer parent) : base(parent)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="settings">the serializer settings</param>
        /// <param name="knownTypes">a collection of known types</param>
        public ExplicitIdSerializer(XmlSerializationSettings settings, IEnumerable<Type> knownTypes) : base(settings, knownTypes)
        {
        }

        /// <inheritdoc />
        protected override string GetAttributeValue(object value, ITypeSerializationInfo info, bool isCollection, XmlSerializationContext context)
        {
            if (value is IModelElement modelElement && modelElement.Model == context.Root)
            {
                return info.IdentifierProperty.GetValue(value, context).ToString();
            }
            return base.GetAttributeValue(value, info, isCollection, context);
        }

        /// <inheritdoc />
        protected override bool WriteIdentifiedObject(XmlWriter writer, object obj, XmlIdentificationMode identificationMode, ITypeSerializationInfo info, XmlSerializationContext context)
        {
            return false;
        }

        /// <inheritdoc />
        protected override IPropertySerializationInfo IdAttribute => XmiArtificialIdAttribute.Instance;
    }
}
