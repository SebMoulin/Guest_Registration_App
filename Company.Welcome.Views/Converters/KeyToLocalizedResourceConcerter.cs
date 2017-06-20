using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ServiceLocation;
using Company.Welcome.Commons;

namespace Company.Welcome.Views.Converters
{
    public class KeyToLocalizedResourceConcerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var localizedStringProvider = ServiceLocator.Current.GetInstance<IProvideLocalizedString>();
            var retrievedValue = localizedStringProvider.GetFor((string) value);
            return !string.IsNullOrWhiteSpace(retrievedValue) ? retrievedValue : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
