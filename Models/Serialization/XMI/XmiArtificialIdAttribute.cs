using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization.Xmi
{
    public class XmiArtificialIdAttribute : IPropertySerializationInfo
    {
        protected XmiArtificialIdAttribute() { }
        private static XmiArtificialIdAttribute instance = new XmiArtificialIdAttribute();
        public static XmiArtificialIdAttribute Instance { get { return instance; } }

        public bool ShallCreateInstance
        {
            get { return true; }
        }

        public string ElementName
        {
            get { return "id"; }
        }

        public string Namespace
        {
            get { return XmiSerializer.XMINamespace; }
        }

        public string NamespacePrefix
        {
            get
            {
                return XmiSerializer.XMIPrefix;
            }
        }

        public virtual bool ShouldSerializeValue(object obj, object value)
        {
            return true;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

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

        public bool IsIdentifier
        {
            get
            {
                return true;
            }
        }

        public XmlIdentificationMode IdentificationMode
        {
            get { return XmlIdentificationMode.FullObject; }
        }

        public bool IsStringConvertible
        {
            get { return true; }
        }

        public object ConvertFromString(string text)
        {
            return text;
        }

        public string ConvertToString(object input)
        {
            return input.ToString();
        }

        ITypeSerializationInfo IPropertySerializationInfo.PropertyType
        {
            get { return XmiStringSerializationInfo.Instance; }
        }
    }
}
