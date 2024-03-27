using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NMF.Controls.Converters
{
    /// <summary>
    /// Denotes a common class to convert collections
    /// </summary>
    public class CollectionConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // collection is always the first
            if (!(values[0] is IEnumerable<object> collection)) return null;
            return string.Join(", ", collection.Select(o => o != null ? o.ToString() : "(null)"));
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
