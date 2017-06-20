using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Company.Welcome.Commons
{
    public class EncodingHelper : IEncodingHelper
    {
        public async Task<string> ToBase64(Image control)
        {
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(control);
            return await ToBase64(bitmap);
        }

        public async Task<string> ToBase64(WriteableBitmap bitmap)
        {
            var bytes = bitmap.PixelBuffer.ToArray();
            return await ToBase64(bytes, (uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth);
        }

        public async Task<string> ToBase64(StorageFile bitmap)
        {
            var stream = await bitmap.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var pixels = await decoder.GetPixelDataAsync();
            var bytes = pixels.DetachPixelData();
            return await ToBase64(bytes, (uint)decoder.PixelHeight, (uint)decoder.PixelWidth, decoder.DpiX, decoder.DpiY);
        }

        public async Task<string> ToBase64(RenderTargetBitmap bitmap)
        {
            var bytes = (await bitmap.GetPixelsAsync()).ToArray();
            return await ToBase64(bytes, (uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth);
        }

        public async Task<string> ToBase64(byte[] image, uint height, uint width, double dpiX = 96, double dpiY = 96)
        {
            // encode image
            var encoded = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, encoded);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                width,
                height,
                dpiX,
                dpiY,
                image);
            await encoder.FlushAsync();
            encoded.Seek(0);

            // read bytes
            var bytes = new byte[encoded.Size];
            await encoded.AsStream().ReadAsync(bytes, 0, bytes.Length);

            // create base64
            return Convert.ToBase64String(bytes);
        }

        public async Task<ImageSource> FromBase64(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            using (var image = bytes.AsBuffer().AsStream().AsRandomAccessStream())
            {
                // decode image
                var decoder = await BitmapDecoder.CreateAsync(image);
                image.Seek(0);
                // create bitmap
                var output = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await output.SetSourceAsync(image);
                return output;
            }

        }
    }
}