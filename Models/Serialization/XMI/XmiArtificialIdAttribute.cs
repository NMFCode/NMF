using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization.Xmi
{
    /// <summary>
    /// Denotes an artificially introduced XMI Id attribute
    /// </summary>
    public class XmiArtificialIdAttribute : IPropertySerializationInfo
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected XmiArtificialIdAttribute() { }
        private static readonly XmiArtificialIdAttribute instance = new XmiArtificialIdAttribute();

        /// <summary>
        /// Denotes the default instance
        /// </summary>
        public static XmiArtificialIdAttribute Instance { get { return instance; } }

        /// <inheritdoc />
        public bool ShallCreateInstance
        {
            get { return true; }
        }

        /// <inheritdoc />
        public string ElementName
        {
            get { return "id"; }
        }

        /// <inheritdoc />
        public string Namespace
        {
            get { return XmiSerializer.XMINamespace; }
        }

        /// <inheritdoc />
        public string NamespacePrefix
        {
            get
            {
                return XmiSerializer.XMIPrefix;
            }
        }

        /// <inheritdoc />
        public virtual bool ShouldSerializeValue(object obj, object value)
        {
            return true;
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public object GetValue(object input, XmlSerializationContext context)
        {
            if (context is XmiSerializationContext)
            {
                return ((XmiSerializationContext)context).GetId(input);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public void SetValue(object input, object value, XmlSerializationContext context)
        {
            if (context is XmiSerializationContext)
            {
                ((XmiSerializationContext)context).SetId(input, value.ToString());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public bool IsIdentifier
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public XmlIdentificationMode IdentificationMode
        {
            get { return XmlIdentificationMode.FullObject; }
        }

        /// <inheritdoc />
        public bool IsStringConvertible
        {
            get { return true; }
        }

        /// <inheritdoc />
        public object ConvertFromString(string text)
        {
            return text;
        }

        /// <inheritdoc />
        public string ConvertToString(object input)
        {
            return input.ToString();
        }

        /// <inheritdoc />
        public void AddToCollection(object input, object item, XmlSerializationContext context)
        {
            throw new InvalidOperationException();
        }

        ITypeSerializationInfo IPropertySerializationInfo.PropertyType
        {
            get { return XmiStringSerializationInfo.Instance; }
        }

        /// <inheritdoc />
        public IPropertySerializationInfo Opposite
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public Type PropertyMinType
        {
            get
            {
                return typeof(string);
            }
        }
    }
}
