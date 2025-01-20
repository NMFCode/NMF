using System;
using System.ComponentModel;

namespace NMF.Serialization
{
    /// <summary>
    /// Defines a string converter to prevent non xml valid documents
    /// </summary>
    public class XmlStringTypeConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null) return null;
            return value.ToString().Replace("&gt;", ">").Replace("&lt;", "<");
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null) return null;
            return value.ToString().Replace(">", "&gt;").Replace("<", "&lt;");
        }
    }
}
