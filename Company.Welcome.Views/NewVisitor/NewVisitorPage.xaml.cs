using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Practices.ServiceLocation;
using Company.Welcome.Commons;
using Company.Welcome.ViewModels.NewVisitor;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Company.Welcome.Views.NewVisitor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewVisitorPage : NavigationPage
    {
        public NewVisitorPage()
        {
            this.InitializeComponent();
            this.LoadViewModel<ApplicationPages>();

            var inkAttributes = new InkDrawingAttributes()
            {
                Color = Colors.Black,
                Size = new Size(4, 4)
            };

            SignatureInkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkAttributes);
            SignatureInkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse
                | CoreInputDeviceTypes.Pen
                | CoreInputDeviceTypes.Touch;
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var signature = await PrepareSignatureImage();
            var vm = this.DataContext as NewVisitorViewModel;
            if (vm != null)
            {
                var bitmap = new RenderTargetBitmap();
                await bitmap.RenderAsync(ImageFrame);
                await vm.RegisterVisitor(signature, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight);
            }
        }

        private async Task<Signature> PrepareSignatureImage()
        {
            var signature = new Signature();
            var device = CanvasDevice.GetSharedDevice();
            var bitmap = new RenderTargetBitmap();

            await bitmap.RenderAsync(ImageFrame);
            var pixels = (await bitmap.GetPixelsAsync()).ToArray();

            var cBitMap = CanvasBitmap.CreateFromBytes(
                device,
                pixels,
                325,
                100,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DisplayInformation.GetForCurrentView().LogicalDpi);

            var strokes = SignatureInkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            var renderTarget = new CanvasRenderTarget(
                device,
                325,
                100,
                96);

            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.White);
                ds.DrawImage(cBitMap);
                ds.DrawInk(strokes);
            }
            signature.Dpi = 96;
            signature.ImageWidth = 325;
            signature.ImageHeight = 100;
            signature.PixelWidth = 325;
            signature.PixelHeight = 100;
            signature.ImageBytes = renderTarget.GetPixelBytes();
            //var encoded = new InMemoryRandomAccessStream();
            //var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, encoded);

            //encoder.SetPixelData(
            //        BitmapPixelFormat.Bgra8,
            //        BitmapAlphaMode.Ignore,
            //        (uint)bitmap.PixelWidth,
            //        (uint)bitmap.PixelHeight,
            //        DisplayInformation.GetForCurrentView().LogicalDpi,
            //        DisplayInformation.GetForCurrentView().LogicalDpi,
            //        test.ToArray());

            //await encoder.FlushAsync();
            return signature;
        }

        private void ResetSignatureButton_OnClick(object sender, RoutedEventArgs e)
        {
            SignatureInkCanvas.InkPresenter.StrokeContainer.Clear();
        }
    }
}
