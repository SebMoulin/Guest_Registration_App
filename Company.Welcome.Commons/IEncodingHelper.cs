using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Company.Welcome.Commons
{
    public interface IEncodingHelper
    {
        Task<string> ToBase64(Image control);
        Task<string> ToBase64(WriteableBitmap bitmap);
        Task<string> ToBase64(StorageFile bitmap);
        Task<string> ToBase64(RenderTargetBitmap bitmap);
        Task<string> ToBase64(byte[] image, uint height, uint width, double dpiX = 96, double dpiY = 96);
        Task<ImageSource> FromBase64(string base64);
    }
}