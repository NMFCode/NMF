using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NMF.Controls.Converters
{
    public class ClassColorConverter : IValueConverter
    {
        private static Brush[] colors =
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

        private Dictionary<Type, Brush> savedColors = new Dictionary<Type, Brush>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Colors.Transparent;
            var type = value.GetType();
            Brush registeredColor;
            if (!savedColors.TryGetValue(type, out registeredColor))
            {
                registeredColor = colors[savedColors.Count % colors.Length];
                savedColors.Add(type, registeredColor);
            }
            return registeredColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
