using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if Avalonia
using Avalonia.Data.Converters;
#else
using System.Windows.Data;
#endif

namespace NMF.Controls.Converters
{
    /// <summary>
    /// Denotes a common class to convert collections
    /// </summary>
    public class CollectionConverter : IMultiValueConverter
    {
#if Avalonia
        /// <inheritdoc />
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
#else
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
#endif
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
