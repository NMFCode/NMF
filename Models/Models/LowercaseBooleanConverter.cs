using System;
using System.ComponentModel;
using System.Globalization;

namespace NMF.Models
{
    /// <summary>
    /// Denotes a type converter that converts booleans to lower case strings
    /// </summary>
    public class LowercaseBooleanConverter : System.ComponentModel.BooleanConverter
    {
        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is bool boolean)
            {
                return value.ToString().ToLowerInvariant();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
