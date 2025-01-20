using System;
using System.ComponentModel;
using System.Globalization;

namespace NMF.Models
{
    /// <summary>
    /// Denotes a type converter that converts date time to string in iso format
    /// </summary>
    public class IsoDateTimeConverter : System.ComponentModel.DateTimeConverter
    {
        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is DateTime dateTime && destinationType == typeof(string))
            {
                if (dateTime == default) return string.Empty;
                return dateTime.ToString("s");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
