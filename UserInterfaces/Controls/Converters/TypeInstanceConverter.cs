using System;
using System.Globalization;
#if Avalonia
using Avalonia.Data.Converters;
#else
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
#endif

namespace NMF.Controls.Converters
{
    /// <summary>
    /// Denotes a converter that gets the possible types for a property item element
    /// </summary>
    public class TypeInstanceConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if Avalonia
            return null;
#else
            if (!(value is PropertyItem property)) return null;
            return (property.ParentElement as PropertyView).GetPossibleItemsFor(property);
#endif
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
