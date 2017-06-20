using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Company.Welcome.Views.Converters
{
    public class ListViewSelectedItemSelectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var listView = parameter as ListView;
            return listView.SelectedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}