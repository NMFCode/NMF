using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models
{
    public class IsoDateTimeConverter : System.ComponentModel.DateTimeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string) || !(value is DateTime))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            DateTime dateTime = (DateTime)value;
            if (dateTime == default(DateTime)) return string.Empty;
            return dateTime.ToString("s");
        }
    }
}
