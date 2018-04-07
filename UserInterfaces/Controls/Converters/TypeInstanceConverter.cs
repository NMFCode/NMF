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
    public class TypeInstanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var property = value as PropertyItem;
            if (property == null) return null;
            return (property.ParentElement as PropertyView).GetPossibleItemsFor(property);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
