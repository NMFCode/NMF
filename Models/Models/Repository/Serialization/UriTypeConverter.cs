using System;
using System.ComponentModel;
using System.Globalization;

namespace NMF.Models.Repository.Serialization
{
    internal class UriTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string uriString)
            {
                return new Uri(uriString, UriKind.RelativeOrAbsolute);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Uri uri && destinationType == typeof(string))
            {
                return uri.OriginalString ?? uri.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
