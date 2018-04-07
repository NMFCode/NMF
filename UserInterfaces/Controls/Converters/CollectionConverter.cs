﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NMF.Controls.Converters
{
    public class CollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // collection is always the first
            var collection = values[0] as IEnumerable<object>;
            if (collection == null) return null;
            return string.Join(", ", collection.Select(o => o != null ? o.ToString() : "(null)"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
