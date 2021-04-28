using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;

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
            if (!(value is PropertyItem property)) return null;
            return (property.ParentElement as PropertyView).GetPossibleItemsFor(property);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
