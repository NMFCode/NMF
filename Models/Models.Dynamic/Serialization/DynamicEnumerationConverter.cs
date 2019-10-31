using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using NMF.Models.Meta;
using Type = System.Type;

namespace NMF.Models.Dynamic.Serialization
{
    public class DynamicEnumerationConverter : TypeConverter
    {
        public IEnumeration Enumeration { get; }

        public DynamicEnumerationConverter(IEnumeration enumeration)
        {
            Enumeration = enumeration;
        }

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
            if (value == null)
            {
                return null;
            }
            return Enumeration.Literals.FirstOrDefault(l => string.Equals(value.ToString(), l.Name, StringComparison.OrdinalIgnoreCase));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var literal = value as ILiteral;
            return literal?.Name;
        }
    }
}
