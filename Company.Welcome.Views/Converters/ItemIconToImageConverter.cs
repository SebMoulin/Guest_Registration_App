using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Company.Welcome.Views.Converters
{
    public class ItemIconToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var img = new BitmapImage();
            //images:
            //http://media.blizzard.com/d3/icons/<type>/<size>/<icon>.png
            //The type can be "items" or "skills" based on the type of icon. 
            //For items size can be "small" or "large" and for skills size can be 21, 42 or 64.
            var uri = string.Format("http://media.blizzard.com/d3/icons/{0}/{1}/{2}.png", "items", parameter, value);
            img.UriSource = new Uri(uri);
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
