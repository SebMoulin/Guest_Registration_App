using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Company.Welcome.Views.Converters
{
    public class TimeExtractorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var datetimeValue = (DateTime)value;
            //var sqlLiteMinDate = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
            //    5,0,0);
            return datetimeValue.Equals(DateTime.MinValue) ? string.Empty : datetimeValue.ToString("t");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
