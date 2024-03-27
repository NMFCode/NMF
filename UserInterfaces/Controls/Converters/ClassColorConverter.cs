using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NMF.Controls.Converters
{
    /// <summary>
    /// Denotes a simple converter that converts model elements to colors by their type
    /// </summary>
    public class ClassColorConverter : IValueConverter
    {
        private static readonly Brush[] colors =
        {
            Brushes.DarkMagenta,
            Brushes.DarkOrange,
            Brushes.DarkGreen,
            Brushes.DarkKhaki,
            Brushes.DarkOrchid,
            Brushes.DarkRed,
            Brushes.DarkGoldenrod,
            Brushes.DarkCyan,
            Brushes.DarkBlue,
            Brushes.DarkSalmon,
            Brushes.DarkSeaGreen,
            Brushes.DarkSlateBlue,
            Brushes.DarkTurquoise,
            Brushes.DarkViolet
        };

        private readonly Dictionary<Type, Brush> savedColors = new Dictionary<Type, Brush>();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Colors.Transparent;
            var type = value.GetType();
            if (!savedColors.TryGetValue(type, out Brush registeredColor))
            {
                registeredColor = colors[savedColors.Count % colors.Length];
                savedColors.Add(type, registeredColor);
            }
            return registeredColor;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
