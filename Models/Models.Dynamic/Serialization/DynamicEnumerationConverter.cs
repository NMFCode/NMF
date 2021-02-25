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
    /// <summary>
    /// Denotes a type converter that dynamically converts strings to enumeration literals and back
    /// </summary>
    public class DynamicEnumerationConverter : TypeConverter
    {
        /// <summary>
        /// The enumeration for this converter
        /// </summary>
        public IEnumeration Enumeration { get; }

        /// <summary>
        /// Creates a new converter for the given enumeration
        /// </summary>
        /// <param name="enumeration">The enumeration</param>
        public DynamicEnumerationConverter(IEnumeration enumeration)
        {
            Enumeration = enumeration;
        }

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
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }
            return Enumeration.Literals.FirstOrDefault(l => string.Equals(value.ToString(), l.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var literal = value as ILiteral;
            return literal?.Name;
        }
    }
}
